using UnityEngine;
using System.Collections;

/* ---This is the basic Unity Script for Lerpz police character---
// Enemy Controller 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Basic enemy controller for idle, find, attack, die
// Instruction: Assign script to an enemy character and supply the necessary elements in the inspector
*/

public class EnemyController : MonoBehaviour 
{
	public Transform target 							;									// who enemy is looking for
	
	public float attackTurnTime 				 		= 0.7f;						// time to turn on attacking
	public float rotateSpeed 					 		= 120.0f;					// rotational speed
	public float attackDistance 				 		= 17.0f;						// distance for attacking true (gizmo)
	public float extraRunTime 							= 2.0f;						// time to run after attacking
	
	public float attackSpeed 					 		= 5.0f;						// speed of attack movement
	public float attackRotateSpeed 			 			= 20.0f;						// speed of attack rotation
	
	public float idleTime 						 		= 1.6f;						// time to remain idle between checking
	public int damageAmount 				 			= 25;
	public Vector3 hitPosition 							= new Vector3 (0.4f, 0f, 0.7f);// hit sphere position (adjustable in inspector)
	public float hitRadius 					 			= 1.1f;						// hit radius size (adjustable in inspector)
	
	public Transform killParticle 					 ;									// particle to play on death
	public Transform coin							 ;									// coin to spawn after death
	
	public Vector3 hitDirection 					;									// direction of hit
	public bool playerHit 								= false;						// enable player hit
	
	float move;
	float attackAngle 						 			= 10.0f;						// angle to attack
	bool isAttacking 						 			= false;						// enable attacking
	float lastHitTime 						 			= 0.0f;						// last time player hit
	Vector3 offset							;										// vector3 for offset amount
	
	CharacterController characterController;										// instance of character controller
	
	IEnumerator Start 					() 													// initialize everything
	{		
		characterController = GetComponent<CharacterController>();							// get characters controller component

		if ( !target )																	// if no target
			target = GameObject.FindWithTag ( "Player" ).transform;						// set target to object with tag name Player
		
		GetComponent<Animation>().wrapMode 					 = WrapMode.Loop;							// set play animation mode to loop
		GetComponent<Animation>().Play ( "idle" );														// play idle animation
		GetComponent<Animation>() [ "targetEnemy" ].wrapMode = WrapMode.Once;							// set animation to play once
		GetComponent<Animation>() [ "walk" ].wrapMode 		 = WrapMode.Once;							// set animation to play once
		GetComponent<Animation>() [ "die" ].wrapMode  		 = WrapMode.Once;							// set animation to play once
		GetComponent<Animation>() [ "die" ].layer 			 = 1;										// set animation priority high
		
		yield return new WaitForSeconds ( Random.value );											// wait random amount
			
		while ( true )																	// just attack for now
		{	
			yield return Idle ();																// idle till player within range		
			yield return Attack ();															// rotate toward player and attack
		}
	}
	IEnumerator Idle 					() 														// idle mode
	{												
	
		yield return new WaitForSeconds ( idleTime );												// wait during idle time
	
		while ( true )																	// loop tile returning out (which sets to false)
		{
			characterController.SimpleMove ( Vector3.zero );							// setup slight movement if player isn't close by
			yield return new WaitForSeconds ( 0.2f );
			
			offset = transform.position - target.position;								// rotate position amount (offset) betweeen it and player position
				
			if ( offset.magnitude < attackDistance )									// if distance between player and enemy is within attackDistance range
				return false;																	// return out and prepare to attack
		}
	} 
	IEnumerator Attack 				() 														// attack mode
	{												
		isAttacking = true;																// set is attacking to true
	
		GetComponent<Animation>().Play ( "run" );														// play animation run
		
		float angle = 180.0f;															// store angle value of direction
		float time  = 0.0f;																// store time per frame
	
		Vector3 direction;																// direction of object
		
		while ( angle > 5.0f || time < attackTurnTime )									// check on angle, till it's 5 degrees away
		{	
			time += Time.deltaTime;														// grab time frame + var time
			angle = Mathf.Abs ( RotateTowardsPosition ( target.position, rotateSpeed ) ); // rotate towards player at rotateSpeed
			move  = Mathf.Clamp01 ( ( 90 - angle ) / 90 );								// rotate between 90 degree angle
			GetComponent<Animation>()["run"].weight = GetComponent<Animation>() [ "run" ].speed = move;					// begin moving if within angle
			direction = transform.TransformDirection ( Vector3.forward * attackSpeed * move );	// store transform based on forward direction, attack speed and move
			characterController.SimpleMove ( direction );								// use simple move (direction) for basic controls		
			yield return null;																		// yield for a frame
		}
		
		float timer 	= 0.0f;														// timer used to check against extra run time
		bool lostSight  = false;												// check for player lossing sight
	
		while ( timer < extraRunTime )													// check till timer completes
		{
			angle = RotateTowardsPosition ( target.position, attackRotateSpeed );		// store angle of player (between target and attack speed)
	
			if ( Mathf.Abs ( angle ) > 40 )												// angle of vision - larger than 40, can't see player
				lostSight = true;														// lost visual sight
	
			if ( lostSight )															// enable lost player
				timer += Time.deltaTime;												// store time to run just a bit longer till stop
				
			direction = transform.TransformDirection ( Vector3.forward * attackSpeed );	// move forward for a few more steps
			characterController.SimpleMove ( direction );								// move direction 
	
			var pos = transform.TransformPoint( hitPosition );							// store pos for current transform position
			if ( Time.time > lastHitTime + 0.3 && ( pos - target.position ).magnitude < hitRadius )	// check if we can hit player
			{			
				lastHitTime = Time.time;												// grab time for delay on next attack
			}
			if ( characterController.velocity.magnitude < attackSpeed * 0.3 ) 			// not moving forward, ran in to wall or something, stop attacking
				break;
			yield return null;																		// yield for one frame
		}
		GetComponent<Animation>().CrossFade ( "idle" );													// now we can go back to playing the idle animation
	}
	
	public IEnumerator ApplyDamage 			() {												// play death animation
		GetComponent<Animation>().CrossFade ( "die" );													// animation die	
		yield return new WaitForSeconds (.65f);														// wait before instantiating
		Instantiate ( killParticle, transform.position, Quaternion.identity );			// create particle
		Instantiate ( coin, transform.position, Quaternion.identity );					// create/leave coin
		Destroy ( gameObject, .1f );														// destroy enemy
	}
	
	void OnDrawGizmosSelected 	() {												// draw gizmos if selected
		Gizmos.color = Color.yellow;													// color for sphere
		Gizmos.DrawWireSphere ( transform.TransformPoint ( hitPosition ), hitRadius );	// draw sphere
		Gizmos.color = Color.red;														// color for sphere
		Gizmos.DrawWireSphere ( transform.position, attackDistance );					// draw sphere
	}
	
	float RotateTowardsPosition ( Vector3 targetPos, float rotateSpeed) //: float{// rotate towards player 
	{
		Vector3 relative 	= transform.InverseTransformPoint ( targetPos );			// compute relative point and get the angle towards it
		float angle 		= Mathf.Atan2 ( relative.x, relative.z ) * Mathf.Rad2Deg;		// store angle in degrees (x,z)
		float maxRotation 	= rotateSpeed * Time.deltaTime;							// clamp it with the max rotation speed
		float clampedAngle 	= Mathf.Clamp ( angle, -maxRotation, maxRotation );	// clamp angle between min/max
		transform.Rotate ( 0.0f, clampedAngle, 0.0f );										// rotate me 
		return angle;																	// return current angle value
	}

	void OnTriggerEnter ( Collider other )												// if collision with player
	{
		if ( other.tag == "Player" )													// check for tag named player
		{
			ControllerColliderAttack.isAttacking = true;	// just double check that player trigger // player is 'hit', yield for 1 second, then allow enemy to 'hit' again
		}
	}

}
