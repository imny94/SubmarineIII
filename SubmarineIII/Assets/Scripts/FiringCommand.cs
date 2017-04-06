using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


public class FiringCommand : NetworkBehaviour {

    /*
	bool facingright;
	public GameObject leftBullet, rightBullet;
	Transform bulletSpawn;
	void Start(){
		bulletSpawn = transform.FindChild ("FirePos");
		//this.facingright = false;


	}
	void FixedUpdate(){
        this.facingright = GameObject.Find("Player").GetComponent<SubmarinePlayer>().facingright;
        Vector2 moveVec = new Vector2 (CrossPlatformInputManager.GetAxis ("Horizontal"), CrossPlatformInputManager.GetAxis ("Vertical"));
		if (moveVec.x < 0 && facingright) {        // If you are moving left and facing right
			flip ();
		} else if (moveVec.x > 0 && !facingright) {  // If you are moving right and facing left
			flip ();
		}
	}
	public void flip(){
		this.facingright = !this.facingright;
	}

    void Update () {
        if (!isLocalPlayer) // Leave if not local player, only local player can fire missle
        {
            return;
        }
		if (CrossPlatformInputManager.GetButtonDown ("Fire")) {
			CmdFire ();
		}
	}

    [Command] // This is used to indicate a server call
	void CmdFire()
	{
        this.facingright = GameObject.Find("Player").GetComponent<SubmarinePlayer>().facingright;
        if (this.facingright) {                  // If facing Right
			Instantiate (rightBullet, bulletSpawn.position, Quaternion.identity);
			NetworkServer.Spawn (rightBullet);  // Spawn right bullet
		}
		if (!this.facingright) {                // If facing left
			Instantiate (leftBullet, bulletSpawn.position, Quaternion.identity);
			NetworkServer.Spawn (leftBullet);   // Spawn left Bullet

		}
	}
    */

    /*
bool facingright;
public GameObject leftBullet, rightBullet;
Transform bulletSpawn;

void Start()
{
    this.facingright = false;
    bulletSpawn = transform.FindChild("FirePos");
}
void FixedUpdate()
{
    Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
    if (moveVec.x < 0 && facingright)
    {        // If you are moving left and facing right
        flip();
    }
    else if (moveVec.x > 0 && !facingright)
    {  // If you are moving right and facing left
        flip();
    }
}
private void flip()
{
    this.facingright = !this.facingright;
}

// Reset allows us to run slow code like "Find" in the editor without affecting game performance at run time
// This is run at compile time, not run time
//void Reset()
//{
//    bulletSpawn = transform.FindChild("FirePos");
//}

// Update is called once per frame
void Update()
{

    if (CrossPlatformInputManager.GetButtonDown("Fire"))
    {
        CmdFire();
    }
}

[Command] // This is used to indicate a server call
void CmdFire()
{
    if (facingright)
    {                  // If facing Right
        GameObject instance = Instantiate(rightBullet, bulletSpawn.position, Quaternion.identity);
        NetworkServer.Spawn(instance);  // Spawn right bullet
    }
    if (!facingright)
    {                // If facing left
        GameObject instance = Instantiate(leftBullet, bulletSpawn.position, Quaternion.identity);
        NetworkServer.Spawn(instance);   // Spawn left Bullet

    }
}

*/

    public GameObject leftBullet, rightBullet , RawBullet;
    Transform bulletSpawn;
    SubmarinePlayer playerControl;
    bool facingright;

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
        // Possible method of implemenation
        // Set spawn point based on pointer of cursor

        GameObject instance = Instantiate(RawBullet, bulletSpawn.position, Quaternion.identity) as GameObject;
        instance.GetComponent<Rigidbody2D>().AddForce(bulletSpawn.forward * 10);
        NetworkServer.Spawn(instance); 


        // Bottom can be ignored after implementing 360 firing!
        //facingright = playerControl.facingright;
        //if (facingright)
        //{                  // If facing Right
        //    Debug.Log("Player is facing right ");
        //    GameObject instance = Instantiate(rightBullet, bulletSpawn.position, Quaternion.identity) as GameObject;
        //    NetworkServer.Spawn(instance);  // Spawn right bullet
        //}
        //if (!facingright)
        //{                // If facing left
        //    Debug.Log("Player is facing left ");
        //    GameObject instance = Instantiate(leftBullet, bulletSpawn.position, Quaternion.identity) as GameObject;
        //    NetworkServer.Spawn(instance);   // Spawn left Bullet
        //}
    }

}
