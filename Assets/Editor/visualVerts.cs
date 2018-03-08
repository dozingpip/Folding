using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(testMeshDebug))]

public class visualVerts : Editor {

    void OnSceneGUI(){
        testMeshDebug f = target as testMeshDebug;
        if( f == null || f.mesh == null )
            return;
        Debug.Log(f.mesh.triangles.Length);
        int vert0 = f.mesh.triangles[0];
        int vert1 = f.mesh.triangles[1];
        int vert2 = f.mesh.triangles[2];
        Handles.DrawLine(f.mesh.vertices[vert0], f.mesh.vertices[vert1]);
        Handles.DrawLine(f.mesh.vertices[vert1], f.mesh.vertices[vert2]);
        Handles.DrawLine(f.mesh.vertices[vert2], f.mesh.vertices[vert0]);
        Handles.Label(f.mesh.vertices[vert0], "vert " +vert0);
        Handles.Label(f.mesh.vertices[vert1], "vert " +vert1);
        Handles.Label(f.mesh.vertices[vert2], "vert " +vert2);
    }
}
