using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour {


	public Text count1;
	public Text count2;
	int counter1;
	int counter2;
	// Use this for initialization
	void Start () {
		count1.text = "0";
		counter1 = 0;
		count2.text = "0";
		counter2 = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncrementFirstCount(){
		counter1 += 1;
		count1.text = counter1.ToString ();

	}
	public void IncrementSecondCount(){
		counter2 += 1;
		count2.text = counter2.ToString ();

	}
}
