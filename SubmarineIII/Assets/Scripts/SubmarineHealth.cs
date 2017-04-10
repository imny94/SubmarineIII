using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SubmarineHealth : NetworkBehaviour {

	[SyncVar] public float maxHealth = 100f;  // Synced to prevent cheating! Maybe? 
    [SyncVar (hook = "SetHealthBar")] public float currentHealth = 0f;  // The hook means that the method declared in the hook will be called everytime this variable is changed
    //public GameObject healthBar;
	Rigidbody2D myBody;
    Text endGameText;
    public RectTransform healthBar;
    //public Slider healthSlider;          // Reference to the UI's health bar.


    // Use this for initialization
    void Start () {
		//Debug.Log ("STARTED");
		currentHealth = maxHealth;
		//InvokeRepeating ("DecreaseHealth", 1f, 1f);

	}
    /*
	// Update is called once per frame
	void Update () {
		if (currentHealth == 0f) {
			Die ();
		}
	}
    */

	void OnCollisionEnter2D(Collision2D col){
        //GameObject hit = col.gameObject;
        //SubmarineHealth health = hit.GetComponent<SubmarineHealth>();
        //if(health != null){
        //	health.DecreaseHealth();
        //}

		//JS, do not call this always
        //DecreaseHealth(); // Decrease health of itself whenever it collides into anything
	}

    // Decrease Health by default value of 10
	public void DecreaseHealth(){
        // Damage can only be calculated by the server, preventing hacked client from cheating
        // Also, if player is dead, no need to run this code anymore
        //Debug.Log("Decrease Health Method is called! current health is : "+currentHealth);
        if (!isServer||currentHealth <= 0)
        {
            return;
        }
        Debug.Log("DecreaseHealth() Method called and is server!");
		currentHealth -= 10f; // Should have no need to make explicit call to set player health as the current health variable is already hooked to setHealthBar method
		//float calculatedHealth = currentHealth / maxHealth;
        //SetHealthBar (calculatedHealth);
        //healthSlider.value = currentHealth / maxHealth * 100; // Need to normalise health to between 0-100

        if(currentHealth <= 0f)
        {
            // Call a method on all instances of this object on all clients (RPC)
            RpcDied();

            Invoke("BackToLobby", 3f); // Return back to lobby since player has died. - Probably would want to include some logic here to prevent game from terminating from 1 player dying alone
            return;
        }
	}

    [ClientRpc]
    private void RpcDied()
    {
        // Find the Text object to display end game message in the scene
        endGameText = GameObject.FindObjectOfType<Text>();

        if (isLocalPlayer)
        {
            endGameText.text = "Game over";
        }else
        {
            endGameText.text = "You win!";
        }
    }

    private void BackToLobby()
    {
        // Go back to the lobby
        FindObjectOfType<NetworkLobbyManager>().ServerReturnToLobby();
    }

	private void SetHealthBar(float myHealth){

        //Debug.Log(gameObject.name + " current health : " + myHealth);
        //healthBar.transform.localScale = new Vector3 (myHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBar.sizeDelta = new Vector2(myHealth/maxHealth*5, healthBar.sizeDelta.y); // Need to normalise size of health bar to the width of the health bar in game

	}
    /*
	void Die(){
		Application.LoadLevel (Application.loadedLevel);
	}
    */
}
