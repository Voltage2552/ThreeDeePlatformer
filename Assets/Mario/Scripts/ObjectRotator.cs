using UnityEngine;
using System.Collections;

/*
// Object Rotator 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Rotates a gameObject around the y axis (could provide inspector options for axis selection and speed)
// Instruction: Assign script to any gameObject you want to spin (coins, keys, etc)
*/

public class ObjectRotator : MonoBehaviour {

	void Update () 										// updates
	{
		transform.Rotate ( 0.0f, 45.0f * Time.deltaTime, 0.0f );		// rotate object on the Y
	}
}
