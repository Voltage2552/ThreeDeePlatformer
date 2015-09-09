using UnityEngine;
using System.Collections;

/*
// Plant Animation 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Animates a plant on trigger event
// Instruction: Assign script to a plant (or object that has 'wiggle' as an animation label)
*/

public class PlantAnimation : MonoBehaviour 
{

	void Start ()	 									// initialize things
	{	
		GetComponent<Animation>().Stop ();								// be sure it ani starts off stopped
	}
	void OnTriggerEnter ( Collider col)					// if player runs through trigger area
	{
		GetComponent<Animation>().Play ( "wiggle" );					// play the animation file
	}
}
