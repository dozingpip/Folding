using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMeshDebug : MonoBehaviour {
    public Mesh mesh;
    // Use this for initialization
    void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        //Debug.Log("verts "+mesh.vertices);
        //Debug.Log("tris "+mesh.triangles);
        foreach(int i in mesh.triangles){
            //Debug.Log("tri vert "+ i);
            Debug.Log("vertex "+ i+ " coords: "+ mesh.vertices[i]);
        }
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
