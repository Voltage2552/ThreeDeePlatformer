using UnityEngine;
using System.Collections;

/*
// Controller System for Player 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Controls all character (player) actions for movement / interactions
// Instruction: Assign script to main character (gameObject). Also, assign a characterController to the component
// Function arguments: Lots. :) Be sure to go through them. 
// Main variables to use are below:
// moveDirection   = player forward direction
// targetDirection = camera forward direction
// inAirVelocity   = speed of player (mainly for jumps)
*/

//[RequireComponent typeof( CharacterController )]								// if no characterController assigned, apply one -later

public class ControllerSystem : MonoBehaviour {

	public SkinnedMeshRenderer skinMeshRenderer;								// need skinned mesh renderer to toggle hide/unhide option for player
	public Camera cameraObject;														// player camera  (usually main camera)
	public GameObject colliderAttack;													// collider for player to attack with
	public GameObject colliderHurt;													// collider for player to get hurt
	public bool canWalk						 			= true;						// enabled walking
	public bool canJog										= true;						// enabled jogging
	public bool canRun							 			= true;						// enabled running
	public bool canSprint						 			= true;						// enabled sprint
	public bool canBoost									= false;					// enable boosted speed
	public bool canJumpAll									= true;						// enabled any jump
	public bool canJump1									= true;						// enabled jump 1
	public bool canJump2									= true;						// enabled jump 2
	public bool canJump3									= true;						// enabled jump 3
	public bool canJumpFromCrouch							= true;						// enabled jumping from crouch
	public bool canJumpFromObject 							= true;						// enabled jumping off object
	public bool canJumpFromAir					 			= true;						// enabled jumping in air
	public bool canControlDecent							= true;						// enabled controlling decent
	public bool canCrouch									= true;						// enabled crouching
	public bool canCrouchHoldKeyDown			 			= true;						// enabled crouching while holding key down
	public bool canAngleSlide					 			= true;						// enabled crouching while holding key down
	public bool canIdleRotate								= true;						// enabled idle turning 
	public bool canJumpFromPad								= true;						// enable jumping from pads
	public bool canFall									= true;						// enable falling from jumps
	public bool canLand									= true;						// enable landing from jumps
	public bool canHurt									= true;						// enable hurting from enemies
	public bool canAttack									= true;						// enable attack ability
	public bool canKillzone								= true;						// enable killzones for player
	public bool canGrab									= true;						// enable pushable objects for player
	public bool canPush									= true;						// enable pushable objects for player
	public bool autoPush									= true;						// enable automatic pushing of objects
	public bool keyboardControls 				 			= false;					// enable keyboard overide for speed control
	
	public float speedIdleMax					 			= 0.2f;						// maxium idle speed before moving 
	public float speedIdleRotate				 			= 1.2f;						// rotate speed on idle turn
	public float speedWalk									= 3.0f;						// maximum walking speed
	public float speedJog									= 5.0f;						// maximum jogging speed
	public float speedRun									= 8.0f;						// maximum running speed
	public float speedSprint								= 12.0f;						// maximum sprint speed
	public float speedBoost 						 		= 20.0f;						// maximum boost speed
	public float speedSlide						 		= 3.0f;						// maximum sliding speed
	public float speedPush									= 1.5f;						// maximum push speed
	public float speedGrab									= 2.0f;						// maximum push speed
	public float speedJumpFromCrouch 						= 3.0f;						// maximum jump from crouch height
	public float speedJumpFromObject 						= 10.0f;						// maximum jump from object height 
	public float speedCrouch						 		= 0.0f;						// maximum crouching speed
	public float speedInAir								= 1.0f;						// float inAirControlAcceleration
	public float speedSmoothing							= 10.0f;						// amount to smooth by
	public float speedRotation								= 50.0f;						// amount to rotate by
	public float targetSpeed								= 0.0f;
	
	public float currentSpeed					 			= 10.0f;						// store speed of character (walk, jog, run, sprint)
	public float currentJumpHeight				 			= 0.0f;						// current height of character
	
	public float jump_1									= 8.0f;						// height for first jump
	public float jump_2									= 10.0f;						// height for second jump
	public float jump_3									= 15.0f;						// height for third jump
	
	public float jumpFromCrouch					 		= 14.0f;						// height for jump from crouch
	public float jumpFromObject					 		= 8.0f;						// height for jump from object
	public string jumpFromObjectTag 						= "wall";					// tag name of object player can jump from
	public float jumpFromAir								= 15.0f;
	
	public float jumpComboTime					 			= 1.5f;						// combo time between jumps to go to next jump mode (jump 1,2,3)
	public float jumpDelayTime 					 		= 0.5f;						// time delay amount (currently used in jump 1 to keep animation from skipping to default stance)
	
	public float crouchControllerHeight  		 			= 1.0f;						// value for height of controller box
	public float crouchControllerCenterY 		 			= 0.5f;						// amount to move controller down 
	
	public string slideTag 								= "slide";					// tag name for any object that player can slide on (you can just set it to slide based on value only
	public float slideThreshold 				 			= 0.88f;						// amount of angle when slide-able
	public float slideControllableSpeed 		 			= 5.0f;						// speed where player still has control sliding down
	
	public float pushPower 					 			= 0.5f;						// how hard the player can push
	public LayerMask pushLayers  					 		= -1;						// layers for pushing objects
	
	public float gravity									= 20.0f;						// gravity (downward pull only, added to vector.y) 
	public int health 										= 100;						// hold health count
	
	public AnimationClip aniIdle_1; 													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniIdle_2;													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniWalk; 														// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniJog;														// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniRun; 														// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniSprint; 													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniCrouchIdle;												// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniLeanLeft;													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniLeanRight;													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniJumpFromCrouch;											// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniJumpFromObject;											// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniJump_1; 													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniJump_2; 													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniJump_3;													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniJumpFall;													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniJumpLand;													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniSlide; 													// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniGrab;		 												// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniGrabIdle;	 												// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniPush;		 												// animation clip for calling up animations (use this rather than direct call)
	public AnimationClip aniLand;		 												// animation clip for calling up animations (use this rather than direct call)
	
	public bool DebugMode									= true;						// sets mode to debug and prints messages to console
	
	CharacterController characterController;									// instance of character controller
	public static float moveSpeed							= 0.0f;						// current player moving speed
	
	private Vector3 moveDirection						=  Vector3.zero;		// store initial forward direction of player
	private Vector3 inAirVelocity						=  Vector3.zero;		// current currentSpeed while in air 
	
	private float smoothDirection			 			= 10.0f;				// amount to smooth camera catching up to player
	private float jumpRepeatTime						= 0.15f;				// amount of time between jumps to make combo happen
	private float jumpFromObjectDelay 					= 0.15f;				// delay time so player can't just jump constantly from objects
	private float jumpDelay								= 0.15f;				// standard jump delay time
	private float groundedDelay							= 0.15f;				// 
	private float cameraTimeDelay						= 0.0f;					// delay on camera time (to allow smoother follow after player movement)
	private float sprintLastTime						= 0.0f;					// time last sprint happened
	private float speedReset							= 0.0f;					// reset speed 
	private float verticalSpeed							= 0.0f;					// speed for vertical use
	private float walkTimeStart				 			= 0.0f;					// store when user started walking to transition to jog
	
	private bool isControllable						= true;						// control for all movement (could be public)
	private bool isMoving							= false; 					// is player pressing any keys
	public static  bool isCrouching				 		= false;					// crouching enabled
	private bool isJumping_1				 		= true;						// jumping 1 enabled
	private bool isJumping_2 			 			= false;					// jumping 2 enabled
	private bool isJumping_3 						= false;					// jumping 3 enabled
	private bool isLanding 							= false;					// is player landing
	private bool isKilled							= false;					// killzone for player
	private float curTime							= 0.0f;						// current time 
	private bool showPlayer				   			= true;						// hide / show player toggle
	private bool resetCharController				= false;					// resets controller for character toggle
	private Vector3 objectJumpContactNormal;									// average normal of the last touched geometry
	private float touchObjectJumpTime 	 			= -1.0f;					// when did we touch the wall the first time during this jump (Used for wall jumping)
	private float wallJumpTimeout 		 			= 0.5f;						// delay time from jumping off walls again
	private bool jumpableObject						= false;					// toggle for jumping off walls
	private float controllerHeightDefault;										// value of controller original height
	private float controllerCenterYDefault;										// value of controller original center Y
	private Vector3 slideDirection;												// direction player is sliding
	private CollisionFlags collisionFlags;										// last collision flag returned from control move
	private int coin 					 				= 0;					// hold coin count	
	private int key  					 				= 0;					// hold key count
	private bool jumpingFromPad 			 			= false;				// disable jumpPad till ready
	private Vector3 playerStartPosition;										// value to hold player position at start
	private Quaternion playerStartRotation;										// value to hold player position at start
	private bool enemyHit;														// enable enemy hitting player 
	private GameObject enemyHurt;												// health reset to store original health value
	private int resetHealth;													// health reset to store original health value
	private Vector3 hitDirection 			 			= new Vector3(0.0f, 10.0f, -2.5f);	// hit direction to send player (could make it complicated, but lets not)
	private Transform pushObject				 		= null;					// store push game object																
	private Transform grabObject				 		= null;					// store grab / pickup / putdown game object																
	private float tempSpeed 				 			= 0.0f;					// hold current speed
	
	
	void Reset					() {											// reset all variables and options to null (0) (add to list as it builds)
		if (!isControllable)
		{
			Input.ResetInputAxes();												// stop all inputs if not controllable
		}
	}
	void Awake 					() {											// before starting, get moveDirection forward from this.gameObject
		moveDirection = transform.TransformDirection ( Vector3.forward ); 				// assign moveDirection local to world forward
	}
	void Start 					() {												// initialize variables
		characterController = GetComponent < CharacterController >();						// initialize characterController
		characterController.tag = "Player";												// set tag name to 'Player'
		controllerHeightDefault = characterController.height;							// set controllerHeightDefault to controllers starting height
		controllerCenterYDefault = characterController.center.y;						// set controllerCenterYDefault to controllers starting center Y
		GetComponent<Animation>().Stop ();																// set animation to stop
		AnimationClipCheck ();															// check animation clips loaded, print missing ones to console	
		playerStartPosition = transform.position;										// store player initial position, move player back to this if he dies
		playerStartRotation = Quaternion.LookRotation (transform.position);				// store player initial rotation 
		resetHealth = health;															// store health value in resetHealth 
		tempSpeed = currentSpeed;														// store tempSpeed of player (used when player pushes)
	}
	void UpdateMoveDirection 	() {												// motor, ani, and direction of player			
		Vector3 forward = cameraObject.transform.TransformDirection ( Vector3.forward );	// forward vector relative to the camera along the x-z plane
		forward.y = 0;																	// up/down is set to 0
		forward = forward.normalized;													// set forward between 0-1	
		Vector3 right = new Vector3( forward.z, 0.0f, -forward.x );						// right vector relative to the camera, always orthogonal to the forward vector
	
		float vertical   = Input.GetAxisRaw ( "Vertical"   );						// get input vertical
		float horizontal   = Input.GetAxisRaw ( "Horizontal" );						// get input horizontal
	
		Vector3 targetDirection = horizontal * right + vertical * forward;		// target direction relative to the camera
	
		if ( IsGrounded () )															// if player on ground
		{
			if ( targetDirection != Vector3.zero )										// store currentSpeed and direction separately
			{
				moveDirection = Vector3.Lerp ( moveDirection, targetDirection, smoothDirection * Time.deltaTime );	// smooth camera follow player direction
				moveDirection = moveDirection.normalized;								// normalize (set to 0-1 value)
			}	
			float currentSmooth = speedSmoothing * Time.deltaTime;				// smooth currentSpeed based on current target direction
			
			targetSpeed = Mathf.Min ( targetDirection.magnitude, 1.0f ); 				// set targetSpeed limit for diagonal movement
			moveSpeed  = Mathf.Lerp ( moveSpeed, targetSpeed * targetDirection.magnitude * currentSpeed, currentSmooth );	// set moveSpeed to smooth to currentSpeed set
	
			jumpableObject = false;														// keep false while on ground
			
			Idle   			();															// check for player idle 
			Crouch 			();															// check for player crouching
			Walk   			();															// check for player walking
			Jog    			();															// check for player jogging
			Run    			();															// check for player running
			Sprint 			();															// check for player sprinting
			StartCoroutine (Jump1 ());															// check for player jumping 1
			StartCoroutine (Jump2 ());															// check for player jumping 2
			Jump3		   	();															// check for player jumping 3
			JumpFromCrouch 	();															// check for player jumping from crouch
			AngleSlide		();															// check for player sliding on slope
			IdleRotate		();															// check for player idle turning
			JumpPad			();															// check for player moving onto jump pad
			StartCoroutine ( Hurt());															// check for player getting hit by enemy
			Attack			();															// check for player attacking with feet collider
			Grab 			();															// check for player grabbing gameObject tagged grab
			Push 			();															// check for player pushing gameObject tagged push
			KeyboardMovementSpeed ();
			Boost			();															//check for player boost
		}
		else																			// if player is in air 
		{										
			inAirVelocity += targetDirection.normalized * Time.deltaTime * speedInAir;	// if in air, move player down based on velocity, direction, time and speed
			JumpFromObject ();															// check for player jumping from objects tagged 'wall'
			JumpFromAir();																// check for player jumping while in air
			Fall ();																	// check if player is falling from jump		
		}
		StartCoroutine (Killzone ());																	// check for player triggering killzone box
	}
	void Update 				() {												// loop for controller
		if ( isControllable )															// if player controllable, then move character
		{
			SetGravity ();																// pulls character to the ground 'if' in air
			UpdateMoveDirection ();														// motor, direction and ani for player movement
	
			Vector3 movement = moveDirection * moveSpeed + new Vector3 ( 0, verticalSpeed, 0 ) + inAirVelocity; // stores direction with speed (h,v)
			movement *= Time.deltaTime;													// set movement to delta time for consistent speed
			
			objectJumpContactNormal = Vector3.zero;										// reset vectors back to zero
			
			collisionFlags = characterController.Move ( movement );						// move the character controller	
			
			if ( IsGrounded () ) 														// character is on the ground (set rotation, translation, direction, speed)
			{
				transform.rotation = Quaternion.LookRotation ( moveDirection );			// set rotation to the moveDirection
				inAirVelocity = new Vector3(0.0f, -0.1f, 0.0f);										// turn off check on velocity, set to zero/// current set to -.1 because zero won't keep him on isGrounded true. goes back and forth			
				if ( moveSpeed < 0.15 ) 												// quick check on movespeed and turn it off (0), if it's
					moveSpeed = 0;														// less than .15
			}
			else 																		// player is in the air
			{
				transform.rotation = Quaternion.LookRotation ( moveDirection );			// quick adjustment for jumping off wall, turn player around in air
			}
		}
		ExampleShowHidePlayer ();														// example usage of show/hide voids	
	}
	
	void Idle 					() {												// idles player
		if ( moveSpeed <= speedIdleMax && !isCrouching )								// check that speed is 0 for idle range
		{
			GetComponent<Animation>().CrossFade ( aniIdle_1.name );										// play animation
			Message ( "Ani State: Idle" );												// print current animation state
		}	
	}
	void Walk 					() {												// walks player
		if ( canWalk )
		{
			if ( moveSpeed > speedIdleMax && moveSpeed < speedJog )						// check that speed is within walk range
			{
				GetComponent<Animation>().CrossFade ( aniWalk.name );									// play animation
				Message ( "Ani State: Walk" );											// print current animation state
			}
		}
	}
	void Jog	 				() {												// jogs player
		if ( canJog )
		{
			if ( moveSpeed > speedWalk && moveSpeed < speedRun ) 					 	// check that speed is within jog range
			{
				GetComponent<Animation>().CrossFade ( aniJog.name );									// play animation
				Message ( "Ani State: Jog" );											// print current animation state
			}
		}
	}
	void Run 					() {												// runs player
		if ( canRun )
		{
			if ( moveSpeed > speedJog && moveSpeed < speedSprint )						// check that speed is within run range 
			{
				GetComponent<Animation>().CrossFade ( aniRun.name );									// play animation
				Message ( "Ani State: Run" );											// print current animation state
			}
		}
	}
	void Sprint 				() {												// sprints player
		if (canSprint)
		{
			if ( moveSpeed > speedRun && moveSpeed <= speedSprint && Input.GetButton( "Fire1" ) )	//check that speed is within sprint range & Fire1 button pressed
			{
				GetComponent<Animation>().CrossFade(aniSprint.name);									//play animation
				Message("Ani State: Sprint");											//print current animation state
			}
		}
	} 
	void Boost()																	// boost player with extra speed
	{
		if (canBoost)
		{
			if ( moveSpeed > speedJog && moveSpeed < speedSprint && Input.GetKey("g"))	// check that speed is within run range and g is pushed
			{
				moveSpeed = speedBoost;													// set speed to boosted speed
				Message ("Player Boosting");
			}
		}
	}
	
	IEnumerator Jump1 				() {												// default jump (if no combo, then defaults to this jump each time)
		if ( canJumpAll )
		{
			if ( !canJump2 )															// if jump_2 turned off, then just repeat jump_1 only
			{
				isJumping_1 = true;														// reset isJumping_1 to true so that it goes back to it
				isJumping_2 = false;													// turn off isJumping_2 to go to jump 1
			}
			if ( canJump1 && !isCrouching && !ControllerColliderGrab.isGrabbing )		// check that player has not picked up an object and if jump 1 enabled and player not crouching (incase toggle is off)
			{
				if ( Input.GetButtonDown ( "Jump" ) && isJumping_1 && !isJumping_2 && !isJumping_3 && curTime + jumpDelayTime < Time.time ) // check for button pressed down "Jump" and isjumping 1 enabled and delay to prevent to fast of jumping
				{
					isJumping_1 = false;												// set jump 1 to false so it just does it once
					curTime = Time.time;												// grab the current actual time to use for testing against next button press
					GetComponent<Animation>().CrossFade ( aniJump_1.name );								// play jump 1 animation
					currentJumpHeight = jump_1;											// set jump 1 height to current jump height
					inAirVelocity.y = currentJumpHeight;								// set current jump to in Air Y (don't have to by-pass twice, just doing it)
					Message ( "Ani State: Jump_1" );									// print on debug that we jumped
				}	
				else if ( IsGrounded () && !isJumping_1 && !isJumping_2 && !isJumping_3 )	// if the player jumped and on the ground, then setup for combo
				{
					Message ( "Combo 2 ready to go!");									// print that you can go to combo jump 2 
					yield return null;
					isJumping_1 = false;												// keep jump 1 set to false
					isJumping_2 = true;													// set jump 2 to true 
				}
				else if ( ControllerColliderHurt.enemyHit || ControllerColliderAttack.isAttacking )
				{
					inAirVelocity.y = 0;
					isJumping_1 = false;
				}
			}
		}
	}
	IEnumerator Jump2					() {												// jump 2 in combo
		if (canJumpAll)
		{
			if (canJump2)
			{
				if (Input.GetButtonDown("Jump") && !isJumping_1 && isJumping_2 && !isJumping_3 && Time.time < (curTime +jumpComboTime) )
				{
					isJumping_2 = false;												// turn off Jump 2
					curTime = Time.time;												// grab current time for next button press
					GetComponent<Animation>().CrossFade(aniJump_2.name);								// play animation for jump 2
					currentJumpHeight = jump_2;											// set current jump height to jump 2 height
					inAirVelocity.y = currentJumpHeight;								// set air Y to current jump height
					Message("Ani State: Jump 2 Combo");									// print that jump combo 2 executed
					yield return null;												// wait one cycle to delay isJumping_3
					isJumping_3 = true;													// after one cycle, set isJumping_3 to true
				}
				
				if(!isJumping_1 && isJumping_2 && !isJumping_3 && Time.time > (curTime +jumpComboTime) )
				{
					isJumping_2 = false;												//turn off jump 2
					isJumping_1 = true;													// reset jump 1 - go back to start
					curTime = Time.time;												// grab current time for next jump
					Message("Ani State: Missed Combo 2, Resetting to Jump_1");			// print that you missed the combo opportunity
				}
			}
		}
	}
	void Jump3					() {													// jump 3 in combo (final jump)
		if (canJumpAll)																	// toggle for all jumps
		{
			if(!canJump3 && isJumping_3)												//if jump 3 turned off, repeat jump 1 and 2
			{
				isJumping_1 = true;														// set jump 1 to true
				isJumping_3 = false;													// set jump 3 back to false
			}
		
			if (canJump3)
			{
				if (Input.GetButtonDown("Jump") && !isJumping_1 && !isJumping_2 && isJumping_3 && Time.time < (curTime +jumpComboTime) )
				{
					isJumping_3 = false;												// turn off jump 3
					isJumping_1 = true;													// set jump 1 to true
					curTime = Time.time;												// get current time
					GetComponent<Animation>().CrossFade(aniJump_3.name);								// play animation for jump 3
					currentJumpHeight = jump_3;											// set current jump height to jump 3
					inAirVelocity.y = currentJumpHeight;								// set air velocity y to current jump height
					Message("Ani State: Jump 3 Combo");									// print that you just did a jump combo 3
				}
				
				if(!isJumping_1 && !isJumping_2 && isJumping_3 && Time.time > (curTime + jumpComboTime) )	// missed combo 3, reset to jump 1
				{
					isJumping_3 = false;												// turn off jump 3
					isJumping_1 = true;													// set jump 1 to true
					Message("Jump 3 Combo Missed. Resetting to Jump_1");				// print that you missed jump combo 3
				}
			}
		}
	}
	
	
	
	void JumpFromAir			() {
		if(canJumpAll)
		{
			if (canJumpFromAir)
			{
				if (!IsGrounded() )
				{
					if (Input.GetButtonDown ("Jump") )
					{
						currentJumpHeight = jumpFromAir;
						inAirVelocity.y = currentJumpHeight;
					}
				}
			}
		}
	}
	
	void JumpFromCrouch			() {												// jump from crouch position
		if(canJumpAll)
		{
			if(canJumpFromCrouch)														//toggle jumping from crouch
			{
				if(isCrouching && Input.GetButtonDown("Jump"))							//check for is crouching and button down
				{
					isCrouching = false;												//set crouching to false
					GetComponent<Animation>().CrossFade(aniJumpFromCrouch.name);						//play crouch jump animation
					currentJumpHeight = jumpFromCrouch;									//set current jump height
					inAirVelocity.y = currentJumpHeight;								//set air velocity
					Message("Ani State: Jump from Crouch");								//send a message for debug state
				}
			}
		}
	}
	void JumpFromObject			() {												// jumping from an object
		if (canJumpFromObject)
		{
			if(jumpableObject && !IsGrounded() && Input.GetButtonDown("Jump"))			// if player in air, jump button pressed, and is pressed against wall-jumpable object
			{
				jumpableObject = false;													// reset to play once
				
				if(collisionFlags == CollisionFlags.CollidedSides)						// only if player is colliding from sides
				{
					isJumping_1 = true;													// enable jump 1 - this will keep it useable even after grounded
					isJumping_2 = false;												// disable jump 2
					isJumping_3 = false;												// disable jump 3
				}
				
				if(Mathf.Abs(objectJumpContactNormal.y) < 0.2)							// check angle of normal
				{
					GetComponent<Animation>().Play(aniJumpFromObject.name);								// play falling animation
					Message("Ani State: Jump from Object");								// print current animation state
					objectJumpContactNormal.y = 0;										// reset y to 0
					moveDirection = objectJumpContactNormal.normalized;					// normalize player direction
					moveSpeed = speedJumpFromObject;									// set move speed to jump from object speed
				}
				else
				{
					moveSpeed = 0;														// if it doesn't work, set move to 0
				}
				verticalSpeed = .02f;													// add just a bit to vertical speed so player doesn't start falling from gravity
				inAirVelocity.y = jumpFromObject;										// set jumpFromObject amount for y
			}
		}
	}
	void JumpPad				() {												// jump from crouch position
		if (canJumpFromPad)																// toggle jumping from pad
		{
			if (jumpingFromPad)															// check for jumpingFromPad
			{
				jumpingFromPad = false;													// turn off jump pad activation
				GetComponent<Animation>().CrossFade(aniJumpFall.name);									// play jump animation
				currentJumpHeight = jumpFromCrouch;										// set current jump height
				inAirVelocity.y = currentJumpHeight;									// set air velocity
				Message("Ani State: Jump From Pad");									// send debug message
			}
		}
	}
	void AngleSlide				() {												// sliding if slope (angle) too much
		if (canAngleSlide)																//toggle can angle slide
		{
			slideDirection = Vector3.zero;												// set slide direction to zero
			RaycastHit hitInfo;
			
			if (Physics.Raycast (transform.position, Vector3.down, out hitInfo))			// store hit information
			{
				if (hitInfo.collider.tag != slideTag)									// cas a ray and find objects position and what it hit
				{
					return;
				}
				
				if (hitInfo.normal.y < slideThreshold)									// if our normal directon on that is less than slide threshold
				{
					slideDirection = new Vector3(hitInfo.normal.x, 0, hitInfo.normal.z);	//direction for the player to slide, y is set to 0
				}
			}
			
			if (slideDirection.magnitude < slideControllableSpeed)						// checks against slide speed, if less than 
			{
				moveDirection += slideDirection;										// this allows for a bit of movement (kinda jerky), clean up
			}
			else
			{
				moveDirection = slideDirection;											// force the slide to go straight down based on slide direction
			}
			
			if (slideDirection.magnitude > 0)											// if player is sliding
			{
				moveSpeed = speedSlide;													// move player down at slide speed
				GetComponent<Animation>().CrossFade(aniSlide.name);										// play slide animation while moving down
				Message("Ani State: Sliding Down");										// print animation state
			}
		}
	}
	void IdleRotate				() {												// turn left/right while in idle 
		if ( canIdleRotate )															// toggle idle rotate (turn left / right)
		{
			if ( Input.GetAxis ( "Horizontal" ) > 0 && Input.GetAxis ( "Horizontal" ) < speedIdleMax || Input.GetAxis( "Vertical" ) > 0 && Input.GetAxis ( "Vertical" ) < speedIdleMax ) 		// check for horizontal movement only at idle value
			{
				GetComponent<Animation>().CrossFade( aniWalk.name );									// play animation
				transform.eulerAngles += new Vector3 (0.0f, Input.GetAxis ("Horizontal") * speedIdleRotate, 0.0f);	// rotate based on horizontal movement / add deltaTime
				Message ( "Ani State: Idle Turn Right" );								// print animation state 
			}
			else if ( Input.GetAxis ( "Horizontal" ) < 0 && Input.GetAxis ( "Horizontal" ) > -speedIdleMax  || Input.GetAxis( "Vertical" ) < 0 && Input.GetAxis ( "Vertical" ) > -speedIdleMax  ) // check for horizontal movement only at idle value
			{
				GetComponent<Animation>().CrossFade ( aniWalk.name );									// play animation
				transform.eulerAngles -= new Vector3(0.0f, Input.GetAxis ( "Horizontal" ) * -speedIdleRotate, 0.0f);	// rotate based on horizontal movement / add deltaTime
				Message ( "Ani State: Idle Turn Left" );								// print animation state
			}
		}
	}
	void Crouch					() {												// crouch player
		if (canCrouch)																	// toggle crouching
		{
			if (canCrouchHoldKeyDown)													// if crouch while holding button enabled
			{
				if (Input.GetButton("Crouch"))											// if button pressed / held down
				{
					isCrouching = true;													// set crouch to true
				}
				else																	// if button down
				{
					isCrouching = false;												// set crouch to false
				}
			}
			else
			{
				if(Input.GetButtonDown("Crouch"))										// if button pressed down, then
				{
					isCrouching = !isCrouching;											// set crouch to false and
				}
			}
			
			if (isCrouching)															// if crouch == true
			{
				Vector3 crouchCharCenterY = new Vector3(0.0f, crouchControllerCenterY, 0.0f);
				
				moveSpeed = speedCrouch;												// change moveSpeed to crouching speed
				characterController.height = crouchControllerHeight;					// use crouchControllerHeight for controller height
				characterController.center = crouchCharCenterY;					// move height y, down just a bit when character crouches
				//canJump = false;														// toggle jumping off
				GetComponent<Animation>().CrossFade(aniCrouchIdle.name);								// animation for crouch idle
				Message ("Ani State: Crouch");											// print state of animation
			}
			
			if (Input.GetButtonUp("Crouch"))											// when button is up for crouch
			{
				resetCharController = true;												// reset character controller height
			}
			
			if (resetCharController)													// if reset enabled
			{
				float tempGravity = gravity;										// hold gravity value temporarily
				gravity = 0;															// turn off gravity
				resetCharController = false;											// turn off reset controller
				characterController.height = controllerHeightDefault;					// set controller height back to default
				characterController.center = new Vector3(0.0f, controllerCenterYDefault, 0.0f);				// set controller centerY back to default
				gravity = tempGravity;													// set gravity back to original value
			}
		}
	}
	void Fall					() {												// player is in the air
		if ( canFall )
		{
			if ( slideDirection.magnitude <= 0 )										// if player not on ground and not sliding
			{	 
				GetComponent<Animation>().CrossFadeQueued ( aniJumpFall.name, 0.3f, QueueMode.CompleteOthers );	// animation for falling
				Message ( "Ani State: Fall" );											// print animation state
			}
		}
	}
	void Attack					() {												// player jumps on enemy head - with feet
		if (canAttack)
		{
			if (colliderAttack == null)
			{
				Message("No target to attack");
				return;
			}
			if(ControllerColliderAttack.isAttacking)
			{
				ControllerColliderAttack.isAttacking = false;
				inAirVelocity.y = 10;
				GetComponent<Animation>().Play(aniJump_2.name);
				Message("Player attacked and killed enemy");
			}
		}
	}
	IEnumerator Hurt					() {												// player hurt by enemy objects
		if(canHurt)																		// toggle if player can be hurt
		{								
			if(colliderHurt == null)													// check for collider hurt
			{
				Message("Need collider hurt assigned");									// print no collider found
				return false;																	// return out
			}
			
			if(ControllerColliderHurt.enemyHit)											// if enemyHit enabled
			{
				ControllerColliderHurt.enemyHit = false;								// disable enemyHit
				inAirVelocity = transform.TransformDirection(hitDirection);				// set in air vel to slam direction
				GetComponent<Animation>().Play(aniJumpLand.name);										// play hurt animation (using jumpLand)
				health -= ControllerColliderHurt.damageAmount;							// set player health to decrease by damageAmount
				yield return null;
				
				if(health <= 0)															// if health reaches zero, kill player
				{
					isKilled = true;													// enable isKilled for player
					Message("Player was killed and his enemies feasted on his remains. Starting over");
				}
				Message("Player was hurt");
			}
		}
	}
	IEnumerator Killzone				() {												// player killed if in this area, respawn at start
		if ( canKillzone )																// toggle killzone areas
		{
			if ( isKilled ) 															// player killed enabled
			{
				isKilled = false;														// turn off isKilled
				HidePlayer ();															// hide player
				yield return new WaitForSeconds (1);												// wait one second before moving player - keeps camera in that spot
				GetComponent<Animation>().Stop();														// stop current animation
				transform.rotation = playerStartRotation;								// set rotation just in case
				transform.position = playerStartPosition;								// move player to original starting point (stored in Start())
				moveDirection = new Vector3(0.0f, 0.0f, 0.1f);										// set player move speed to almost zero (when he comes back in we want him stopped)- almost zero throws error in update
				yield return new WaitForSeconds (1);												// wait one second before showing player
				GetComponent<Animation>().Play ( aniIdle_1.name );										// play animation
				Message ( "Ani State: Idle" );											// print current 
				isJumping_1 = true;														// reset jumping to true
				isJumping_2 = false;													// reset jumping to false
				isJumping_3 = false;													// reset jumping to false
				health = resetHealth;													// reset health to default
				ShowPlayer ();															// show player			
			}
		}
	}
	void Push 					() {												// player can push objects by moving in to them
		if ( canPush )																	// toggle push
		{
			if ( ControllerColliderPush.push && autoPush )								// check push collider and auto pushing on
			{	
				if ( moveSpeed < speedIdleMax ) 										// check for move speed < idle maximum
				{
					ControllerColliderPush.push = false;								// set push to false
					ControllerColliderPush.isPushing = false;							// set is pushing to false
				}
				if ( ControllerColliderPush.isPushing && !isCrouching && !ControllerColliderGrab.isGrabbing && moveSpeed >= speedIdleMax )	// is pushing and not crouching
				{
					currentSpeed = speedPush;											// when push player slows to half speed				
					GetComponent<Animation>().CrossFade ( aniPush.name );								// play push animation if button pressed																			// print pushing it							
				}		
				if (  !ControllerColliderPush.isPushing && moveSpeed < speedIdleMax )	// when button up, detach object from player
				{	
					currentSpeed = tempSpeed;											// reset currentSpeed to default
					Message ("Dropping it");											// print dropping it
				}
			}
			else if ( ControllerColliderPush.push && !autoPush )						// if autopush off, then require button press to push objects
			{
				if ( Input.GetButton ( "RightBumper" ) && ControllerColliderPush.isPushing && !isCrouching && !ControllerColliderGrab.isGrabbing && moveSpeed >= speedIdleMax )
				{
					currentSpeed = speedPush;											// when push player slows to half speed				
					GetComponent<Animation>().CrossFade ( aniPush.name );								// play push animation if button pressed		
				}
				if ( Input.GetButtonUp ( "RightBumper" ) && ControllerColliderPush.isPushing )	// when button up turn off pushing
				{
					currentSpeed = tempSpeed;											// reset currentSpeed to default
					Message ("Dropping it");											// print dropping it				
				}
			}
		}
	}
	void Grab					() {												// player can grab objects
		if(canGrab)
		{
			if(ControllerColliderGrab.isGrabbing && !isCrouching && moveSpeed <= speedIdleMax)
			{
				GetComponent<Animation>().CrossFade(aniGrabIdle.name);
				Message("Ani State: Grab Idle");
			}
			else if (ControllerColliderGrab.isGrabbing && !isCrouching && moveSpeed > speedIdleMax)
			{
				currentSpeed = speedGrab;
				GetComponent<Animation>().CrossFade(aniGrab.name);
				Message("Ani State: Grab Walk");
			}
			else
			{
				currentSpeed = tempSpeed;
			}
		}
	}
	void ShowPlayer				() {												// turn player rendering mesh 'on'
		skinMeshRenderer.enabled 	= true;
		isControllable				= true;
	}
	void HidePlayer				() {												// turn player rendering mesh 'off'
		skinMeshRenderer.enabled 	= false;
		isControllable 				= false;
	}
	void KeyboardMovementSpeed 	() {												// controls for keyboard movement/speed
		if ( keyboardControls )															// enable keyboard controls if no joystick
		{
			currentSpeed = 3;															// hardcode cur speed to 3
			float curTimer 	= 0.0f;												// store current time of game
			float curSmooth  = speedSmoothing * Time.deltaTime;					// smooth out with speed value and delta
			bool timeStopped = false;											// toggle for stopping time
	
			if ( !Input.anyKey )														// if no key is pressed
			{
				moveSpeed = 0;															// set moving to zero
			}
			
			if ( moveSpeed < speedIdleMax )												// if still within idle range
			{
				curTimer = Time.time;													// grab current time
			}
			
			if ( Input.GetButton ("Fire3") && !ControllerColliderGrab.isGrabbing )		// pushing 'control' while moving makes player sprint
			{
				targetSpeed = speedSprint;
			}
			else if ( moveSpeed > speedIdleMax )
			{
				if ( Time.time - 1 >= curTimer )
				{ 
					targetSpeed = speedWalk;
				}
				if ( Time.time - 2 > curTimer )
				{
					targetSpeed = speedJog;
				}
				if ( Time.time - 3 > curTimer )
				{
					targetSpeed = speedRun;
				}
				if ( Time.time - 6 > curTimer )
				{
					targetSpeed = speedSprint;
				}
			}		
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		}
	}
	
	bool IsGrounded 			() {												// check if player is touching the ground or a collision flag
	return ( collisionFlags & CollisionFlags.CollidedBelow ) != 0;					// if isGround not equal to 0 if it doesn't equal 0
	}
	void SetGravity				() {												// sets gravity to 0 for ground and subtracts if in air
		if ( IsGrounded () )
			verticalSpeed = 0.0f;														// stop subtracting, if player on ground set to 0
		else
			verticalSpeed -= gravity * Time.deltaTime;									// if character in air, begin moving downward
	}
	void Message ( string text ) {												// debug mode handling for development - easy toggle on/off
		if ( DebugMode )
			Debug.Log ( text );
	}
	void Message ( float text )  {												// debug mode handling for development - easy toggle on/off
		if ( DebugMode )
			Debug.Log ( text );
	}
	void Message ( int text )    {												// debug mode handling for development - easy toggle on/off
		if ( DebugMode )
			Debug.Log ( text );
	}
	void AnimationClipCheck 	() {												// in debug mode, check for clip, if null, put in default ani
		if ( !DebugMode ) return;
		
		if ( aniIdle_1 			== null ) {	Debug.Log ( "Missing Animation Clip: idle_1, adding default" 			); aniIdle_1 		 = GetComponent<Animation>().clip; }
		if ( aniIdle_2 			== null ) {	Debug.Log ( "Missing Animation Clip: idle_2, adding default"  			); aniIdle_2 		 = GetComponent<Animation>().clip; }
		if ( aniWalk 			== null ) {	Debug.Log ( "Missing Animation Clip: walk, adding default"  			); aniWalk 	 		 = GetComponent<Animation>().clip; }
		if ( aniJog 			== null ) {	Debug.Log ( "Missing Animation Clip: jog, adding default"  				); aniJog 			 = GetComponent<Animation>().clip; }
		if ( aniRun 			== null ) {	Debug.Log ( "Missing Animation Clip: run, adding default"  				); aniRun 		 	 = GetComponent<Animation>().clip; }
		if ( aniSprint 			== null ) {	Debug.Log ( "Missing Animation Clip: sprint, adding default"  			); aniSprint 		 = GetComponent<Animation>().clip; }
		if ( aniCrouchIdle 		== null ) {	Debug.Log ( "Missing Animation Clip: crouch idle, adding default"  		); aniCrouchIdle 	 = GetComponent<Animation>().clip; }
		if ( aniLeanLeft 		== null ) {	Debug.Log ( "Missing Animation Clip: lean left, adding default"  		); aniLeanLeft 		 = GetComponent<Animation>().clip; }
		if ( aniLeanRight 		== null ) { Debug.Log ( "Missing Animation Clip: lean right, adding default"  		); aniLeanRight 	 = GetComponent<Animation>().clip; }
		if ( aniJumpFromCrouch 	== null ) {	Debug.Log ( "Missing Animation Clip: jump from crouch, adding default"  ); aniJumpFromCrouch = GetComponent<Animation>().clip; }
		if ( aniJumpFromObject 	== null ) { Debug.Log ( "Missing Animation Clip: jump from object, adding default"  ); aniJumpFromObject = GetComponent<Animation>().clip; }
		if ( aniJump_1 			== null ) { Debug.Log ( "Missing Animation Clip: jump_1, adding default"  			); aniJump_1 		 = GetComponent<Animation>().clip; }
		if ( aniJump_2 			== null ) {	Debug.Log ( "Missing Animation Clip: jump_2, adding default"  			); aniJump_2 		 = GetComponent<Animation>().clip; }
		if ( aniJump_3 			== null ) {	Debug.Log ( "Missing Animation Clip: jump_3, adding default"  			); aniJump_3 		 = GetComponent<Animation>().clip; }
		if ( aniJumpFall 		== null ) {	Debug.Log ( "Missing Animation Clip: jump fall, adding default"  		); aniJumpFall 		 = GetComponent<Animation>().clip; }
		if ( aniJumpLand 		== null ) {	Debug.Log ( "Missing Animation Clip: jump land, adding default"  		); aniJumpLand 		 = GetComponent<Animation>().clip; }
		if ( aniSlide 			== null ) {	Debug.Log ( "Missing Animation Clip: slide, adding default"  			); aniSlide 		 = GetComponent<Animation>().clip; }
		if ( aniGrab	 		== null ) {	Debug.Log ( "Missing Animation Clip: grabPush, adding default"  		); aniGrab			 = GetComponent<Animation>().clip; }
		if ( aniGrabIdle	 	== null ) {	Debug.Log ( "Missing Animation Clip: grabIdle, adding default"  		); aniGrabIdle		 = GetComponent<Animation>().clip; }
		if ( aniPush	 		== null ) {	Debug.Log ( "Missing Animation Clip: aniPush, adding default"  			); aniPush	 		 = GetComponent<Animation>().clip; }
	}
	void ExampleShowHidePlayer 	() {												// example show hide player - shown in update function 
		if ( Input.GetKeyDown ( "h" ) )													// example of showPlayer function toggle
		{
			showPlayer = !showPlayer;
			if (  showPlayer ) ShowPlayer ();											// show player (render mesh)
			if ( !showPlayer ) HidePlayer ();											// hide player (render mesh off)
		}
	}
	IEnumerator OnTriggerEnter ( Collider other ) {										// trigger events for coin, key, bridge, jumpPad
		if (other.tag == "jumpPad")
		{
			jumpingFromPad = true;
			isJumping_1 = false;
			isJumping_2 = false;
			isJumping_3 = false;
			Message("JumpPad activated");
			
			if (other.GetComponent<Animation>() != null)
			{
				other.GetComponent<Animation>().Play("jumpPad_up");
				yield return new WaitForSeconds (.3f);
				other.GetComponent<Animation>().Play("jumpPad_down");
			}
		}
		
		if (other.tag == "killzone")													// check for killzone tag
		{
			isKilled = true;															// enable player killed
		}
		
		if (other.tag == "coin")														// check for coin tag
		{
			coin += 1;																	// add a coin to coins
			Destroy(other.gameObject);													// remove the coin object
			Message ("You have collected " + coin + " coins");							// print coin collected
		}
		
		if (other.tag == "key")															// check for key tag
		{
			key += 1;																	// add a key to keys
			Destroy(other.gameObject);													// remove the key object
			Message ("You have collected " + key + " keys");							// print key collected
		}
		
		if (other.tag == "bridge")
		{
			if (key < 1)
			{
				Message("You must have 1 key to unlock the bridge");
			}
			
			if (key >= 1)
			{
				key -= 1;
				other.GetComponent<Animation>().Play("bridge_down");
				Message("Your keys unlocked the bridge!");
			}
		}
	}
	void OnTriggerStay  ( Collider other ) {										// trigger event while in collider (for platforms)
		if (other.tag == "platform")
		{
			this.transform.parent = other.transform.parent;
			print ("Jumped on a platform");
		}
	}
	void OnTriggerExit  (Collider other ) {										// trigger even when leaving collider (for platforms)
		if(other.tag == "jumpPad")
		{
			jumpingFromPad = false;
		}
		
		if(other.tag == "platform")
		{
			this.transform.parent = null;
			print ("no longer on object");
		}
	}
	void OnControllerColliderHit ( ControllerColliderHit hit) {					// check for raycast hit from controller
		Debug.DrawRay ( hit.point, hit.normal );										// draw line showing direction of ray cast
		if ( hit.moveDirection.y > 0.01 && isJumping_3 ) 								// if player hits head (top collider) then let's move player down so it doesn't hang in the checking for jump3 as well
		{
			inAirVelocity.y = 0;														// quick fix - set player movement speed to 0, moving player back down
			Message ( "Player hit top (head) collider. Moving back down" );				// print player hit head
			return;																		// return out
		}
		if ( hit.collider.tag == jumpFromObjectTag ) 									// if hit tag is equal to the wall tag
		{	
			jumpableObject = true;														// set jump object true
		}
		
		objectJumpContactNormal = hit.normal;											// store jumps contact normal direction	
		
		Rigidbody body = hit.collider.attachedRigidbody;							// set body to hit rigidbody
		
		if ( body == null || body.isKinematic )											// no rigidbody
			return;
	
		int bodyLayerMask = 1 << body.gameObject.layer;							// only push rigidbodies in the right layers or default 1
		if ( ( bodyLayerMask & pushLayers.value ) == 0 ) 								// if not on the right layers, break out
			return;	
	
		if ( hit.moveDirection.y < -0.3 ) 												// we dont want to push objects below us
		{
			return;										
		}		
		Vector3 pushDir = new Vector3( hit.moveDirection.x, 0.0f, hit.moveDirection.z );// calculate push direction from move direction, we only push objects to the sides, never up and down
		body.velocity = pushDir * pushPower; 											// push object based on direction and strength						
	}

	void OnGUI 					() {												// quick gui for coins and key display
		GUI.Box   ( new Rect ( 0,0, 100, 60  ), "" );										// gui box for background	
		GUI.Label ( new Rect ( 10,5,100,100  ), "Health: " + health );						// gui label to show coin and current value
		GUI.Label ( new Rect ( 10,20,100,100 ), "Coins: "  + coin   );						// gui label to show coin and current value
		GUI.Label ( new Rect ( 10,35,100,100 ), "Keys: "   + key    );						// gui label to show key and current value
	}	
}
