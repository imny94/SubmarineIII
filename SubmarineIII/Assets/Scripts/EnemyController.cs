using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyController : NetworkBehaviour {

	public LayerMask enemyMask;
	public float attack_speed = 0.5f;
	public float evade_speed  = 0.5f;
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

//    [ServerCallback]         // Only runs this update method on the server, the server is going to keep track of the blob to ensure location of blob is consistent throughout clients
//    void FixedUpdate()
//	{		
//			
//
//	}

	public void idle_mode(){
		
		Vector2 myVel = myBody.velocity;	
		myVel.x = Random.Range (-30.0f, 30.0f);
		myVel.y = Random.Range (-10.0f, 18.0f);
		myBody.velocity = myVel*0.1f;
	}

	[ServerCallback]  
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Submarine")) {
			Debug.Log ("Submarine detected");
			myBody.transform.position = Vector2.MoveTowards (myBody.transform.position, other.gameObject.transform.position, attack_speed);

		} else if (other.gameObject.CompareTag ("Missile")) {
			Debug.Log ("Dodging Missile");
			myBody.transform.position = Vector2.MoveTowards (myBody.transform.position, -other.gameObject.transform.position, evade_speed);
		}
	}



}
