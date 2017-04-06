using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public LayerMask enemyMask;
	public float speed;
	Rigidbody2D myBody;
	Transform myTrans;
	float myWidth;


	void Start ()
	{
		myTrans = this.transform;
		myBody = this.GetComponent<Rigidbody2D> ();
		myWidth = this.GetComponent<SpriteRenderer> ().bounds.extents.x;

	}
	void FixedUpdate()
	{	

		//check if there is ground
		Vector2 lineCastPos = myTrans.position - myTrans.right * myWidth;
		Debug.DrawLine (lineCastPos, lineCastPos + Vector2.down);
		bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);

		//if no ground, turn around
		if (!isGrounded) 
		{
			Vector3 currRotation = myTrans.eulerAngles;
			currRotation.y += 180;
			myTrans.eulerAngles = currRotation;

		}

		//always move forward

		Vector2 myVel = myBody.velocity;
		myVel.x = -myTrans.right.x * speed;
		myBody.velocity = myVel;


	}
	void OnCollisionEnter2D(Collision2D col)
	{
		GameObject hit = col.gameObject;
		SubmarineHealth health = hit.GetComponent<SubmarineHealth>();
		if(health != null){
			health.DecreaseHealth();
		}
	}


}
