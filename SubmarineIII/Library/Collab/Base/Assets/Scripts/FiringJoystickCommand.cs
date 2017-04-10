using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


public class FiringJoystickCommand : NetworkBehaviour {

	public GameObject bullet;
	Vector3 newPosition;
	private bool cooledDown = true;

	void Update(){

		if (!isLocalPlayer) { // Only local Players can fire
			return;
		}

		//using absolute values to make sure the bullet never spawns too near to itself
		float xChecker = Mathf.Abs (CrossPlatformInputManager.GetAxis ("HorizontalFire1"));
		float yChecker = Mathf.Abs (CrossPlatformInputManager.GetAxis ("VerticalFire1"));
		if ((xChecker > 0.5f || yChecker > 0.5f) && cooledDown) {
			//getting the coordinates of the submarine player in game
			float subx = gameObject.transform.localPosition.x;
			float suby = gameObject.transform.localPosition.y;

			//creating a new spawn position for the bullet according to the joystick position and fire command is called
			newPosition = new Vector3 (subx + (CrossPlatformInputManager.GetAxis ("HorizontalFire1")* 2.5f) , suby + (CrossPlatformInputManager.GetAxis ("VerticalFire1") * 2.5f), 0.0f);
			//StartCoroutine (WaitMethod ()); // this doesn't work, as it just makes the bullet spawn later
			CmdFire();
			cooledDown = false;
			StartCoroutine(startCoolDown ());
		
		}
	}

	IEnumerator startCoolDown(){
		Debug.Log("Before Waiting 2 seconds");
		yield return new WaitForSeconds(0.5f);
		Debug.Log("After Waiting 2 Seconds");
		cooledDown = true;
	}

	IEnumerator WaitMethod() {
		Debug.Log("Before Waiting 2 seconds");
		yield return new WaitForSeconds(2);
		Debug.Log("After Waiting 2 Seconds");
	}

	// This method will spawn the bullet seen in the network
	[Command]// This method will spawn the bullet seen in the network
	void CmdFire(){// Method will not be run on client! - Note that the bullet must allow server and client authority to work!
		Debug.Log ("Cmd Fire Initiated");
		GameObject instance = Instantiate (bullet, newPosition, Quaternion.identity);
		Debug.Log ("Bullet instance is : " + instance);
		Vector2 forwardForce = new Vector2((newPosition.x-gameObject.transform.localPosition.x) * 100, (newPosition.y-gameObject.transform.localPosition.y) * 100);
		Debug.Log (forwardForce);
		instance.GetComponent<Rigidbody2D>().AddForce(forwardForce);
		NetworkServer.Spawn (bullet);
		Debug.Log ("Bullet has been Spawned on Server");
	}
				
}
