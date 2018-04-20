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

    private GameObject mostRecentlyChangedPoint = null;

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

        //points[0].createHingeJoint();
        Debug.Log("finished model creation");
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

    public void Update(){}

    // update vector points array based on which points was moved
    public void updateVectorArray(GameObject whichPoint){
        mostRecentlyChangedPoint = whichPoint;
    }

    // goes before PreRender
    public void OnWillRenderObject(){
        if(mostRecentlyChangedPoint != null){
            List<Vector3> oldVectorList = positionChange.Peek();
            Point p = mostRecentlyChangedPoint.GetComponent<Point>();
            List<Vector3> newVectors = vectorPoints;
            newVectors[p.index] = p.position;
            positionChange.Push(newVectors);
            if(isValidPosition(p, oldVectorList)){
                positionChange.Push(newVectors);
            }
            mostRecentlyChangedPoint = null;
        }
    }

    public void onPreRender(){
        List<Vector3> newVectorList = offsetAllBy(positionChange.Peek(), -transform.position);
        fold.update_vertices_coords(newVectorList.ToArray());

        updateMeshVerts();
        highlightAllEdges();
    }

    bool isValidPosition(Point point, List<Vector3> previousList){
        // if(vectorPoints.Contains(point.position)){
        //     Debug.Log("has the point in my vectorPoints at index "+ vectorPoints.IndexOf(point.position));
        // }
        
        // List<Vector3> currentList = new List<Vector3>();
        // currentList.AddRange(mesh.vertices);
        // if(!compareDeterminantSets(point, previousList, currentList)){
        //     Debug.Log("some determinants have changed");
        //     return false;
        // }

        // for(int i = 0; i<point.connectedPoints.Count; i++){
        //     Point connectedPoint = point.connectedPoints[i];
        //     // the current distance to the connected point (after the change)
        //     float currentDistance = Vector3.Distance(connectedPoint.position, point.position);
        //     Vector3 pointBeforeChange = previousList[connectedPoint.index];
        //     float distanceBeforeChange = Vector3.Distance(point.position, pointBeforeChange);
        //     if(currentDistance!=distanceBeforeChange){
        //         return false;
        //     }
        // }

        return true;
    }

    bool compareDeterminantSets(Point point, List<Vector3> list1, List<Vector3> list2){
        int[] determinants1 = getDeterminants(point, list1);
        int[] determinants2 = getDeterminants(point, list2);
        for(int i = 0; i<mesh.triangles.Length; i+=3){
            if(determinants1[i]!= determinants2[i]){
                return false;
            }
        }
        return true;
    }

    int[] getDeterminants(Point point, List<Vector3> list){
        // get the determinants of plane to point before the point has been moved
        int[] determinantSign = new int[mesh.triangles.Length/3];
        for(int i = 0; i<mesh.triangles.Length; i+=3){
            // to do: exclude planes the point is connected to
            int[] plane = {mesh.triangles[i], mesh.triangles[i+1], mesh.triangles[i+2]};
            Vector3 diff1 = list[plane[1]] - list[plane[0]];
            Vector3 diff2 = list[plane[2]] - list[plane[0]];
            Vector3 diff3 = point.position - list[plane[0]];
            
            float determinant = diff1.x*diff2.y*diff3.z +
                                diff2.x*diff3.y*diff1.z +
                                diff3.x*diff1.y*diff2.z -
                                diff3.x*diff2.y*diff1.z -
                                diff2.x*diff1.y*diff3.z -
                                diff1.x*diff3.y*diff2.z;
            if(determinant> 0){
                determinantSign[i] = 1;
            }else{
                determinantSign[i] = 0;
            }
        }

        return determinantSign;
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
