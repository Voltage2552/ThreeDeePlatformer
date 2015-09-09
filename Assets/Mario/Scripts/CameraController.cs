using UnityEngine;
using System.Collections;

/*
// Camera Controller (Parts and pieces from everywhere)
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Controls basic movement of camera
// Instruction: Assign script to any camera and complete inspector options 
*/

public class CameraController : MonoBehaviour 
{

	public Transform target;											// target for camera to look at
	public float targetHeight 					= 1.0f;					// height of target
	public LayerMask collisionLayers   		= -1;					// collision layers for camera
	public float distance 						= 8.0f;					// distance between target and camera
	public float xSpeed 						= 250.0f;				// movement on horizontal
	public float ySpeed 						= 120.0f;				// movement on vertical
	public float yMinLimit 					= -12.0f;					// limit how low on vertical to rotate
	public float yMaxLimit 					= 80.0f;					// limit how high on vertical to rotate
	public float rotationSpeed 				= 3.0f;					// speed of rotation
	public float zoomMinLimit 					= 2f;					// limit how close to zoom in (mouse wheel roll)
	public float zoomMaxLimit 					= 6f;					// limit how far to zoom out (mouse wheel roll)
	public float zoomDampening 				= 5.0f; 					// speed of zoom easing
	public float offsetFromWall 				= 0.1f;					// distance away from walls
	
	Vector3 position;
	
	private float x 			 		= 0.0f;					// store axis x from input
	private float y 			 		= 0.0f;					// store axix y from input
	private float currentDistance; 								// current distance between target and camera
	private float desiredDistance;								// wanted distance between target and camera
	private float correctedDistance; 								// amount to correct for between target and camera
	
	
	void Start () 
	{																					// initialize 
	    Vector2 angles = transform.eulerAngles;															// set vector 2 values from this transform (camera)
	    x = angles.y;																							// set x to equal angle x
	    y = angles.x;																							// set y to equal angle y
		
	    currentDistance   = distance; 																			// set default distance
	    desiredDistance   = distance; 																			// set default distance
	    correctedDistance = distance; 																			// set default distance
	}
	
	void LateUpdate ()
	{																					// after character moves and animations play
		Vector3 vTargetOffset;																			// store vertical target offset amount (x,y,z)
	
		x += Input.GetAxis("CameraX") * xSpeed * 0.02f;															// set x to axis movement horizontal
		y -= Input.GetAxis("CameraY") * ySpeed * 0.02f;															// set y to axis movement vertical
		
		y = ClampAngle(y, yMinLimit, yMaxLimit);																// clamp the vertical movement between a min and max

		Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(y, x, 0), Time.deltaTime * 3);		// set rotation value to equal the rotation of the camera and time
	
		vTargetOffset = new Vector3 (0, -targetHeight, 0);														// calculate desired camera position
		position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset); 			// set camera position and angle based on rotation, wanted distance and target offset amount
	
		RaycastHit collisionHit; 																			// set a ray cast
		Vector3 trueTargetPosition = new Vector3 (target.position.x, target.position.y + targetHeight, target.position.z);		// check for collision using the true target's desired registration point as set by user using height  
	
		bool isCorrected = false; 																		// check for movement of camera corrected because of collision
		
		if (Physics.Linecast (trueTargetPosition, position, out collisionHit, collisionLayers.value)) 				// if there was a collision, correct the camera position and calculate the corrected distance  
		{ 
			correctedDistance = Vector3.Distance (trueTargetPosition, collisionHit.point) - offsetFromWall;		// corrected distance takes distances between target and hit point - an offset from wall to prevent clipping
			isCorrected = true;																					// if collided, set corrected to true
		}	
	
		if ( !isCorrected || correctedDistance > currentDistance )												// check if distance has not been corrected or greater than current distance
		{
			currentDistance = Mathf.Lerp (currentDistance, correctedDistance, Time.deltaTime * zoomDampening);	// for smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance 
		}
		else 																									// else there was a collision (linecast) and we need to move the camera to the corrected amount
		{
			isCorrected = false;																				// set back to false so camera will lerp after corrected
			currentDistance = correctedDistance;																// else set current distance of camera to corrected amount
		}
		
		currentDistance = Mathf.Clamp (currentDistance, zoomMinLimit, zoomMaxLimit); 							// keep within legal limits
		position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset); 			// recalculate position based on the new currentDistance 
	
		transform.rotation = rotation;																			// set camera rotation to current rotation amount
		transform.position = position;																			// set camera position to current position amount
	}
	static float ClampAngle (float angle, float min, float max)
	{										// limit angle amount for vertical rotation
	    if (angle < -360)																						// if angle is less than -360
	    {
	        angle += 360.0f;																						// angle + 360
	    }
	    if (angle > 360)																						// if angle is greater than 360
	    {
	        angle -= 360.0f;																						// angle - 360
	    }
	    return Mathf.Clamp (angle, min, max);																	// return the min max amount for angle
	}
}
