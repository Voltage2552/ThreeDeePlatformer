using UnityEngine;
using System.Collections;

/*
// Controller Collider Hurt 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Toggles hurt mode for player
// Instruction: Assign script to a gameObject that checks for 'hurt' from enemies (include collider and isTrigger)
*/

public class ControllerColliderHurt : MonoBehaviour 
{
	public static bool enemyHit	 = false;											// toggle for attack mode
	public static int damageAmount;														// hold player damage amount
	
	IEnumerator OnTriggerEnter ( Collider other ) 												// trigger events for collider on foot
	{										
		if ( other.tag == "enemy" ) 													// if collider equals enemy object
		{
			enemyHit = true;															// enable attacking 
			damageAmount = other.GetComponent < EnemyController >().damageAmount;			// take damage to player health
			yield return new WaitForSeconds ( 1.0f );													// wait a second before checking for another hit
		}
	}
	void OnTriggerExit  ( Collider other ) 												// check for collider exiting
	{										
		if ( other.tag == "enemy" )														// if tag enemey exits
		{
			enemyHit = false;															// disable enemy hit ability
		}
	}
}
