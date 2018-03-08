using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class Fold {
	public Vector3[] vertices_coords;
	public int[][] faces_vertices;


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

    }
}