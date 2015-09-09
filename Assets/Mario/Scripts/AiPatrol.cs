using UnityEngine;
using System.Collections;

//[RequireComponent ( CharacterController )]																	// make sure there is always a character controller
public class AiPatrol : MonoBehaviour {

	/* ---Unity Script---
// Ai Patrol 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Follow (patrol) from/to each waypoint in calculated group (based on autowaypoint.js)
// Instruction: Assign script to a character (npc) that will walk around
//

float speed 					 		 		= 3.0f;															// movement speed for player
float rotationSpeed 			 		 		= 5.0f;															// rotation speed for player
float pickNextWaypointDistance 			 		= 2.0f;															// distance of next waypoint


void Start  			() 					 	{																// initialize
	Patrol ();																										// start patroling
}
	
IEnumerator Patrol 			() 					 	{																// pick waypoints and move 
	var curWayPoint = AutoWayPoint.FindClosest ( transform.position );												// use autoWayPoint script to find waypoint position
	while ( true ) 																									// constantly patrol
	{
		Vector3 waypointPosition = curWayPoint.transform.position;											// set way point position to current way point position

		if ( Vector3.Distance ( waypointPosition, transform.position ) < pickNextWaypointDistance )					// if current distance is < next distance then 
			curWayPoint = PickNextWaypoint ( curWayPoint );															// keep moving player toward waypoint position
	
		MoveTowards ( waypointPosition );																			// move towards our target
		
		yield return(0);																										// yield one frame
	}
}
void MoveTowards 		( Vector3 position ) 	{																// move player towards position (waypoint)
	Vector3 direction = position - transform.position;																	// set position based on current waypoint - current position 
	direction.y = 0;																								// y movement set to 0

	transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.LookRotation ( direction ), rotationSpeed * Time.deltaTime );// rotate towards the target
	transform.eulerAngles = Vector3 ( 0, transform.eulerAngles.y, 0 );

	Vector3 forward 		 = transform.TransformDirection ( Vector3.forward );								// modify speed so we slow down when we are not facing the target
	float speedModifier 	 = Vector3.Dot ( forward, direction.normalized );									// stores angle between vectors
	speedModifier 				  = Mathf.Clamp01 ( speedModifier );												// clamp between 0 and 1
	direction 					  = forward * speed * speedModifier;												// move the character based on direction, speed and angle bet
	GetComponent ( CharacterController ).SimpleMove ( direction );													// use simplemove function for character controller - basic moving system
	
	animation.Play ( "walk" );																						// play animation - direct call to walk from animation component
}
	/*
void PickNextWaypoint 	( AutoWayPoint currentWaypoint ) {													// get next way point from auto way point script
	
	Vector3 forward = transform.TransformDirection ( Vector3.forward );										// the forward direction in which we are walking
	AutoWayPoint best = currentWaypoint;																		// the closer two vectors, the larger the dot product will be.
	float bestDot = -10.0;																					// value for best waypoint
	
	foreach ( AutoWayPoint cur in currentWaypoint.connected ) 													// loop through all way points in group
	{
		Vector3 direction = Vector3.Normalize ( cur.transform.position - transform.position );				// find direction normalized between cur waypoint and current position
		float dot = Vector3.Dot ( direction, forward );														// set which direction it faces (1 is same direction, -1 opposite, 0 perpendicular)
		if ( dot > bestDot && cur != currentWaypoint ) 															 	// if player is not to the waypoint yet
		{
			bestDot = dot;																							// set bestDot to dot value
			best = cur;																								// set waypoint to current waypoint
		}
	}
	return best;																									// return best (current waypoint)
	}
	 */
}
