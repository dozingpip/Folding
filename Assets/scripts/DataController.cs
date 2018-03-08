using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class DataController : MonoBehaviour {
	private Fold fold;
	public Mesh meshToEdit;

	private string foldDataFileName = "flappingBird _ 70PercentFolded.fold";
	// Use this for initialization
	public void Start () {
		DontDestroyOnLoad(gameObject);
		LoadFold();
	}
	
	public void LoadFold(){
		string filePath = Path.Combine(Application.streamingAssetsPath, foldDataFileName);
		if(File.Exists(filePath)){
			string dataAsJson = File.ReadAllText(filePath);
			JSONNode parsed = JSON.Parse(dataAsJson);

			fold = new Fold(parsed);
			Debug.Log(fold.vertices_coords[1][0]);
			Vector3[] meshV = new Vector3[fold.vertices_coords.Length];
			for(int i=0; i<meshV.Length; i++){
				meshV[i] = new Vector3((float)fold.vertices_coords[i][0], (float)fold.vertices_coords[i][1], (float)fold.vertices_coords[i][2]);
			}
			//meshToEdit.vertices = meshV;
		}
	}

	public class Fold {
		public string file_spec;
		public string file_creator;
		public string file_author;
		public string frame_title;
		public string frame_unit;
		public double[][] vertices_coords;
		public int[][] edges_vertices;
		public string[] edges_assignment;
		public string[] frame_classes;
		public string[] frame_attributes;
		public int[][] faces_vertices;
		public double[] edges_foldAngles;
		public Fold(JSONNode n){
			file_spec = n["file_spec"].Value;
			file_creator = n["file_creator"].Value;
			file_author = n["file_author"].Value;
			frame_title = n["frame_title"].Value;
			frame_unit = n["frame_unit"].Value;

			JSONNode vc = n["vertices_coords"];
			vertices_coords = new double[vc.Count][];
			for(int i=0; i<vc.Count; i++){
				vertices_coords[i] = new double[vc[i].Count];
				for(int j = 0; j<vertices_coords[i].Length; j++){
					vertices_coords[i][j] = vc[i][j].AsDouble;
				}
			}
			
			JSONNode ec = n["edges_vertices"];
			edges_vertices = new int[ec.Count][];
			for(int i=0; i<ec.Count; i++){
				edges_vertices[i] = new int[ec[i].Count];
				for(int j = 0; j<edges_vertices[i].Length; j++){
					edges_vertices[i][j] = ec[i][j].AsInt;
				}
			}

			JSONNode ea = n["edges_assignment"];
			edges_assignment = new string[ea.Count];
			for(int i=0; i<edges_assignment.Length; i++){
				edges_assignment[i] = ea[i].Value;
			}

			JSONNode fc = n["frame_classes"];
			frame_classes = new string[fc.Count];
			for(int i=0; i<frame_classes.Length; i++){
				frame_classes[i] = fc[i].Value;
			}

			JSONNode fa = n["frame_attributes"];
			frame_attributes = new string[fa.Count];
			for(int i=0; i<frame_attributes.Length; i++){
				frame_attributes[i] = fa[i].Value;
			}


			JSONNode fv = n["faces_vertices"];
			faces_vertices = new int[fv.Count][];
			for(int i=0; i<fv.Count; i++){
				faces_vertices[i] = new int[fv[i].Count];
				for(int j = 0; j<faces_vertices[i].Length; j++){
					faces_vertices[i][j] = fv[i][j].AsInt;
				}
			}

			JSONNode efa = n["edges_foldAngles"];
			edges_foldAngles = new double[efa.Count];
			for(int i=0; i<edges_foldAngles.Length; i++){
				edges_foldAngles[i] = efa[i].AsDouble;
			}
		}
	}
}
