using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fold))]

public class foldAtVerts : Editor {

	void OnSceneGUI(){
		Fold f = target as Fold;
		if( f == null || f.mesh == null )
            return;

        Vector3[] verts = f.mesh.vertices;
		Handles.DrawLine(f.vectors[0, 5], f.vectors[10,5]);
		Handles.Label(f.vectors[9, 6], "vert " + 9+ ", "+ 6);
		Handles.Label(f.vectors[0, 0], "vert " + 0+ ", "+ 0);
		Handles.Label(f.vectors[10, 10], "vert " + 10+ ", "+ 10);
	}
}
