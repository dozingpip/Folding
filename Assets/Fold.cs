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
	}


	public Mesh mesh;
	float numRows;
	int numV, rowSize;
	public PlaneVector[,] vectors;
	public float speed = 5;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
        numV = mesh.vertices.Length;
		rowSize = (int)Mathf.Sqrt(numV);
		vectors = new PlaneVector[rowSize,rowSize];
		int total = 0;
		for(int i = 0; i< vectors.GetLength(0); i++){
			for(int j = 0; j< vectors.GetLength(1); j++){
				vectors[i, j] = mesh.vertices[total];
				total++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3[] vertices = mesh.vertices;
		if(Input.GetKeyUp("space")){
	    	vectors[0, 0] += new Vector3(0, 7, 0);
	    	vectors[0, 1] += new Vector3(0, 5, 0);
	        Debug.Log(vectors[0, 0]);
		}

		//to fold the plane in half, for every point below (or above, pick only one side)
		// the edge along which to fold, match with a point on the other
		// side, find the midpoint between the two and rotate 180 degrees
		// around that midpoint.
		// for ex the point at row 0, col 0 corresponds with point at row 10, col 0
		Vector3 midpoint = (vectors[10, 0] - vectors[0,0]) *0.5f;
		Debug.Log(midpoint);
		// the 2 points must have the same position on one plane,
		// we must know which axis that is on. (in this case it's x)

		if(vectors[0, 0] != vectors[10, 0]){
			float newX = vectors[0, 0].X()
			float newY = vectors[0, 0].Y()*Mathf.cos(vectors[0, 0].addToAngle(speed))

			vectors[0, 0].setVector(new Vector3());
		}

		// put the 2d array back into its original format
		int total = 0;
		for(int i = 0; i< vectors.GetLength(0); i++){
			for(int j = 0; j< vectors.GetLength(1); j++){
				vertices[total] = vectors[i, j];
				total++;
			}
		}
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
	}

	void FoldAlongEdge(int edge1Index, int edge2Index){

	}
}
