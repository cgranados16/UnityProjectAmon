using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMapCameraController : MonoBehaviour {

	public float smooth = 3f;		
	Transform standardPos;
	Transform customPos;
	bool inBuilding;
	
	void Start () {

		standardPos = GameObject.Find ("CamPos").transform;
		customPos = standardPos;
		transform.position = standardPos.position;
	}

	void FixedUpdate(){
		if (inBuilding){
			setCameraPositionCustomView();
		}else{
			setCameraPositionNormalView();
		}
	}


	
	void setCameraPositionNormalView()
	{
		transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.fixedDeltaTime * smooth);	
		transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
		inBuilding = false;
	}

	void setCustomView(Transform newPos)
	{
		inBuilding = true;
		if(customPos!= newPos){
			customPos = newPos;
			print("Es diferente");
		}
		setCameraPositionCustomView();
	}

	void setCameraPositionCustomView()
	{
		transform.position = Vector3.Lerp(transform.position, customPos.position, Time.fixedDeltaTime * smooth);	
		transform.forward = Vector3.Lerp(transform.forward, customPos.forward, Time.fixedDeltaTime * smooth);		
	}

}
