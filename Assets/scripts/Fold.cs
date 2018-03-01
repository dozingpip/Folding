using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fold {
	public string file_spec;
	public string file_creator;
	public string file_author;
	public string frame_title;
	public string frame_unit;
	public Vector3[] vertices_coords;
	public int[][] edges_vertices;
	public string[] edges_assignment;
	public string[] frame_classes;
	public string[] frame_attributes;
	public int[] faces_vertices;
	public double[] edges_foldAngles;
	public List<List<int>> vertices_vertices;

	public Fold(string _file_spec,
	 string _file_creator,
	 string _file_author,
	 string _frame_title,
	 string _frame_unit,
	 Vector3[] _vertices_coords,
	 int[][] _edges_vertices,
	 string[] _edges_assignment,
	 string[] _frame_classes,
	 string[] _frame_attributes,
	 int[] _faces_vertices,
	 double[] _edges_foldAngles){
		file_spec = _file_spec;
 		file_creator = _file_creator;
 		file_author = _file_author;
 		frame_title = _frame_title;
 		frame_unit = _frame_unit;
 		vertices_coords = _vertices_coords;
 		edges_vertices = _edges_vertices;
 		edges_assignment = _edges_assignment;
 		frame_classes = _frame_classes;
 		frame_attributes = _frame_attributes;
 		faces_vertices = _faces_vertices;
 		edges_foldAngles = _edges_foldAngles;
	}
}
