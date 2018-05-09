using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {
	public Material highlightMaterial;
	private Material originalMaterial;
	private MeshRenderer renderer;

	void Start(){
		renderer = GetComponent<MeshRenderer>();
		originalMaterial = renderer.material;
	}

	public void highlight(){
		renderer.material = highlightMaterial;
	}

	public void normal(){
		renderer.material = originalMaterial;
	}
}
