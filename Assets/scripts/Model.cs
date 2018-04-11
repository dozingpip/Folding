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

    // point objects (the object the user interacts with)
    private List<Point> points;
    // the points in vector3 form
    private List<Vector3> vectorPoints;
    // line objects to give the mesh some definition
    private List<GameObject> edgeLines;
    private Fold fold;
    private Mesh mesh;
    // history of position changes of points
    private Stack<List<Vector3>> positionChange;

    // Use this for initialization
    void Start () {
        Debug.Log("starting model creation");
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

        vectorPoints = new List<Vector3>();
        points = new List<Point>();
        highlightAllPoints();

        positionChange = new Stack<List<Vector3>>();
        positionChange.Push(vectorPoints);

        edgeLines = new List<GameObject>();
        highlightAllEdges();
        gameObject.tag = "fold";

        points[0].createHingeJoint();
        Debug.Log("finished model creation");
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

    // change the mesh, points, and edges based on which points was moved
    public void update(GameObject whichPoint = null){
        // get the last vector list without removing it from the stack
        List<Vector3> oldVectorList = positionChange.Peek();
        List<Vector3> newVectorList = offsetAllBy(vectorPoints, -transform.position);
        fold.update_vertices_coords(newVectorList.ToArray());

        updateMeshVerts();
        highlightAllEdges();

        if(!isValidPosition(whichPoint.GetComponent<Point>(), oldVectorList)){
            vectorPoints = positionChange.Pop();
        }
    }

    bool isValidPosition(Point point, List<Vector3> previousList){
        if(vectorPoints.Contains(point.position)){
            Debug.Log("has the point in my vectorPoints at index "+ vectorPoints.IndexOf(point.position));
        }

        int[] determinantSignAfter = new int[mesh.triangles.Length/3];
        for(int i = 0; i<mesh.triangles.Length; i+=3){
            int[] plane = {mesh.triangles[i], mesh.triangles[i+1], mesh.triangles[i+2]};
            Vector3 diff01 = mesh.vertices[plane[1]] - mesh.vertices[plane[0]];
            Vector3 diff02 = mesh.vertices[plane[2]] - mesh.vertices[plane[0]];
            Vector3 diff0x = point.position - mesh.vertices[plane[0]];
            float determinant = diff01.x*diff02.y*diff0x.z +
                                diff02.x*diff0x.y*diff01.z +
                                diff0x.x*diff01.y*diff02.z -
                                diff0x.x*diff02.y*diff01.z -
                                diff02.x*diff01.y*diff0x.z -
                                diff01.x*diff0x.y*diff02.z;
            if(determinant> 0){
                determinantSignAfter[i] = 1;
            }else{
                determinantSignAfter[i] = 0;
            }
        }
        int[] determinantSignBefore = new int[mesh.triangles.Length/3];
        for(int i = 0; i<mesh.triangles.Length; i+=3){
            int[] plane = {mesh.triangles[i], mesh.triangles[i+1], mesh.triangles[i+2]};
            Vector3 diff01 = previousList[plane[1]] - previousList[plane[0]];
            Vector3 diff02 = previousList[plane[2]] - previousList[plane[0]];
            Vector3 diff0x = point.position - previousList[plane[0]];
            float determinant = diff01.x*diff02.y*diff0x.z +
                                diff02.x*diff0x.y*diff01.z +
                                diff0x.x*diff01.y*diff02.z -
                                diff0x.x*diff02.y*diff01.z -
                                diff02.x*diff01.y*diff0x.z -
                                diff01.x*diff0x.y*diff02.z;
            if(determinant> 0){
                determinantSignBefore[i] = 1;
            }else{
                determinantSignBefore[i] = 0;
            }
        }
        for(int i = 0; i<mesh.triangles.Length; i+=3){
            if(determinantSignBefore[i]!= determinantSignAfter[i]){
                Debug.Log("invalid position");
                return false;
            }else{
                Debug.Log("valid position");
                return true;
            }
        }
        return false;
    }


    void highlightPoint(int vertexIndex){
        Vector3 point = mesh.vertices[vertexIndex]+transform.position;
        GameObject pointObject = Instantiate(pointInsta, point, Quaternion.identity);
        pointObject.transform.parent = transform;
        pointObject.name = "Vertex "+ vertexIndex;
        Point pointComponent = pointObject.GetComponent<Point>();
        points.Add(pointComponent);
        pointComponent.index = vertexIndex;
        vectorPoints.Add(mesh.vertices[vertexIndex]);
    }

    void highlightAllPoints(){
        for(int i = 0; i< mesh.vertices.Length; i++){
            highlightPoint(i);
        }
    }

    void highlightEdge(int vertexIndex1, int vertexIndex2){
        Vector3 point1 = mesh.vertices[vertexIndex1]+transform.position;
        Vector3 point2 = mesh.vertices[vertexIndex2]+transform.position;
        Point p1 = points[vertexIndex1];
        Point p2 = points[vertexIndex2];
        // p1.Awake();
        // p2.Awake();
        p1.newConnection(p2);
        p2.newConnection(p1);
        string name = "Line from "+ vertexIndex1 + " to "+ vertexIndex2;
        GameObject lineObject = (transform.Find(name)) ? transform.Find(name).gameObject : null;
        if(!lineObject){
            lineObject = Instantiate(lineInsta, point1, Quaternion.identity);
            lineObject.transform.parent = transform;
            lineObject.name = name;
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

    List<Vector3> offsetAllBy(List<Vector3> vectors, Vector3 offset){
        for(int i = 0; i< vectors.Count; i++){
            vectors[i]+=offset;
        }
        return vectors;
    }
}
