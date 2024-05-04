using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Graph
{
    public List<Vector3> vertices;
    public SquareMatrix<float> distanceMatrix;

    public Graph(IEnumerable<Vector3> vertices, IEnumerable<GraphEdge> edges)
    {
        this.vertices = new List<Vector3>(vertices);
        distanceMatrix = new SquareMatrix<float>(this.vertices.Count, -1f);

        foreach (GraphEdge edge in edges) 
        {
            distanceMatrix.SetValue(edge.start, edge.end, (vertices.ElementAt(edge.start) - vertices.ElementAt(edge.end)).sqrMagnitude);
            distanceMatrix.SetValue(edge.end, edge.start, (vertices.ElementAt(edge.start) - vertices.ElementAt(edge.end)).sqrMagnitude);
        }
    }

    public int VertexCount()
    {
        return vertices.Count;
    }

    public Vector3 GetVertex(int index)
    {
        return vertices[index];
    }

    public Dictionary<MatrixPosition, float> GetNeighbours(int i, int j)
    {
        Dictionary<MatrixPosition, float> pairs = new Dictionary<MatrixPosition, float>();
        foreach (MatrixPosition position in distanceMatrix.GetNeighbourPositions(i, j))
            pairs.Add(position, distanceMatrix.GetValue(position.i, position.j));

        return pairs;
    }

    public GraphPath GetShortestPathTo(int start, int end)
    {
        GraphPath path = GetShortestPath(distanceMatrix.DeepCopy(), start, end);
        return path;
    }

    public GraphPath GetRandomPathTo(int start, int end)
    {
        GraphPath path = GetRandomPath(distanceMatrix.DeepCopy(), start, end);
        return path;
    }

    public Vector3 GetClosestPointInGraph(Vector3 vector)
    {
        List<KeyValuePair<int, Vector3>> vertices = new List<KeyValuePair<int, Vector3>>();
        for (int i = 0; i < this.vertices.Count; i++)
            vertices.Add(new KeyValuePair<int, Vector3>(i, this.vertices[i]));

        vertices.Sort(
            delegate (KeyValuePair<int, Vector3> a, KeyValuePair<int, Vector3> b)
            {
                float distA = Vector2.Distance(vector, a.Value);
                float distB = Vector2.Distance(vector, b.Value);
                return (distA < distB) ? -1 : (distA > distB) ? 1 : 0;
            }
        );

        return vertices[0].Value;
    }

    static GraphPath GetRandomPath(SquareMatrix<float> distanceMatrix, int start, int end)
    {
        if (start == end)
            return new GraphPath(new List<int> { start }, 0f);

        Dictionary<int, float> distances = new Dictionary<int, float>(distanceMatrix.GetColumnValues(start).Where(
            (KeyValuePair<int, float> pair) => pair.Value >= 0f
        ));
        
        if (distances.Count == 0)
            return new GraphPath();

        distanceMatrix.SetLine(start, -1f);
        distanceMatrix.SetColumn(start, -1f);
        
        List<int> validNeighbours = distances.Keys.ToList();
        GraphPath finalPath = new GraphPath();
        int neighbour = -1;
        while (!finalPath.IsValid() && validNeighbours.Count > 0)
        {
            neighbour = validNeighbours[UnityEngine.Random.Range(0, validNeighbours.Count)];
            validNeighbours.Remove(neighbour);
            finalPath = GetRandomPath(distanceMatrix.DeepCopy(), neighbour, end);
        }

        if (!finalPath.IsValid())
            return new GraphPath();

        finalPath.totalLength += distances[neighbour];
        finalPath.vertices.Add(start);
        return finalPath;
    }

    static GraphPath GetShortestPath(SquareMatrix<float> distanceMatrix, int start, int end)
    {
        if (start == end)
            return new GraphPath(new List<int> { start }, 0f);

        Dictionary<int, float> distances = new Dictionary<int, float>(distanceMatrix.GetColumnValues(start).Where(
            (KeyValuePair<int, float> pair) => pair.Value >= 0f
        ));
        
        if (distances.Count == 0)
            return new GraphPath();

        distanceMatrix.SetLine(start, -1f);
        distanceMatrix.SetColumn(start, -1f);
        
        List<KeyValuePair<int, GraphPath>> neighbourPaths = new List<int>(distances.Keys)
            .ConvertAll((int neighbour) => new KeyValuePair<int, GraphPath>(neighbour, GetShortestPath(distanceMatrix.DeepCopy(), neighbour, end)))
            .Where((KeyValuePair<int, GraphPath> pair) => pair.Value.IsValid())
            .OrderBy((KeyValuePair<int, GraphPath> pair) => pair.Value.totalLength)
            .ToList();

        if (neighbourPaths.Count == 0)
            return new GraphPath();

        neighbourPaths[0].Value.totalLength += distances[neighbourPaths[0].Key];
        neighbourPaths[0].Value.vertices.Add(start);
        return neighbourPaths[0].Value;
    }

    public SquareMatrix<float> GetMatrix()
    {
        return distanceMatrix;
    }
}

[Serializable]
public class GraphEdge
{
    public int start;
    public int end;

    public GraphEdge(int start, int end)
    {
        this.start = start;
        this.end = end;
    }
}

[Serializable]
public class GraphPath
{
    public List<int> vertices;
    public float totalLength;

    public GraphPath()
    {
        vertices = null;
        totalLength = -1f;
    }

    public GraphPath(List<int> vertices, float totalLength)
    {
        this.vertices = vertices;
        this.totalLength = totalLength;
    }

    public bool IsValid()
    {
        return totalLength >= 0f && vertices != null;
    }

    public override string ToString()
    {
        if (!IsValid())
            return "No Path";

        String str = "|";
        foreach (int vertex in vertices)
            str = String.Format("{0} <- {1}", str, vertex);

        return str;
    }
}