using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


public class FiringCommand : NetworkBehaviour {

    public GameObject leftBullet, rightBullet , RawBullet;
    Transform bulletSpawn;
    SubmarinePlayer playerControl;

    private void Start()
    {
        bulletSpawn = transform.FindChild("FirePos");
        GameObject g = GameObject.Find("Submarine Player Nic(Clone)");
        Debug.Log("Value of g in Firing Command is : " + g);
        playerControl = GetComponent<SubmarinePlayer>();
        Debug.Log("playerControl is : " + playerControl);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) // Only local players can fire
        {
            return;
        }
        if (CrossPlatformInputManager.GetButtonDown("Fire"))
        {
            Debug.Log("FIRE BUTTON PRESSED");
            CmdFire();
            Debug.Log("CmdFire() should have been executed");
        }
    }

    // The reason this script is not in submarine player is because this command needs to be accessible to all
    // Players when the server calls this method
    // This method will spawn the bullet seen in the network
    [Command] // This is used to indicate a server call    - Command is run on server when called by local player
    void CmdFire()  // Method will not be run on client! - Note that the bullet must allow server and client authority to work!
    {
        Debug.Log("CmdFire Called by "+this.name);
        //TODO: Implement 360 firing
        // Possible method of implementation
        // Set spawn point based on pointer of cursor

        GameObject instance = Instantiate(RawBullet, bulletSpawn.position, Quaternion.identity) as GameObject;
        
        Vector2 forwardForce = new Vector2(bulletSpawn.forward.z * 100, 0);
        // bulletspawn.forward will give (0,0,1) if facing right and (0,0,-1) when facing left. 
        // Thus some massaging has to be done to get the right direction.
         
        instance.GetComponent<Rigidbody2D>().AddForce(forwardForce);
        NetworkServer.Spawn(instance); 
    }

}
