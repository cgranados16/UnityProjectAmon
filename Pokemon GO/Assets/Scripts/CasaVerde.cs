using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasaVerde : MonoBehaviour {
 	 private GUIStyle guiStyle = new GUIStyle();
	 string message = "";
	 bool onRange = false;

     void OnTriggerEnter(Collider other)
     {
     	if(other.tag == "Player")
     	{
	     	message = "Press E for something";
	     	onRange = true;
		}
        
     }
     void OnTriggerExit(Collider other)
     {
     	onRange = false;
        message = "";

     }
     void OnGUI()
     {
     	guiStyle.fontSize = 50;
     	int x = (Screen.width / 3);
		int y = (Screen.height / 4)*3;
         GUI.Label(new Rect(x, y, 200, 20), message,guiStyle);
     }

     void Update(){
     	if (onRange) {
	     	if (Input.GetKeyDown(KeyCode.E)) {
				print ("Caca");
			}
		}
     }

}
