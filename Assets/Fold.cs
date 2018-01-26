using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fold : MonoBehaviour {

	public class PlaneVector{
		float angle;
		Vector3 v;
		public PlaneVector(Vector3 v_){
			v = v_;
			angle = 0;
		}

		public void addToAngle(float amount){
			angle+=amount;
		}

		public void  addToVector(Vector3 addV){
			v+=addV;
		}

		public void setVector(Vector3 newV){
			v = newV;
		}

		public float X(){
			return v.x;
		}

		public float Y(){
			return v.y;
		}

		public float Z(){
			return v.z;
		}

		public float A(){
			return angle;
		}

		public Vector3 V(){
			return v;
		}
	}


	public Mesh mesh;
	float numRows;
	int numV, rowSize;
	public PlaneVector[,] vectors;
	public float speed = 1f;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
        numV = mesh.vertices.Length;
		rowSize = (int)Mathf.Sqrt(numV);
		vectors = new PlaneVector[rowSize,rowSize];
		int total = 0;
		for(int i = 0; i< vectors.GetLength(0); i++){
			for(int j = 0; j< vectors.GetLength(1); j++){
				vectors[i, j] = new PlaneVector(mesh.vertices[total]);
				total++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3[] vertices = mesh.vertices;
		if(Input.GetKeyUp("space")){
	    	vectors[0, 0].addToVector(new Vector3(0, 7, 0));
	    	vectors[0, 1].addToVector(new Vector3(0, 5, 0));
	        Debug.Log(vectors[0, 0].V());
		}

		//to fold the plane in half, for every point below (or above, pick only one side)
		// the edge along which to fold, match with a point on the other
		// side, find the midpoint between the two and rotate 180 degrees
		// around that midpoint.
		// for ex the point at row 0, col 0 corresponds with point at row 10, col 0
		Vector3 midpoint = (vectors[10, 0].V() - vectors[0,0].V()) *0.5f;
		Debug.Log(midpoint);
		// the 2 points must have the same position on one plane,
		// we must know which axis that is on. (in this case it's x)

		if(vectors[0, 0].A() < 180f){
			vectors[0, 0].addToAngle(speed);
			Debug.Log(vectors[0, 0].A());
			float cosA = Mathf.Cos(vectors[0, 0].A());
			float sinA = Mathf.Sin(vectors[0, 0].A());
			float y = vectors[0, 0].Y();
			float z = vectors[0, 0].Z();
			float newX = vectors[0, 0].X();
			// https://academo.org/demos/rotation-about-point/
			// pretend y is y, and z is like x from that site ^
			float newY = y*cosA - z*sinA;
			float newZ = z*cosA - y*sinA;
			vectors[0, 0].setVector(new Vector3(newX, newY, newZ));
		}

		// put the 2d array back into its original format
		int total = 0;
		for(int i = 0; i< vectors.GetLength(0); i++){
			for(int j = 0; j< vectors.GetLength(1); j++){
				vertices[total] = vectors[i, j].V();
				total++;
			}
		}
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
	}

	void FoldAlongEdge(int edge1Index, int edge2Index){

	}
}
