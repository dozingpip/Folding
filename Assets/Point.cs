using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
	List<Point> connectedPoints;
	private float SnapDistance = 1f;
	private float RungAngleInterval = 360f`;
	private Vector3 LastAngularVelocity = Vector3.zero;
	private Transform InitialAttachPoint;
	public float currentAngle;

	void Awake(){
		GetComponent<Rigidbody>().maxAngularVelocity = 100f;
	}

	// Use this for initialization
	void Start () {
		connectedPoints = new List<Point>();
	}

	List<Vector3> getPossiblePoints(){
		List<Vector3> possiblePoints = new List<Vector3>();
		possiblePoints.Add(transform.position);
		if(connectedPoints.Count > 1){
			// see if the line composed of two points we're connected to can make a perpendicular
			// line that goes through this point.
			// to get a perpendicular line: find the slope, and then the perpendicular slope is 
			// the opposite sign and reciprocal of the original slope
			
		}
		//TO-DO: calculate other possible points
		return possiblePoints;
	}

	public void newConnection(Point connectedTo){
		if(connectedPoints!=null)
			connectedPoints.Add(connectedTo);
		else
			Debug.Log("connected points not initialized");
	}
}
