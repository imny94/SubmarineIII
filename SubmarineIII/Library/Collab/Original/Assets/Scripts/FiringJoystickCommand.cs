using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


public class FiringJoystickCommand : NetworkBehaviour {

	public GameObject bullet;
	Vector3 newPosition;

    // Update is called once per frame
    void Update(){

        if (!isLocalPlayer) // Only Local Players can fire
        {
            return;
        }

		//using absolute values to make sure the bullet never spawns too near to itself
		float xChecker = Mathf.Abs (CrossPlatformInputManager.GetAxis ("HorizontalFire1"));
		float yChecker = Mathf.Abs (CrossPlatformInputManager.GetAxis ("VerticalFire1"));
		if (xChecker > 0.5f || yChecker > 0.5f) {
			//getting the coordinates of the submarine player in game
			float subx = gameObject.transform.localPosition.x;
			float suby = gameObject.transform.localPosition.y;

			//creating a new spawn position for the bullet according to the joystick position and fire command is called
			newPosition = new Vector3 (subx + (CrossPlatformInputManager.GetAxis ("HorizontalFire1")* 2.5f) , suby + (CrossPlatformInputManager.GetAxis ("VerticalFire1") * 2.5f), 0.0f);
			CmdFire ();
		
		}
	}
    
    // The reason this script is not in submarine player is because this command needs to be accessible to all
    // Players when the server calls this method
    // This method will spawn the bullet seen in the network
    [Command] // This is used to indicate a server call    - Command is run on server when called by local player
    void CmdFire()  // Method will not be run on client! - Note that the bullet must allow server and client authority to work!
    { 
        GameObject instance = Instantiate (bullet, newPosition, Quaternion.identity);
		//Debug.Log("new position is : " + newPosition);
		Vector2 forwardForce = new Vector2((newPosition.x-gameObject.transform.localPosition.x) * 100, (newPosition.y-gameObject.transform.localPosition.y) * 100);
		instance.GetComponent<Rigidbody2D>().AddForce(forwardForce);
		NetworkServer.Spawn (bullet);
	}
				
}
