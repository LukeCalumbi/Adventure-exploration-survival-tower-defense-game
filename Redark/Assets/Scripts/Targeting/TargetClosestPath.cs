using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class TargetClosestPath : TargetingSystem
{
    public TargetingSystem support;
    public float tolerance = 3f;
    public int pathUpdateCount = 3;

    private readonly Path _cachedPath = new Path();
    private readonly NodePathGenerator _nodePathGenerator = 
        new NodePathGenerator(Vector2.negativeInfinity, Vector2.positiveInfinity, String.Empty);

    public override void UpdateTarget()
    {
        UpdatePath();

        if (_cachedPath.IsEmpty())
        {
            cachedTarget = null;
            cachedObject = null;
            return;
        }

        cachedTarget = _cachedPath.GetStartingPoint()!;
        cachedObject = support.GetTargetObject()!;
    }

    private void UpdatePath()
    {
        Vector3? target = support.GetTarget();
        GameObject targetObject = support.GetTargetObject();
        if (target == null)
        {
            _cachedPath.Clear();
            return;
        }

        var end = _nodePathGenerator.GetEnd();
        var distance = Vector2.Distance(end, target.Value);
        if (distance > tolerance)
        {
            MakeNewPath(target.Value, targetObject.tag);
            return;
        }

        if (_cachedPath.GetPointCount() <= 1)
        {
            for (int i = 0; i < pathUpdateCount; i++)
                _nodePathGenerator.Next();

            var current = _nodePathGenerator.GetCurrentClosestNode();
            try
            {
                _cachedPath.Merge(ref current);
            }
            catch
            {
                MakeNewPath(target.Value, targetObject.tag);
                
                for (int i = 0; i < pathUpdateCount; i++)
                    _nodePathGenerator.Next();
                
                return;
            }
        }

        KeepOnPath();
    }

    private void KeepOnPath()
    {
        var start = _cachedPath.GetStartingPoint()!.Value;
        var distanceToStart = Vector2.Distance(start, transform.position);

        if (distanceToStart < 0.4f * GridSnapping.TILE_SIZE)
            _cachedPath.PopStart();

        var next = _cachedPath.GetStartingPoint();
        if (next is null)
            return;

        cachedTarget = next.Value;
    }

    private void MakeNewPath(Vector3 target, string targetTag)
    {
        var position = GridSnapping.ClosestSnapPointOf(transform.position);
        _nodePathGenerator.ChangePath(position, GridSnapping.ClosestSnapPointOf(target), targetTag);
        
        _cachedPath.Clear();
        _cachedPath.PushEnd(position);
    }

    private void OnDrawGizmos()
    {
        for (int depth = _cachedPath.GetVertices().Count - 1; depth >= 0; depth--)
        {
            Gizmos.color = Color.blue * (_cachedPath.GetVertices().Count - depth) + Color.green * depth;
            Gizmos.DrawCube(_cachedPath.GetVertices()[depth], Vector3.one * GridSnapping.TILE_SIZE);
        }
    }
}

class Path
{
    private List<Vector3> _vertices;

    public Path()
    {
        _vertices = new List<Vector3>();
    }

    public Path(IEnumerable<Vector3> vertices)
    {
        _vertices = vertices.ToList();
    }

    public Path(ref Node node)
    {
        _vertices = new List<Vector3>();

        var current = node;
        while (current is not null)
        {
            _vertices.Add(current!.GetPosition());
            current = current.GetPrevious();
        }
    }

    public void Merge(ref Node node)
    {
        var nodes = new Stack<Vector2>();

        var current = node;
        while (true)
        {
            if (current == null)
                throw new ArgumentException("Paths are not mergeable");
            
            var end = GetFinalPoint();
            var preEnd = GetPreFinalPoint();
            var nodePosition = current!.GetPosition();

            var endDistance = Vector2.Distance(end ?? Vector2.positiveInfinity, nodePosition);
            var preEndDistance = Vector2.Distance(preEnd ?? Vector2.positiveInfinity, nodePosition);
            
            if (endDistance <= 0.5f * GridSnapping.TILE_SIZE)
                break;

            if (endDistance <= GridSnapping.TILE_SIZE)
            {
                nodes.Push(current.GetPosition());
                break;
            }
            
            if (preEnd != null && endDistance > preEndDistance)
                PopEnd();
            
            nodes.Push(current.GetPosition());
            current = current.GetPrevious();
        }

        var final = GetFinalPoint();
        while (nodes.Count > 0)
        {
            Vector3 position = nodes.Pop();
            position.z = (final ?? Vector3.zero).z;
            PushEnd(position);
        }
    }

    public void PushEnd(Vector3 position)
    {
        _vertices.Insert(0, position);
    }

    public void PopStart()
    {
        if (IsEmpty())
            return;
        
        _vertices.RemoveAt(_vertices.Count - 1);
    }

    public void PopEnd()
    {
        if (IsEmpty())
            return;
        
        _vertices.RemoveAt(0);
    }

    public void Clear()
    {
        _vertices.Clear();
    }

    public int GetPointCount() => _vertices.Count;
    public Vector3? GetStartingPoint() => IsEmpty() ? null : _vertices[^1];
    public Vector3? GetFinalPoint() => IsEmpty() ? null : _vertices[0];
    public Vector3? GetPreFinalPoint() => _vertices.Count < 2 ? null : _vertices[1];

    public bool IsEmpty() => _vertices.Count == 0;
    public List<Vector3> GetVertices() => _vertices;
}

class NodePathGenerator : IComparer<Node>
{
    private string _targetTag;
    private Vector2 _start;
    private Vector2 _end;

    private readonly Vector2[] _directions =
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };
    
    private readonly SortedList<Node, float> _open;
    private readonly List<Node> _closed;

    public NodePathGenerator(Vector2 start, Vector2 end, string targetTag)
    {
        _targetTag = targetTag;
        _start = start;
        _end = end;

        _open = new SortedList<Node, float>(this);
        _closed = new List<Node>();

        var startNode = new Node(_start);
        _open.Add(startNode, TotalEstimatedCost(ref startNode));
    }

    public void Next()
    {
        if (_open.Count == 0)
            return;

        var current = GetCurrentClosestNode();
        if (NodeIsAtPosition(ref current, _end))
            return;
        
        _open.RemoveAt(0);
        _closed.Add(current);

        foreach (var direction in _directions)
        {
            Vector2 neighbourPosition = GridSnapping.ClosestSnapPointOf(current.GetPosition() + direction * GridSnapping.TILE_SIZE);
            if (_closed.Any(closedNode => NodeIsAtPosition(ref closedNode, neighbourPosition)))
                continue;
                
            Collider2D hit = Physics2D.OverlapBox(neighbourPosition,
                Vector2.one * (GridSnapping.TILE_SIZE - 0.1f), 0f);
                
            if (hit is not null && !hit.gameObject.CompareTag(_targetTag))
                continue;

            try
            {
                Node openNeighbour = _open.First(delegate(KeyValuePair<Node, float> openNode)
                {
                    Node node = openNode.Key;
                    return NodeIsAtPosition(ref node, neighbourPosition);
                }).Key;
                
                openNeighbour.SetPrevious(ref current);
            }
            catch
            {
                Node neighbour = new Node(neighbourPosition, ref current);
                _open.Add(neighbour, TotalEstimatedCost(ref neighbour));
            }
        }
    }

    public void ChangePath(Vector2 start, Vector2 end, string targetTag)
    {
        _targetTag = targetTag;
        _start = start;
        _end = end;
        
        _open.Clear();
        _closed.Clear();
        
        var startNode = new Node(_start);
        _open.Add(startNode, TotalEstimatedCost(ref startNode));
    }

    [CanBeNull]
    public Node GetCurrentClosestNode() => _open.Count == 0 ? null : _open.First().Key;
    
    public float DistanceToStart(Vector2 node)
        => Vector2.Distance(node, _start);
    
    public float DistanceToEnd(Vector2 node)
        => Vector2.Distance(node, _end);

    public Vector2 GetStart() => _start;
    public Vector2 GetEnd() => _end;

    private float FromStartCost(ref Node node)
        => TileDistance(node.GetPosition(), _start);

    private float ToEndCost(ref Node node)
        => TileDistance(node.GetPosition(), _end);

    private float TotalEstimatedCost(ref Node node)
        => FromStartCost(ref node) + ToEndCost(ref node);

    private static float TileDistance(Vector2 node, Vector2 start)
        => Mathf.Abs(node.x - start.x) + Mathf.Abs(node.y - start.y);
    
    private const float TILE_DISTANCE_EPSILON = 0.001f;

    private static bool AreSameNode(ref Node a, ref Node b)
        => Vector2.Distance(a.GetPosition(), b.GetPosition()) <= TILE_DISTANCE_EPSILON;

    private static bool NodeIsAtPosition(ref Node node, Vector2 position)
        => Vector2.Distance(node.GetPosition(), position) <= TILE_DISTANCE_EPSILON;

    public int Compare(Node a, Node b)
    {
        if (a is null && b is not null)
            return 1;
        
        if (a is not null && b is null)
            return -1;

        if (a is null && b is null)
            return 0;

        if (AreSameNode(ref a, ref b))
            return 0;

        var costComparison = TotalEstimatedCost(ref a).CompareTo(TotalEstimatedCost(ref b));
        if (costComparison != 0) return costComparison;
        
        var toEndComparison = ToEndCost(ref a).CompareTo(ToEndCost(ref b));
        if (toEndComparison == 0) return -1;
        return toEndComparison;
    }
}

class Node
{
    private readonly Vector2 _position;
    
    [CanBeNull] 
    private Node _previous;

    public Node(Vector2 position)
    {
        _position = position;
        _previous = null;
    }
    
    public Node(Vector2 position, ref Node previous)
    {
        _position = position;
        _previous = previous;
    }

    public void SetPrevious(ref Node newPrevious)
    {
        _previous = newPrevious;
    }

    public Vector2 GetPosition() => _position;
    public Node GetPrevious() => _previous;
}