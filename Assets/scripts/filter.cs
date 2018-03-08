using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class filter{
	// List<int[]> edgesAssigned(Fold fold, String target){
	// 	List<int[]> edgesAssigned = new List<int[]>();
	// 	for(int i = 0; i< fold.edges_assignment.Length; i++){
	// 		if(fold.edges_assignment[i] == target){
	// 			edgesAssigned.Add(fold.edges_vertices[i]);
	// 		}
	// 	}
	// 	return edgesAssigned;
	// }

	// List<int[]> mountainEdges(Fold fold){
	// 	return edgesAssigned(fold, "M");
	// }

	// List<int[]> valleyEdges(Fold fold){
	// 	return edgesAssigned(fold, "V");
	// }

	// List<int[]> flatEdges(Fold fold){
	// 	return edgesAssigned(fold, "F");
	// }

	// List<int[]> boundaryEdges(Fold fold){
	// 	return edgesAssigned(fold, "B");
	// }

	// List<int[]> unassignedEdges(Fold fold){
	// 	return edgesAssigned(fold, "U");
	// }

	// int numType(Fold fold, String type){
	// 	switch(type){
	// 		case "vertices":
	// 			return fold.vertices_coords.Length;
	// 		case "edges":
	// 			return Mathf.Max(fold.edges_vertices.Length, fold.edges_assignment.Length);
	// 		case "frames":
	// 			return Mathf.Max(fold.frame_classes.Length, fold.frame_attributes.Length);
	// 		case "faces":
	// 			return fold.faces_vertices.Length;
	// 		default: return 0;
	// 	}
	// }

	// int numVertices(Fold fold){
	// 	return numType(fold, "vertices");
	// }

	// int numEdges(Fold fold){
	// 	return numType(fold, "edges");
	// }

	// int numFaces(Fold fold){
	// 	return numType(fold, "faces");
	// }

	// /*
	// filter.edges_vertices_to_vertices_vertices = (fold) ->
 //  ###
 //  Works for abstract structures, so NOT SORTED.
 //  Use sort_vertices_vertices to sort in counterclockwise order.
 //  ###
 //  ## If there are no vertices_... fields, use largest vertex specified in
 //  ## edges_vertices (which must exist in this function).
 //  numVertices = filter.numVertices(fold) ?
 //    Math.max (
 //      for edge in fold.edges_vertices
 //        Math.max edge[0], edge[1]
 //    )...
 //  vertices_vertices = ([] for v in [0...numVertices])
 //  for edge in fold.edges_vertices
 //    [v, w] = edge
 //    while v >= vertices_vertices.length
 //      vertices_vertices.push []
 //    while w >= vertices_vertices.length
 //      vertices_vertices.push []
 //    vertices_vertices[v].push w
 //    vertices_vertices[w].push v
 //  vertices_vertices
	// */
	// public List<List<int>> edges_vertices_to_vertices_vertices(Fold fold){
	// 	int num_vertices = numVertices(fold);
	// 	List<List<int>> vertices_vertices = new List<List<int>>();
	// 	foreach(int[] edge in fold.edges_vertices){
	// 		if(edge[0]< num_vertices){
	// 			vertices_vertices.Add(new List<int>());
	// 		}
	// 		if(edge[1]< num_vertices){
	// 			vertices_vertices.Add(new List<int>());
	// 		}
	// 		vertices_vertices[edge[0]].Add(edge[1]);
	// 		vertices_vertices[edge[1]].Add(edge[0]);
	// 	}
	// 	return vertices_vertices;
	// }

	// bool rangesDisjoint(float a1, float a2, float b1, float b2){
	// 	float aMax = Mathf.Max(a1, a2);
	// 	float aMin = Mathf.Min(a1, a2);
	// 	return (b1< aMin && b2>aMin) || (b1>aMax && aMax<b2);
	// }

	// float twiceSignedArea(Vector2[] points){
	// 	float sum = 0;
	// 	for(int i=0; i<points.Length; i++){
	// 		Vector2 v0 = points[i];
	// 		Vector2 v1 = points[(i+1)%points.Length];
	// 		sum += (v0.x * v1.y) - (v1.x * v0.y);
	// 	}
	// 	return sum;
	// }

	// int polygonOrientation(Vector2[] points){
	// 	float tsa = twiceSignedArea(points);
	// 	if(tsa>0)
	// 		return 1;
	// 	else if(tsa<0)
	// 		return -1;
	// 	else if(tsa==0)
	// 		return 0;
	// 	else return 0;
	// }

	// bool segmentsCross(Vector2 p0, Vector2 q0, Vector2 p1, Vector2 q1){
	// 	if(rangesDisjoint(p0.x, q0.x, p1.x, q1.x) ||
	// 		 rangesDisjoint(p0.y, q0.y, p1.y, q1.y)){
	// 			 return false;
	// 	}
	// 	return(
	// 	( polygonOrientation(new Vector2[]{p0, q0, p1}) != polygonOrientation(new Vector2[]{p0, q0, q1}) ) &&
	// 	( polygonOrientation(new Vector2[]{p1, q1, p0}) != polygonOrientation(new Vector2[]{p1, q1, q0}) )
	// 	);
	// }

	// float ang2D(Vector2 a, double eps){
	// 	double sqrm = a.sqrMagnitude;
	// 	if(sqrm < eps){
	// 		return 0;
	// 	}
	// 	return Mathf.Atan2(a.y, a.x);
	// }

	//List<Vector2> sortByAngle(List<Vector2> points, Vector2 origin = new Vector2(0, 0), )
}
