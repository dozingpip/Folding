using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {
	public float thickness = 0.01f;
	private Model parentModel;
	private Highlight highlight;
	private bool selected = false;
	public Point pointA;
	public Point pointB;

	public void Start(){
		highlight = gameObject.GetComponent<Highlight>();
		parentModel = transform.parent.gameObject.GetComponent<Model>();
	}

	public void SetPosition(Point _pointA, Point _pointB){
		pointA = _pointA;
		pointB = _pointB;
		Vector3 vectorA = _pointA.position;
		Vector3 vectorB = _pointB.position;
		Vector3 between = vectorB - vectorA;
		float distance = between.magnitude;
		Vector3 scale = new Vector3(thickness, thickness, distance);
		transform.localScale = scale;

		transform.position = vectorA + (between *0.5f);
		transform.LookAt(vectorB);
	}

	public void select(){
    	if(!selected){
	    	parentModel.lineSelected(this);
	    	highlight.highlight();
	    	selected = true;
    	}else{
    		deselect();
    		selected = false;
    	}
    }

    public void deselect(){
    	parentModel.lineDeselected(this);
    }
}
