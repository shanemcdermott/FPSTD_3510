using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private GameObject player;
	private GameObject[] enemies = new GameObject[10];





	void Start ()
	{
		
	}

	void Update ()
	{

		for (int i = 0; i < 10; i++) {
			if (enemies [i] == null) {
				enemies [i] = Instantiate (enemyPrefab) as GameObject;
				enemies [i].transform.position = new Vector3 (Random.Range (-50f, 50f), 1, Random.Range (-50f, 50f));
				//float angle = Random.Range (0, 360);
				//enemies [i].transform.Rotate (0, angle, 0);

				//enemies [i].GetComponent<FollowingEnemy> ().target = player;
			}
		}
	}
}
