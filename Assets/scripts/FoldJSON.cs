using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoldJSON {
	public string file_spec;
	public string file_creator;
	public string file_author;
	public string frame_title;
	public string frame_unit;
	public double[][] vertices_coords;
	public int[] edges_vertices;
	public string[] edges_assignment;
	public string[] frame_classes;
	public string[] frame_attributes;
	public int[] faces_vertices;
	public double[][] edges_foldAngles;
}