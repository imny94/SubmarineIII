using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {

	public Vector2 speed;
	Rigidbody2D rb;                 // Reference to the bullet
    float bulletLifeTime = 5f;      // How long can the bullet exist in the game
    bool isDamaging = true;        // Does the bullet damage players?

    float age;                      // How long the shell has been alive for

	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		//rb.velocity = speed;
	}

    [ServerCallback]        // Only runs this update method on the server, the server is going to keep track of the bullet
	void FixedUpdate () 
	{
		//rb.velocity = speed;
        age += Time.deltaTime;
        if (age > bulletLifeTime)
        {
            NetworkServer.Destroy(gameObject); // Destroys bullet every where if bullet has been around for too long
        }
	}

    // When the shell hits something
	void OnCollisionEnter2D(Collision2D col)
	{
        NetworkServer.Destroy(gameObject); // Destroys bullet every where as bullet has already collided
        Debug.Log("Bullet has hit something.... might or might not be server");
        if (!isServer)
        // leave if not the server, only the server can deal damage to the players
        {
            return;
        }
        if (!isDamaging)    // Leave if bullet is not damaging
        {
            return;
        }
        //Debug.Log("Bullet has hit something, and action is managed by server");
        GameObject hit = col.gameObject;        // Reference to object that bullet collides with
        SubmarineHealth health;                 // Player health
        if (hit.tag == "Player")
        {
            Debug.Log("Hit object is Player");
            health = hit.GetComponent<SubmarineHealth>();
            Debug.Log("Health of hit object is : " + health);
            if (health != null)
            {
                health.DecreaseHealth();
            }else
            {
                Debug.Log("HEALTH IS NULL, WHY");
            }
        }else if(hit.tag == "monster")//TODO: Unimplemented logic for monster
        {
            // Do something
        }
        else
        {
            Debug.Log("hit object tag miss.");
            Debug.Log("hit is : " + hit);
            // Probably do nothing...
            // For now just carry out indiscriminate damage and assume target is a submarine
            health = hit.GetComponent<SubmarineHealth>();
            if (health != null)
            {
                health.DecreaseHealth();
            }
        }
	}
}
