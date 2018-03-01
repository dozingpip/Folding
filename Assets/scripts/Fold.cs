using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fold {
	//public string file_spec;
	//string file_creator;
	//public string file_author;
	//public string frame_title;
	//public string frame_unit;
	public Vector3[] vertices_coords;
	//public int[][] edges_vertices;
	//public string[] edges_assignment;
	//public string[] frame_classes;
	//public string[] frame_attributes;
	public int[] faces_vertices;
	//public double[] edges_foldAngles;
	//public List<List<int>> vertices_vertices;

	//public Fold(string _file_spec,
	// string _file_creator,
	// string _file_author,
	// string _frame_title,
	// string _frame_unit,
	// Vector3[] _vertices_coords,
	// int[][] _edges_vertices,
	// string[] _edges_assignment,
	// string[] _frame_classes,
	// string[] _frame_attributes,
	// int[] _faces_vertices,
	// double[] _edges_foldAngles){
	//	file_spec = _file_spec;
 //		file_creator = _file_creator;
 //		file_author = _file_author;
 //		frame_title = _frame_title;
 //		frame_unit = _frame_unit;
 //		vertices_coords = _vertices_coords;
 //		edges_vertices = _edges_vertices;
 //		edges_assignment = _edges_assignment;
 //		frame_classes = _frame_classes;
 //		frame_attributes = _frame_attributes;
 //		faces_vertices = _faces_vertices;
 //		edges_foldAngles = _edges_foldAngles;
	//}

    public Fold(string file_name)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, foldDataFileName);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            JSONNode parsed = JSON.Parse(dataAsJson);

            fold =  createFold(parsed);
            Debug.Log(fold.vertices_coords[1][0]);
            Vector3[] meshV = new Vector3[fold.vertices_coords.Length];
            //something about triangles
            for (int i = 0; i < meshV.Length; i++)
            {
                meshV[i] = new Vector3((float)fold.vertices_coords[i][0], (float)fold.vertices_coords[i][1], (float)fold.vertices_coords[i][2]);
            }
            meshToEdit.vertices = meshV;
        }
    }

    public void createFold(JSONNode n)
    {
        //file_spec = n["file_spec"].Value;
        //file_creator = n["file_creator"].Value;
        //file_author = n["file_author"].Value;
        //frame_title = n["frame_title"].Value;
        //frame_unit = n["frame_unit"].Value;

        JSONNode vc = n["vertices_coords"];
        vertices_coords = new Vector3[vc.Count];
        for (int i = 0; i < vc.Count; i++)
        {
            vertices_coords.x = vc[i][0];
            vertices_coords.y = vc[i][1];
            vertices_coords.z = vc[i][2];
        }

        //JSONNode ec = n["edges_vertices"];
        //edges_vertices = new int[ec.Count][];
        //for (int i = 0; i < ec.Count; i++)
        //{
        //    edges_vertices[i] = new int[ec[i].Count];
        //    for (int j = 0; j < edges_vertices[i].Length; j++)
        //    {
        //        edges_vertices[i][j] = ec[i][j].AsInt;
        //    }
        //}

        //JSONNode ea = n["edges_assignment"];
        //edges_assignment = new string[ea.Count];
        //for (int i = 0; i < edges_assignment.Length; i++)
        //{
        //    edges_assignment[i] = ea[i].Value;
        //}

        //JSONNode fc = n["frame_classes"];
        //frame_classes = new string[fc.Count];
        //for (int i = 0; i < frame_classes.Length; i++)
        //{
        //    frame_classes[i] = fc[i].Value;
        //}

        //JSONNode fa = n["frame_attributes"];
        //frame_attributes = new string[fa.Count];
        //for (int i = 0; i < frame_attributes.Length; i++)
        //{
        //    frame_attributes[i] = fa[i].Value;
        //}


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

        //JSONNode efa = n["edges_foldAngles"];
        //edges_foldAngles = new double[efa.Count];
        //for (int i = 0; i < edges_foldAngles.Length; i++)
        //{
        //    edges_foldAngles[i] = efa[i].AsDouble;
        //}
    }
}
