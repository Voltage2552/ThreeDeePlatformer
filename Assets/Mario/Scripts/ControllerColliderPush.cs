using UnityEngine;
using System.Collections;

/*
// Controller Collider Push 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Toggles pushing mode for player
// Instruction: Assign script to a gameObject where the player will 'push' an object
// Important: Currently layer 13 has the option for 'push', if you change it, just change this one
*/

public class ControllerColliderPush : MonoBehaviour 
{
	public static bool push 	 = false;								// toggle for push mode
	public static bool isPushing = false;								// toggle for pushing
	public float speedIdle 		 = 0.2f;								// idle speed, could connect it directly to controller system - speedIdleMax
	
	void OnTriggerStay ( Collider other )								// trigger events for collider on push
	{																	// trigger events for collider on push
		if ( other.tag == "push" || other.gameObject.layer == 13 && ControllerSystem.moveSpeed > speedIdle && !ControllerSystem.isCrouching )	// if collider equals pushing object tag name
		{
			push = true;													// enable push mode				
			isPushing = true;												// enable pushing
		}
	}
	void OnTriggerExit ( Collider other )									// trigger events for collider on push
	{							
		if ( other.tag == "push" || other.gameObject.layer == 13 )			// if collider equals pushing object tag name
		{
			isPushing = false;												// disable pushing
		}
	}
}
