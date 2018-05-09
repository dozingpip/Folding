using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;


public class Point : MonoBehaviour 
{
	public List<Point> connectedPoints;
	public Vector3 position;
	public string name;
	public Transform transform;
	// this point's index in the mesh vertices array, should be same as name without "vertex"
	public int index;
	private Model parentModel;
	private Highlight highlight;
	private bool selected = false;

	protected void Awake(){
		position = gameObject.transform.position;
		name = gameObject.name;
		transform = gameObject.transform;
		
	}

	public void Start(){
		highlight = gameObject.GetComponent<Highlight>();
		parentModel = transform.parent.gameObject.GetComponent<Model>();
	}


    public void changedPosition(){
    	parentModel.updateVectorArray(this.gameObject);
    }

    public void select(){
    	if(!selected){
	    	parentModel.pointSelected(this);
	    	highlight.highlight();
	    	selected = true;
    	}else{
    		deselect();
    		selected = false;
    	}
    }

    public void deselect(){
    	parentModel.pointDeselected(this);
    	highlight.normal();
    }

	public void newConnection(Point connectedTo){
		if(connectedPoints== null){
			Debug.Log("connected points not initialized, so initializing and adding");
			connectedPoints = new List<Point>();
		}
		connectedPoints.Add(connectedTo);
	}
}
