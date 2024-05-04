using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGlobalPath : MonoBehaviour
{
    public List<VectorEdge> edges = new List<VectorEdge>();

    public static Graph worldPaths;

    void Start()
    {
        HashSet<Vector3> vertices = new HashSet<Vector3>();
        foreach (VectorEdge edge in edges)
        {
            vertices.Add(edge.start);
            vertices.Add(edge.end);
        }
        List<Vector3> verticesList = new List<Vector3>(vertices);

        List<GraphEdge> graphEdges = edges.ConvertAll(
            (VectorEdge edge) => edge.ToGraphEdge(verticesList)
        );

        Debug.Log(verticesList.Count);
        
        worldPaths = new Graph(verticesList, graphEdges);
        Debug.Log(worldPaths.GetMatrix());
    }

    public static KeyValuePair<int, Vector3> ClosestVertexTo(Vector3 point)
    {
        return worldPaths.GetClosestPointInGraph(point);
    }

    public static GraphPath PathTo(Vector3 from, Vector3 to)
    {
        KeyValuePair<int, Vector3> start = worldPaths.GetClosestPointInGraph(from);
        KeyValuePair<int, Vector3> end = worldPaths.GetClosestPointInGraph(to);

        return worldPaths.GetRandomPathTo(start.Key, end.Key);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach (VectorEdge edge in edges)
        {
            Gizmos.DrawLine(edge.start, edge.end);
            Gizmos.DrawSphere(edge.start, 2f);
            Gizmos.DrawCube(edge.end, Vector3.one * 2f);
        }
    }
}

[Serializable]
public class VectorEdge
{
    public Vector3 start;
    public Vector3 end;

    public VectorEdge(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }

    public GraphEdge ToGraphEdge(List<Vector3> vertices)
    {
        return new GraphEdge(vertices.IndexOf(start), vertices.IndexOf(end));
    }
}
