using UnityEngine;
using System.Collections;

/* ---Unity Script---
// Collision Foot 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Checks for player foot collisions and creates sound/particle based on trigger
// Instruction: Assign script to player 'Foot' - both left and right
*/

public class CollisionFoot : MonoBehaviour 
{
	public float baseFootAudioVolume 			= 1.0f;											// audio volume
	public float soundEffectPitchRandomness  	= 0.05f;										// pitch level rnd
	
	void OnTriggerEnter ( Collider other ) 									//collision enters
	{			 
		CollisionParticleEffect collisionParticleEffect = other.GetComponent<CollisionParticleEffect>();
		 																				// if theres an effect
		if(collisionParticleEffect)
		{
			Instantiate ( collisionParticleEffect.effect, transform.position, transform.rotation );					// create the particle 
		}
		
		CollisionSoundEffect collisionSoundEffect = other.GetComponent<CollisionSoundEffect>();				// get sound effect
	
		if ( collisionSoundEffect ) 																				// if theres a sound
		{
			GetComponent<AudioSource>().clip	 = collisionSoundEffect.audioClip;															// set clip to this clip
			GetComponent<AudioSource>().volume = collisionSoundEffect.volumeModifier * baseFootAudioVolume;								// set volume
			GetComponent<AudioSource>().pitch  = Random.Range ( 1.0f - soundEffectPitchRandomness, 1.0f + soundEffectPitchRandomness );		// set pitch
			GetComponent<AudioSource>().Play ();																							// play audio file
		}
	}
	void Reset () 
	{																								// reset function
		GetComponent<Rigidbody>().isKinematic = true;																				// enable isKinematic
		GetComponent<Collider>().isTrigger = true;																					// enable isTrigger
	}
}
