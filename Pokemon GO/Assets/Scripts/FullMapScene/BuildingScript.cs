using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour {

	private GameObject cameraObject;


	void Start (){
		cameraObject = GameObject.FindWithTag("MainCamera");
	}

	void FixedUpdate(){
		if (Input.GetMouseButtonDown(0)){ // if left button pressed...
	     Ray ray = cameraObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
	     RaycastHit hit;
	     if (Physics.Raycast(ray, out hit)){
	       cameraObject.SendMessage("setCustomView",hit.transform.GetChild(1).transform);
	     }
	   }
    }
}
