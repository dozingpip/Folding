using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;


public class Point : MonoBehaviour 
{
	public List<Point> connectedPoints;
	private float SnapDistance = 1f;
	private float RungAngleInterval = 360f;
	private Vector3 LastAngularVelocity = Vector3.zero;
	public Vector3 position;
	public string name;
	public Transform transform;
	// this point's index in the mesh vertices array, should be same as name without "vertex"
	public int index;
	HingeJoint hinge;
	public Model parentModel;

	protected void Awake(){
		//base.Awake();
		position = gameObject.transform.position;
		name = gameObject.name;
		transform = gameObject.transform;
		
	}

	public void Start(){
		parentModel = transform.parent.gameObject.GetComponent<Model>();
	}

	// protected void FixedUpdate()
 //        {
 //            //base.FixedUpdate();
 //            if (IsAttached == false)
 //            {
 //                float wheelAngle = this.transform.localEulerAngles.z;

 //                float rung = Mathf.RoundToInt(wheelAngle / RungAngleInterval);

 //                float distanceToRung = wheelAngle - (rung * RungAngleInterval);
 //                float distanceToRungAbs = Mathf.Abs(distanceToRung);

 //                float velocity = Mathf.Abs(this.Rigidbody.angularVelocity.z);

 //                if (velocity > 0.001f && velocity < 0.5f)
 //                {
 //                    if (distanceToRungAbs > SnapDistance)
 //                    {
 //                        this.Rigidbody.angularVelocity = LastAngularVelocity;
 //                    }
 //                    else
 //                    {
 //                        this.Rigidbody.velocity = Vector3.zero;
 //                        this.Rigidbody.angularVelocity = Vector3.zero;

 //                        Vector3 newRotation = this.transform.localEulerAngles;
 //                        newRotation.z = rung * RungAngleInterval;
 //                        this.transform.localEulerAngles = newRotation;

 //                        this.Rigidbody.isKinematic = true;
 //                    }
 //                }
 //            }

 //            LastAngularVelocity = this.Rigidbody.angularVelocity;
 //        }

	// public override void BeginInteraction(NVRHand hand){
	// 	base.BeginInteraction(hand);
	// 	createHingeJoint();
	// }

	// public override void EndInteraction(NVRHand hand)
 //        {
 //            base.EndInteraction(hand);

 //            if(hinge != null)
 //            	Destroy(hinge);
 //        }


    public void changedPosition(){
    	parentModel.updateVectorArray(this.gameObject);
    }

	// public void createHingeJoint(){
	// 	// get the line perpendicular to the cross product of the edge b/w 
	// 	// two points this point is connected to
	// 	Debug.Log("creating hinge joint, and I have "+ connectedPoints.Count+" total connections.");
	// 	Debug.Log("connected to " + connectedPoints[0].name + " and " + connectedPoints[1].gameObject.name);
	// 	Debug.Log("point 1 at "+ connectedPoints[0].transform.localToWorldMatrix);
	// 	HingeJoint hinge = gameObject.AddComponent<HingeJoint>();
	// 	hinge.autoConfigureConnectedAnchor = false;
	// 	hinge.connectedAnchor = connectedPoints[0].position;
	// 	hinge.anchor = Vector3.zero;
	// 	hinge.axis = Vector3.MoveTowards(connectedPoints[0].position, connectedPoints[1].position, 0.5f);
	// 	/*Constrain user movement of points using a hinge joint system with 
	// 	the connected anchor for each point being the edge perpendicular to 
	// 	the selected point (choose this perpendicular edge and create the 
	// 	hinge joint upon point selection).  Use the NVRInteractableRotator 
	// 	script as a basis.*/
	// }

	public void newConnection(Point connectedTo){
		if(connectedPoints== null){
			Debug.Log("connected points not initialized, so initializing and adding");
			connectedPoints = new List<Point>();
		}
		connectedPoints.Add(connectedTo);
	}
}
