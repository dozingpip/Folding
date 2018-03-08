using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Model : MonoBehaviour {
    public string file_name = "flappingBird";
    public string interpolate_to_file_name = "miura-ori";
    public float interpolateSpeed = 0.1f;
    private Fold fold;
    private Mesh mesh;
    private Fold fold2;
    Material material;
    GameObject pointInsta;
    GameObject lineInsta;
    public List<string> foldFiles;

    // Use this for initialization
    void Start () {
        // DirectoryInfo levelDirectoryPath = new DirectoryInfo(Application.dataPath);
        // FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);
                 
        // foreach (FileInfo file in fileInfo) {
        //     // file extension check
        //     if (file.Extension == ".fold") {
        //          foldFiles.Add(file.Name);
        //     }
        //     // etc.
        // }
        // file_name = //currently selected option from foldFiles
        material = Resources.Load("rando") as Material;
        pointInsta = Resources.Load("Point") as GameObject;
        lineInsta = Resources.Load("Line") as GameObject;

        fold = new Fold(file_name);
        fold2 = new Fold(interpolate_to_file_name);
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        mesh = newMesh(fold, GetComponent<MeshFilter>().mesh);
        GetComponent<MeshRenderer>().material = material;
        for(int i = 0; i< mesh.vertices.Length; i++){
            highlightPoint(i);
        }
        highlightEdge(1, 2);
    }

    Mesh newMesh(Fold fold, Mesh mesh){
        mesh.Clear();
        List<Vector3> verts = new List<Vector3>();
        for (int i = 0; i < fold.vertices_coords.Length; i++)
        {
            verts.Add(new Vector3((float)fold.vertices_coords[i][0],
                                   (float)fold.vertices_coords[i][1],
                                   (float)fold.vertices_coords[i][2]));
        }

        mesh.vertices = verts.ToArray();

        List<int> triangles = new List<int>();

        for(int i = 0; i< fold.faces_vertices.Length; i++){
            if(fold.faces_vertices[i].Length==3){
                triangles.Add(fold.faces_vertices[i][2]);
                triangles.Add(fold.faces_vertices[i][1]);
                triangles.Add(fold.faces_vertices[i][0]);

                triangles.Add(fold.faces_vertices[i][0]);
                triangles.Add(fold.faces_vertices[i][1]);
                triangles.Add(fold.faces_vertices[i][2]);
            }else if(fold.faces_vertices[i].Length==4){
                //two triangles for if the face has 4 vertices
                triangles.Add(fold.faces_vertices[i][2]);
                triangles.Add(fold.faces_vertices[i][3]);
                triangles.Add(fold.faces_vertices[i][1]);

                triangles.Add(fold.faces_vertices[i][1]);
                triangles.Add(fold.faces_vertices[i][3]);
                triangles.Add(fold.faces_vertices[i][2]);

                //triangle 2
                triangles.Add(fold.faces_vertices[i][3]);
                triangles.Add(fold.faces_vertices[i][0]);
                triangles.Add(fold.faces_vertices[i][1]);

                triangles.Add(fold.faces_vertices[i][1]);
                triangles.Add(fold.faces_vertices[i][0]);
                triangles.Add(fold.faces_vertices[i][3]);
            }
        }
        mesh.triangles = triangles.ToArray();

        Vector2[] uv = new Vector2[verts.Count];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(verts[i].x+0.5f, verts[i].z+0.5f);
        }
        mesh.uv = uv;
        // better way is to use the flat version of the model as a base and get
        // the corners and base the uv coordinates on distance from the 0, 0 corner

        return mesh;
    }

    void Update(){
        //interpolate(Time.deltaTime);
    }

    void interpolate(float dt){
        Debug.Log(fold.vertices_coords.Length);
        Debug.Log(fold2.vertices_coords.Length);
        if(fold.vertices_coords.Length == fold2.vertices_coords.Length){
            for(int i = 0; i < fold.vertices_coords.Length; i++){
                if(fold.vertices_coords[i].x< fold2.vertices_coords[i].x){
                    fold.vertices_coords[i].x+= interpolateSpeed*dt;
                }
            }

        }
    }

    void highlightPoint(int vertexIndex){
        Vector3 point = mesh.vertices[vertexIndex]+transform.position;
        GameObject pointObject = Instantiate(pointInsta, point, Quaternion.identity);
        pointObject.transform.parent = transform;
        pointObject.name = "Vertex "+ vertexIndex;
    }

    void highlightEdge(int vertexIndex1, int vertexIndex2){
        Vector3 point1 = mesh.vertices[vertexIndex1]+transform.position;
        Vector3 point2 = mesh.vertices[vertexIndex2]+transform.position;
        GameObject lineObject = Instantiate(lineInsta, point1, Quaternion.identity);
        LineRenderer line = lineObject.GetComponent<LineRenderer>();
        line.SetPosition(0, point1);
        line.SetPosition(1, point2);
        lineObject.transform.parent = transform;
        lineObject.name = "Line from "+ vertexIndex1 + " to "+ vertexIndex2;
    }

}
