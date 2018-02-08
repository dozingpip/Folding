using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour {
	private Fold fold;

	private string foldDataFileName = "flappingBird _ 70PercentFolded.fold";
	// Use this for initialization
	public void Start () {
		DontDestroyOnLoad(gameObject);
		LoadFold();
	}
	
	public void LoadFold(){
		string filePath = Path.Combine(Application.streamingAssetsPath, foldDataFileName);
		if(File.Exists(filePath)){
			string dataAsJson = File.ReadAllText(filePath);
			fold = JsonUtility.FromJson<Fold>(dataAsJson);
			Debug.Log(fold.vertices_coords);
		}
	}
}
