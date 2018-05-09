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
    public GameObject pointPrefab;
    public GameObject linePrefab;
    public GameObject attachJointPrefab;
    private List<string> foldFiles;
    public string foldFilePath ="folds";
    public Dropdown dropdown;
    public LineRenderer foldLineRenderer;

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
    private Queue<Point> pointQueue;
    private Queue<Line> lineQueue;

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
        // string path = Application.dataPath + "/Resources/"+foldFilePath;
        // string[] filePaths = Directory.GetFiles(@path, "*.fold");
        // List<string> dropOptions = new List<string>();
        // foreach(string fileName in filePaths){
        //     dropOptions.Add(fileName);
        // }
        // dropdown.ClearOptions();
        // dropdown.AddOptions(dropOptions);

        fold = new Fold(file_name);
        pointQueue = new Queue<Point>();
        lineQueue = new Queue<Line>();
        
        createMesh();

        vectorPoints = new List<Vector3>();
        points = new List<Point>();
        highlightAllPoints();

        positionChange = new Stack<List<Vector3>>();
        positionChange.Push(vectorPoints);

        edgeLines = new List<GameObject>();
        highlightAllEdges();
        gameObject.tag = "fold";

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

    public void pointSelected(Point p){
        if(pointQueue.Count>1){
            pointQueue.Dequeue();
            pointQueue.Enqueue(p);
        }else{
            pointQueue.Enqueue(p);
        }
    }

    public void pointDeselected(Point p){
        if(pointQueue.Contains(p)){
            Point peek = pointQueue.Peek();
            if(peek == p){
                pointQueue.Dequeue();
            }else{
                pointQueue.Dequeue();
                pointQueue.Dequeue();
                pointQueue.Enqueue(peek);
            }
        }
    }

    public void lineSelected(Line l){
        if(lineQueue.Count>1){
            lineQueue.Dequeue();
            lineQueue.Enqueue(l);
        }else{
            lineQueue.Enqueue(l);
        }
    }

    public void lineDeselected(Line l){
        if(lineQueue.Contains(l)){
            Line peek = lineQueue.Peek();
            if(peek == l){
                lineQueue.Dequeue();
            }else{
                lineQueue.Dequeue();
                lineQueue.Dequeue();
                lineQueue.Enqueue(peek);
            }
        }
    }

    public void foldIt(){
        // using Huzita's axioms for determining geometric
        // constructibility
        if(pointQueue.Count == 2 && lineQueue.Count == 0){
            // Huzita's 1st axiom
            foldLineRenderer.SetPosition(0, pointQueue.Dequeue().position);
            foldLineRenderer.SetPosition(1, pointQueue.Dequeue().position);
            // rotator axis: the fold line
            // allowed to rotate: just the vertex of the plane 
            // that wasn't included in fold line, aka the only point
            // both points are connected to.
        }else if(pointQueue.Count == 0 && lineQueue.Count == 2){
            //Huzita's 3rd axiom
            Line lineA = lineQueue.Dequeue();
            Line lineB = lineQueue.Dequeue();
            float distanceA = Vector3.Distance(lineA.pointA.position, lineB.pointA.position);
            float distanceB = Vector3.Distance(lineA.pointA.position, lineB.pointB.position);
            Vector3 secondPoint = distanceA <= distanceB ? lineB.pointA.position : lineB.pointB.position;
            Vector3 perpLine = Vector3.Cross(lineA.pointA.position, secondPoint);
            foldLineRenderer.SetPosition(0, perpLine.normalized);
            foldLineRenderer.SetPosition(1, perpLine);
            // rotator axis: fold line
            // allowed to rotate: lineA        
        }else if(pointQueue.Count == 1 && lineQueue.Count == 1){
            //Huzita's 4th axiom
            Line line = lineQueue.Dequeue();
            Point point = pointQueue.Dequeue();
            Vector3 cross = Vector3.Cross(line.pointA.position, line.pointB.position);
            foldLineRenderer.SetPosition(0, cross);
            foldLineRenderer.SetPosition(1, point.position);
            // rotator axis: fold line
            // allowed to rotate: line.pointA
            HingeJoint hinge = line.pointA.gameObject.AddComponent<HingeJoint>();
            hinge.axis = cross;
            JointLimits limits = hinge.limits;
            limits.min = 0;
            limits.bounciness = 0;
            limits.bounceMinVelocity = 0.2f;
            limits.max = 350;
            hinge.limits = limits;
            hinge.useLimits = true;

        }else if(pointQueue.Count == 2 && lineQueue.Count == 1){
            //Huzita's 5th axiom
            Point pointA = pointQueue.Dequeue();
            Point pointB = pointQueue.Dequeue();
            Line line = lineQueue.Dequeue();
            Vector3 cross = Vector3.Cross(line.pointA.position, line.pointB.position);
            foldLineRenderer.SetPosition(0, cross);
            foldLineRenderer.SetPosition(1, pointB.position);
            // rotator axis: fold line
            // allowed to rotate: pointA

        }
        

    }

    // goes before PreRender
    public void OnWillRenderObject(){
        if(mostRecentlyChangedPoint != null){
            List<Vector3> oldVectorList = positionChange.Peek();
            Point p = mostRecentlyChangedPoint.GetComponent<Point>();
            List<Vector3> newVectors = vectorPoints;
            newVectors[p.index] = p.position;
            positionChange.Push(newVectors);
            
            mostRecentlyChangedPoint = null;
        }
    }

    public void onPreRender(){
        List<Vector3> newVectorList = offsetAllBy(positionChange.Peek(), -transform.position);
        fold.update_vertices_coords(newVectorList.ToArray());

        updateMeshVerts();
        highlightAllEdges();
    }

    void highlightPoint(int vertexIndex){
        Vector3 point = mesh.vertices[vertexIndex]+transform.position;
        GameObject pointObject = Instantiate(pointPrefab, point, Quaternion.identity);
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

        p1.newConnection(p2);
        p2.newConnection(p1);
        string name = "Line from "+ vertexIndex1 + " to "+ vertexIndex2;
        GameObject lineObject = (transform.Find(name)) ? transform.Find(name).gameObject : null;
        if(!lineObject){
            lineObject = Instantiate(linePrefab, point1, Quaternion.identity);
            lineObject.transform.parent = transform;
            lineObject.name = name;
            edgeLines.Add(lineObject);
        }
        Line line = lineObject.GetComponent<Line>();
        line.SetPosition(p1, p2);
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
