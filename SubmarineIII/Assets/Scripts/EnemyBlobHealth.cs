using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyBlobHealth : NetworkBehaviour {
	[SyncVar]public float maxHealth; // Synced to prevent cheating! Maybe? 
    [SyncVar (hook = "SetHealthBar")]public float currentHealth = 0f;  // The hook means that the method declared in the hook will be called everytime this variable is changed
    //public GameObject healthBar;
	Rigidbody2D myBody;
    public RectTransform healthBar;

    // Use this for initialization
    void Start () {
		currentHealth = maxHealth;

	}
	
	// Update is called once per frame
	//void Update () {              // This is not very computationally efficient
	//	if (currentHealth <= 0f) {
	//		Die ();
	//	}
	//}

    [ClientRpc]
	void RpcDie(){
		NetworkServer.Destroy (gameObject); // Destroys all instances of the blob everywhere
	}

	public void DecreaseHealth(){
        // Damage can only be calculated by the server, preventing hacked client from cheating
        // Also, if player is dead, no need to run this code anymore
        Debug.Log("Decrease Health Method on Blob is called! current health is : " + currentHealth);
        if (!isServer || currentHealth <= 0)
        {
            return;
        }
        currentHealth -= 10f; // No need to make explicit call to set player health as the current health variable is already hooked to setHealthBar method
        //float calculatedHealth = currentHealth / maxHealth;
        //SetHealthBar (calculatedHealth);

        if (currentHealth <= 0f)
        {
            // Call a method on all instances of this object on all clients (RPC)
            RpcDie();

            return;
        }

    }
	private void SetHealthBar(float myHealth){

        Debug.Log("Blob is hit! health of blob is : "+myHealth);
		healthBar.sizeDelta = new Vector2 (myHealth/maxHealth*5, healthBar.sizeDelta.y);

	}
}
