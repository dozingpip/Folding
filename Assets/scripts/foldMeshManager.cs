using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foldMeshManager : MonoBehaviour {
	List<Model> models;
	// Use this for initialization
	void Start () {
		models = new List<Model>();
		GameObject[] folds = GameObject.FindGameObjectsWithTag("fold");
		foreach(GameObject model in folds){
			models.Add(model.GetComponent<Model>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Model model in models){
			//model.update();
		}
	}

	// void interpolate(float dt, Model model1, Model model2){
 //        Debug.Log(model1.vertices_coords.Length);
 //        Debug.Log(model2.vertices_coords.Length);
 //        if(points.Count == fold2.vertices_coords.Length){
 //            for(int i = 0; i < fold.vertices_coords.Length; i++){
 //                if(fold.vertices_coords[i].x< fold2.vertices_coords[i].x){
 //                    fold.vertices_coords[i].x+= interpolateSpeed*dt;
 //                }
 //            }

 //        }
 //    }
}
