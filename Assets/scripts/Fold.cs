using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class Fold {
	public Vector3[] vertices_coords;
	public int[][] faces_vertices;
    public int[][] edges_vertices;


    public Fold(string file_name)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, file_name + ".fold");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            JSONNode parsed = JSON.Parse(dataAsJson);

            createFold(parsed);
        }
    }

    void createFold(JSONNode n)
    {
        JSONNode vc = n["vertices_coords"];
        vertices_coords = new Vector3[vc.Count];
        for (int i = 0; i < vc.Count; i++)
        {
            vertices_coords[i].x = vc[i][0];
            vertices_coords[i].y = vc[i][1];
            vertices_coords[i].z = vc[i][2];
        }

        JSONNode fv = n["faces_vertices"];
        faces_vertices = new int[fv.Count][];
        for (int i = 0; i < fv.Count; i++)
        {
            faces_vertices[i] = new int[fv[i].Count];
            for (int j = 0; j < faces_vertices[i].Length; j++)
            {
                faces_vertices[i][j] = fv[i][j].AsInt;
            }
        }

        JSONNode ev = n["edges_vertices"];
        edges_vertices = new int[ev.Count][];
        for (int i = 0; i < ev.Count; i++)
        {
            edges_vertices[i] = new int[ev[i].Count];
            for (int j = 0; j < edges_vertices[i].Length; j++)
            {
                edges_vertices[i][j] = ev[i][j].AsInt;
            }
        }
    }

    public void update_vertices_coords(Vector3[] newCoords){
        vertices_coords = newCoords;
    }

    public Vector3[] vertices_coords_toArray(){
        List<Vector3> verts = new List<Vector3>();
        for (int i = 0; i < vertices_coords.Length; i++)
        {
            verts.Add(new Vector3( (float)vertices_coords[i][0],
                                   (float)vertices_coords[i][1],
                                   (float)vertices_coords[i][2]));
        }
        return verts.ToArray();
    }
}