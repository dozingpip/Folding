using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
	List<Point> connectedPoints;
	private float SnapDistance = 1f;
	private float RungAngleInterval = 360f;
	private Vector3 LastAngularVelocity = Vector3.zero;
	private Transform InitialAttachPoint;
	public float currentAngle;
	public Vector3 position;

	public void Awake(){
		GetComponent<Rigidbody>().maxAngularVelocity = 100f;
		connectedPoints = new List<Point>();
		position = transform.position;
	}
	void createHingeJoint(){
		// see if the line composed of two points we're connected to can make a perpendicular
		// line that goes through this point.
		// to get a perpendicular line: find the slope, and then the perpendicular slope is 
		// the opposite sign and reciprocal of the original slope
		if(connectedPoints.Count == 2){
			Vector3 midpoint = Vector3.Lerp(connectedPoints[0].position, connectedPoints[1].position, 0.5f);
			HingeJoint hinge = gameObject.AddComponent<HingeJoint>();
			hinge.connectedAnchor = midpoint;
			//hinge.axis = midpoint;
		}
		/*Constrain user movement of points using a hinge joint system with 
		the connected anchor for each point being the edge perpendicular to 
		the selected point (choose this perpendicular edge and create the 
		hinge joint upon point selection).  Use the NVRInteractableRotator 
		script as a basis.*/
	}

	public void touched(){
		createHingeJoint();
	}

	public void newConnection(Point connectedTo){
		if(connectedPoints!=null)
			connectedPoints.Add(connectedTo);
		else
			Debug.Log("connected points not initialized");
	}
}
