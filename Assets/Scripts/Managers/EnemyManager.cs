using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public bool bShouldSpawn;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;


    public void OnEnable()
    {
        bShouldSpawn = true;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    public void OnDisable()
    {
        bShouldSpawn = false;
    }

    void Spawn ()
    {
        if(!bShouldSpawn)
        {
            return;
        }
       
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = enemyPrefabs[enemyIndex];
        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    
}
