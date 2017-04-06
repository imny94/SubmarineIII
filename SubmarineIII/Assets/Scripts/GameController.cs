using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Game Controller for each player to hold common attributes
public class GameController : NetworkBehaviour
{
    public bool FACINGRIGHT;        // This can be accessed by all sub-classes to determine direction submarine is facing
    public float CURRENTHP;         

    // Update is called once per frame
    void Update () {
		
	}

    // Updated every Physics Step at fixed intervals - used for regular updates such as physics objects
    private void FixedUpdate()
    {

    }
}
