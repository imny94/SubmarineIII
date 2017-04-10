using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlobSpawn : NetworkBehaviour {
	public GameObject blobPrefab;
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
            GameObject enemy = (GameObject)Instantiate(blobPrefab, spawnPosition, Quaternion.identity);
            NetworkServer.Spawn(enemy);
        }
    }
 //   public override void OnStartServer()
	//{
	//	for (int i = 0; i < numberOfEnemies; i++) {
	//		Vector3 spawnPosition = new Vector3 (Random.Range (-30.0f, 30.0f), Random.Range (-10.0f, 18.0f), 0.0f);
	//		GameObject enemy = (GameObject)Instantiate (blobPrefab, spawnPosition, Quaternion.identity);
	//		NetworkServer.Spawn (enemy);
	//	}
	//}

}
