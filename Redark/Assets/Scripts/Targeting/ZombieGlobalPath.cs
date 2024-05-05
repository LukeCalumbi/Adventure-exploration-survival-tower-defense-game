using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieGlobalPath : MonoBehaviour
{
    public static Graph worldPaths;

    void Start()
    {
        List<Vector3> vertices = new List<Vector3>();
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            vertices.Add(child.position);

        List<GraphEdge> edges = new List<GraphEdge>();
        int startFrom = 1;
        for (int j = 0; j < vertices.Count; j++) 
        {
            for (int i = startFrom; i < vertices.Count; i++) 
            {
                Vector3 start = vertices[j];
                Vector3 end = vertices[i];
                RaycastHit2D ray = Physics2D.Raycast(start, end - start, (end - start).magnitude);

                if (ray.collider == null)
                    edges.Add(new GraphEdge(j, i));
            }
        }

        worldPaths = new Graph(vertices, edges);

        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            Destroy(child.gameObject);
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
        if (worldPaths == null)
            return;

        foreach (MatrixPosition position in worldPaths.distanceMatrix.GetAllPositions())
        {
            Vector3 start = worldPaths.vertices[position.i];
            Vector3 end = worldPaths.vertices[position.j];

            if (worldPaths.distanceMatrix.GetValue(position.i, position.j) > 0)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(start, 1f);

                Gizmos.color = Color.red;
                Gizmos.DrawCube(end, 0.5f * Vector3.one);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(start, end);
            }
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
