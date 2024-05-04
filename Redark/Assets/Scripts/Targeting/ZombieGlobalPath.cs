using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGlobalPath : MonoBehaviour
{
    public List<VectorEdge> edges = new List<VectorEdge>();

    Graph worldPaths;

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

    void FixedUpdate()
    {
        if (worldPaths == null)
            return;

        int start = UnityEngine.Random.Range(0, worldPaths.VertexCount());
        int end = UnityEngine.Random.Range(0, worldPaths.VertexCount());

        Debug.Log(String.Format("de {0} ate {1} grafo diz {2}", worldPaths.GetVertex(start), worldPaths.GetVertex(end), worldPaths.GetRandomPathTo(start, end)));
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
