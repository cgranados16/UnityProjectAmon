//
// Unityちゃん用の三人称カメラ
// 
// 2013/06/07 N.Kobyasahi
//
using UnityEngine;
using System.Collections;


public class CharacterCamera : MonoBehaviour
{
	public float smooth = 3f;
	Transform standardPos;	
	Transform jumpPos;		
	bool bQuickSwitch = false;
	
	
	void Start()
	{
		standardPos = GameObject.Find ("CamPos").transform;
		transform.position = standardPos.position;	
		transform.forward = standardPos.forward;	
	}

	
	void FixedUpdate ()
	{
		setCameraPositionNormalView();
	}

	void setCameraPositionNormalView()
	{
		if(bQuickSwitch == false){
						transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.fixedDeltaTime * smooth);	
						transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
		}
		else{
			transform.position = standardPos.position;	
			transform.forward = standardPos.forward;
			bQuickSwitch = false;
		}
	}

	void setCameraPositionJumpView()
	{
		bQuickSwitch = false;
				transform.position = Vector3.Lerp(transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);	
				transform.forward = Vector3.Lerp(transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);		
	}
}
