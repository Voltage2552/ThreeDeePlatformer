using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewCameraStructure : MonoBehaviour {

	#region Data

	public enum CameraStyle										//Holds all of the different camera Types
	{
		Standard,
		other
	};
	CameraStyle camStyle;										//The holder variable for the current selected Camera
	public static NewCameraStructure instance;
	public GameObject thePlayer;								//Holds the player gameObject
	public GameObject followNode;								//in case we want to use a follow node instead of the player
	public Transform playerTransform;
	GameObject theCamera;										//holds the camera gameObject
	#endregion

	#region Unity Methods
	// Use this for initialization
	void Awake()						
	{
		instance = this;								//Create a singleton for the script
		theCamera = this.gameObject;							//gets and stores the camera gameObject
	}
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(camStyle == CameraStyle.Standard)					//check if the camera is in the standard state
		{
			theCamera.transform.position = new Vector3(thePlayer.transform.position.x,thePlayer.transform.position.y + 2, thePlayer.transform.position.z - 5.5f);
			this.transform.LookAt(playerTransform);
			//theCamera.transform.eulerAngles = new Vector3 (0, thePlayer.transform.rotation.y, 0);
		}
		else if (camStyle == CameraStyle.other)					//check if the camera is in the other state
		{
			//Do the other camera Style
		}
	}
	#endregion

	#region Public Methods
	public void SwitchCamera(CameraStyle newCamera)				//call this and pass in a new camera type to change the camer(coonected to a trigger object)
	{
		switch(newCamera)										//check the new camera variable
		{
		case CameraStyle.Standard:								//if it is standard....
			camStyle = CameraStyle.other;
			Debug.LogError ("Moving from standard to other");	//switch it to the other camera
			break;
		case CameraStyle.other:									//if it is other....
			camStyle = CameraStyle.Standard;
			Debug.LogError ("Moving from other to Standard");	//switch back to standard
			break;
		}
	}
	#endregion
}
