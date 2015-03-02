using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public Transform[] SpawnPoints;

	private int lastSpawnPoint;
	private int beforeLastSpawnPoint;

	private int spawn;

	public void Respawn(PlayerController controller)
	{
		while (spawn == lastSpawnPoint || spawn == beforeLastSpawnPoint)
			spawn = Random.Range(0, 4);
		beforeLastSpawnPoint = lastSpawnPoint;
		lastSpawnPoint = spawn;
		controller.SpawnPlayer(SpawnPoints[spawn].transform.position);

	}
}
