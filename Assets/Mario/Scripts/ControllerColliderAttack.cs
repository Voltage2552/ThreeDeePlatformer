using UnityEngine;
using System.Collections;

public class ControllerColliderAttack : MonoBehaviour 
{
	public static bool isAttacking = false;											// toggle for attack mode
	
	void OnTriggerEnter ( Collider other)
	{										// trigger events for collider on foot
		if ( other.tag == "enemy" ) 													// if collider equals enemy object
		{
			isAttacking = true;															// enable attacking 
			other.GetComponent <EnemyController>().ApplyDamage ();						// apply damage state to enemy
		}
	}
	void OnTriggerExit ( Collider other)
	{										// on exit of trigger
		if ( other.tag == "enemy" )														// check for tag name enemy
		{
			isAttacking = false;														// disable attacking
		}
	}
}
