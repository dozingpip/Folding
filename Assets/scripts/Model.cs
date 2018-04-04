using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Model : MonoBehaviour {
    public string file_name = "flappingBird";
    public string interpolate_to_file_name = "miura-ori";
    public float interpolateSpeed = 0.1f;
    public Material material;
    public GameObject pointInsta;
    public GameObject lineInsta;
    private List<string> foldFiles;
    public string foldFilePath ="folds";
    public Dropdown dropdown;

    private List<GameObject> points;
    private List<GameObject> edgeLines;
    private Fold fold;
    private Mesh mesh;

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
        string path = Application.dataPath + "/Resources/"+foldFilePath;
        string[] filePaths = Directory.GetFiles(@path, "*.fold");
        List<string> dropOptions = new List<string>();
        foreach(string fileName in filePaths){
            dropOptions.Add(fileName);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(dropOptions);

        fold = new Fold(file_name);
        

        createMesh();
        
        points = new List<GameObject>();
        highlightAllPoints();

        edgeLines = new List<GameObject>();
        highlightAllEdges();
        gameObject.tag = "fold";
    }

    void Update(){
        update();
    }

    void createMesh(){
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        gameObject.AddComponent<MeshRenderer>();
        mesh.Clear();

        updateMeshVerts();

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

        Vector2[] uv = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(mesh.vertices[i].x+0.5f, mesh.vertices[i].z+0.5f);
        }
        mesh.uv = uv;
        // better way is to use the flat version of the model as a base and get
        // the corners and base the uv coordinates on distance from the 0, 0 corner

        GetComponent<MeshRenderer>().material = material;
        mesh.RecalculateBounds();
    }

    void updateMeshVerts(){
        mesh.vertices = fold.vertices_coords_toArray();
        mesh.RecalculateBounds();
    }

    public void update(){
        Vector3[] newVectorList = offsetAllBy(gameobjectsToVectorPositionList(points), -transform.position);
        fold.update_vertices_coords(newVectorList);

        updateMeshVerts();
        highlightAllEdges();
    }   

    void highlightPoint(int vertexIndex){
        Vector3 point = mesh.vertices[vertexIndex]+transform.position;
        GameObject pointObject = Instantiate(pointInsta, point, Quaternion.identity);
        pointObject.transform.parent = transform;
        pointObject.name = "Vertex "+ vertexIndex;
        points.Add(pointObject);
    }

    void highlightAllPoints(){
        for(int i = 0; i< mesh.vertices.Length; i++){
            highlightPoint(i);
        }
    }

    void highlightEdge(int vertexIndex1, int vertexIndex2){
        Vector3 point1 = mesh.vertices[vertexIndex1]+transform.position;
        Vector3 point2 = mesh.vertices[vertexIndex2]+transform.position;
        Point p1 = points[vertexIndex1].gameObject.GetComponent<Point>();
        Point p2 = points[vertexIndex2].gameObject.GetComponent<Point>();
        
        p1.newConnection(p2);
        p2.newConnection(p1);
        string name = "Line from "+ vertexIndex1 + " to "+ vertexIndex2;
        GameObject lineObject = (transform.Find(name)) ? transform.Find(name).gameObject : null;
        if(!lineObject){
            lineObject = Instantiate(lineInsta, point1, Quaternion.identity);
            
            lineObject.transform.parent = transform;
            lineObject.name = name;
            // FixedJoint joint = points[vertexIndex1].GetComponent<FixedJoint>();
            // if(joint){
            //     joint = points[vertexIndex2].GetComponent<FixedJoint>();
            //     if(joint){
            //         Debug.Log("both vertices already have joints, halp");
            //     }else{
            //         joint = points[vertexIndex2].AddComponent<FixedJoint>();
            //     }
            //     joint.connectedBody = points[vertexIndex1].GetComponent<Rigidbody>();
            // }else{
            //     joint = points[vertexIndex1].AddComponent<FixedJoint>();
            //     joint.connectedBody = points[vertexIndex2].GetComponent<Rigidbody>();
            // }
            edgeLines.Add(lineObject);
        }
        LineRenderer line = lineObject.GetComponent<LineRenderer>();
        line.SetPosition(0, point1);
        line.SetPosition(1, point2);
    }

    void highlightAllEdges(){
        for(int i = 0; i< fold.edges_vertices.Length; i++){
            highlightEdge(fold.edges_vertices[i][0], fold.edges_vertices[i][1]);
        }
    }

    Vector3[] gameobjectsToVectorPositionList(List<GameObject> objects){
        Vector3[] vectorList = new Vector3[objects.Count];
        for(int i = 0; i< vectorList.Length; i++){
            vectorList[i] = objects[i].transform.position;
        }
        return vectorList;
    }

    Vector3[] offsetAllBy(Vector3[] list, Vector3 offset){
        for(int i = 0; i< list.Length; i++){
            list[i]+=offset;
        }
        return list;
    }
}
