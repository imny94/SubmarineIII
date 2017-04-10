using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlobSpawn : NetworkBehaviour {
	public GameObject blob_level1;
	public GameObject blob_level2;
	public GameObject motherBlob;
	public int numberOfEnemies;

    private void Start()
    {	
        if (!isServer)
        {
            Destroy(this);  // Spawning of the blobs can only be done by the server
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-30.0f, 30.0f), Random.Range(-10.0f, 18.0f), 0.0f);    // Create a random spawn location
			GameObject enemy = (GameObject)Instantiate(blob_level1, spawnPosition, Quaternion.identity);
            NetworkServer.Spawn(enemy);
        }


    }

	[ServerCallback]
	void Update(){
		if (Time.realtimeSinceStartup==30.0f) {
			//if 30s of gameplay is done
			for (int i = 0; i < numberOfEnemies; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-30.0f, 30.0f), Random.Range (-10.0f, 18.0f), 0.0f);    // Create a random spawn location
				GameObject enemy = (GameObject)Instantiate (blob_level2, spawnPosition, Quaternion.identity);
				NetworkServer.Spawn (enemy);
			}
		} else if (Time.realtimeSinceStartup==60f) {
			Vector3 spawnPosition = new Vector3 (Random.Range (-30.0f, 30.0f), Random.Range (-10.0f, 18.0f), 0.0f);    // Create a random spawn location
			GameObject enemy = (GameObject)Instantiate (motherBlob, spawnPosition, Quaternion.identity);
			NetworkServer.Spawn (enemy);
		}
	}

}
