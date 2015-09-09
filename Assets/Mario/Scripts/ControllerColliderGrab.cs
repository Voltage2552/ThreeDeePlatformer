using UnityEngine;
using System.Collections;

/*
// Controller Collider Grab 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Allows player to grab/pickup objects and throw them
// Instruction: Assign script to a gameObject called colliderGrab (include collider, isTrigger and animations to use for pickup and throw)
*/

public class ControllerColliderGrab : MonoBehaviour 
{
	Transform otherObject; 													// object we grab transform
	bool isPickingUp 			= false;									// toggle for picking object up
	static bool grabs 			= false;									// toggle for grab mode
	static bool pickedUp 		= false;									// toggle for object picked up
	public static bool isGrabbing 		= false;									// toggle for grabbing
	
	void Update 		() 													// loop through events
	{													
		Grab ();
	}
	IEnumerator Grab 			() 													// grab system
	{													
		if ( isPickingUp && Input.GetButtonDown ( "Fire1" )  )						// grab object
		{	
			isPickingUp = false;													// disable picking up
			Destroy ( otherObject.gameObject.GetComponent( "Rigidbody" ) );			// get rid of the rigidbody to player can pickup object with no issues
			otherObject.transform.parent = transform;								// set other objects parent to players object (colliderGrab)
			GetComponent<Animation>().Play ( "grab_pickup" );										// play the pickup animation
			otherObject.position = transform.position;								// align other object with player center position (based on the colliderGrab ga)
			isGrabbing = true;														// if all worked out, enable grabbing
			yield return null;																	// break out for one frame so that the next if check doesn't happen till then
		}
		if ( isGrabbing && Input.GetButtonDown ( "Fire1" ) )						// throw object
		{	
			Vector3 forward = this.transform.forward * ( ControllerSystem.moveSpeed + .5f );	 // forward stores player forward direction with speed
			Vector3 up = new Vector3 ( 0.0f, 2.0f, 0.0f );									// up holds vec3 up direction 
			Vector3 direction = forward + up;									// direction stores value for throwing object
			otherObject.parent = null;												// break parent connection
			otherObject.gameObject.AddComponent < Rigidbody >();						// add the rigidbody back to the object
			Destroy(otherObject.gameObject.GetComponent ( "BoxCollider" ) );		// get rid of the boxcollider - it hits him while throwing so if we destroy it, there's no issue
			otherObject.gameObject.GetComponent<Rigidbody>().AddForce ( ( direction ) * 150 );		// add force to throw object from player
			GetComponent<Animation>().Play ( "grab_putdown" );										// play the grab put down animation
			isGrabbing = false;														// disable grabbing 
			yield return new WaitForSeconds ( .1f );											// wait a second before adding box collider back in so that it doesn't hit the player
			otherObject.gameObject.AddComponent < BoxCollider >();					// add the box collider back
		}
	}
	void OnTriggerEnter ( Collider other )
	{									// trigger events for collider on grab
		if ( other.tag == "grab" ) 													// if collider equals grabbing object
		{																			// enable grabbing and pushing mode 		
			otherObject = other.transform;											// set other object to collided object
			isPickingUp = true;														// enable picking up 
			grabs = true;															// enable grabs
		}
	}
	void OnTriggerExit 	( Collider other )
	{									// trigger event for exiting collider
		if ( other.tag == "grab" )													// if collider equals grabbing object
		{
			isPickingUp = false;													// turn off picking up
		}
	}

}
