using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyController : NetworkBehaviour {

	public LayerMask enemyMask;
	public float speed = 2.0f;
	Rigidbody2D myBody;
	Transform myTrans;
	float myWidth;
	Vector3 spawn_position;
	SubmarinePlayer sp;         // Reference to Submarine Player
	bool aggressive = false;    // Flag to decide if enemy becomes aggressive
	GameObject[] submarines;    
	GameObject[] missiles;




	void Start ()
	{
		spawn_position = this.transform.position;
		myTrans = this.transform;
		myBody = this.GetComponent<Rigidbody2D> ();
		myWidth = this.GetComponent<SpriteRenderer> ().bounds.extents.x;

		submarines = GameObject.FindGameObjectsWithTag ("Submarine");
		missiles = GameObject.FindGameObjectsWithTag ("Missile");



		InvokeRepeating ("idle_mode", 0.0f, Random.Range (3.0f, 7.0f)); // Constantly make blobs carry out random motion when they are first spawned


	}

    [ServerCallback]         // Only runs this update method on the server, the server is going to keep track of the blob to ensure location of blob is consistent throughout clients
    void FixedUpdate()
	{			
		foreach (GameObject missile in missiles) {
			if (Vector2.Distance (missile.transform.position, myBody.transform.position) < 10f) {
				aggressive = true;
				myBody.transform.position = Vector2.MoveTowards (myBody.transform.position, -missile.transform.position, 0.1f);
			}
		}

		foreach (GameObject submarine in submarines) { 
			if (Vector2.Distance (submarine.transform.position, myBody.transform.position) < 10f) {
				aggressive = true;
				myBody.transform.position = Vector2.MoveTowards (myBody.transform.position, submarine.transform.position, 0.1f);
			}
		}

			

		//check if there is ground
//		Vector2 lineCastPos = myTrans.position - myTrans.right * myWidth;
//		Debug.DrawLine (lineCastPos, lineCastPos + Vector2.down);
//		bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
//
//		//if no ground, turn around
//		if (true) 
//		{
//			Vector3 currRotation = myTrans.eulerAngles;
//			currRotation.y += 180;
//			myTrans.eulerAngles = currRotation;
//
//		}
//
//
//		//always move forward
//

//		myVel.x = -myTrans.right.x * speed;
//



	
		//find submarine player
		//GameObject player = GameObject.Find ("Submarine Player Nic");

			
//		//check if stealthmode is on
//		if (sp.stealthmode) { //if stealth mode is on
//			Debug.Log("stealth mode");
//			aggressive_mode (sp);
//		}	


	}



	void OnCollisionEnter2D(Collision2D col)
	{			
		Debug.Log ("THAT HAPPENED");

        if (!isServer)
        // leave if not the server, only the server can control effects to the blobs
        {
            return;
        }
		

		GameObject hit = col.gameObject;
		EnemyBlobHealth health = hit.GetComponent<EnemyBlobHealth>();

		if (hit.tag == "Missile") {

			health = this.GetComponent<EnemyBlobHealth> ();
			health.DecreaseHealth ();

		}

		if(health != null && hit.tag == "Missile"){ // ??? Health is already decreased above?
			Debug.Log ("Missile hit");
			health.DecreaseHealth();
		}
	}

	public void aggressive_mode(){
//		Debug.Log ("Submarine near blob");
//		Vector2 myVel = myBody.velocity; 
//		myBody.velocity = myVel * (speed * 0.5f);
//
//	


	}

	public void idle_mode(){


		Vector2 myVel = myBody.velocity;	
		myVel.x = Random.Range (-30.0f, 30.0f);
		myVel.y = Random.Range (-10.0f, 18.0f);
		myBody.velocity = myVel*0.1f;
	}



}
