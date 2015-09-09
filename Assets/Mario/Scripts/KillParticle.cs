using UnityEngine;
using System.Collections;

/*
// Kill Particle 
// Walker Boys (www.walkerboystudio.com)
// November 28, 2011
// Description: Deletes this gameObject if called (waits a second)
// Instruction: Assign script to particle that will be destroyed after 1 second
*/

public class KillParticle : MonoBehaviour 
{
	void start()
	{
		Destroy(gameObject, 1.0f);
	}
}
