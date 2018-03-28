using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
	public GameObject snapZonePrefab;
	List<Point> connectedPoints;
	// Use this for initialization
	void Start () {
		connectedPoints = new List<Point>();
	}

	void createSnapDropZones(){
		List<Vector3> possibleDropPoints = getPossiblePoints();
		foreach(Vector3 possibleDropPoint in possibleDropPoints){
			GameObject.Instantiate(snapZonePrefab, possibleDropPoint, transform.rotation);
		}
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
		connectedPoints.Add(connectedTo);
	}
}
