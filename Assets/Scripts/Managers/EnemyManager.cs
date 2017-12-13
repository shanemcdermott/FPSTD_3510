using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public bool bShouldSpawn;
    public float spawnTime = 3f;
    public int waveSize = 10;

    private int totalSpawned = 0;
    private GameObject[] spawnedEnemies;

	private TileMap map;

    public void OnEnable()
    {
        totalSpawned = 0;
        spawnedEnemies = new GameObject[waveSize];
        bShouldSpawn = true;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    public void OnDisable()
    {
        bShouldSpawn = false;
        CancelInvoke();
        
    }

    void Spawn ()
    {
        if (!bShouldSpawn) return;
        
       
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        if (spawnPoints[spawnPointIndex] == null) return;

        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = enemyPrefabs[enemyIndex];
       
        spawnedEnemies[totalSpawned] = Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		spawnedEnemies [totalSpawned].GetComponent<EnemyMovement> ().setTileMap (map);
        totalSpawned++;
        if (totalSpawned >= waveSize)
            bShouldSpawn = false;
    }

    public int GetLivingCount()
    {
        int count = 0;
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy == null) continue;

            MonsterController mc = enemy.GetComponent<MonsterController>();
            if (mc == null || mc.health.IsDead()) continue;
            count++;
        }
        return count;
    }

    public int GetTotalSpawned()
    {
        return totalSpawned;
    }

	public void setTileMap(TileMap tm)
	{
		map = tm;
	}

    public void clearEnemies()
    {
        for (int i = 0; i < spawnedEnemies.Length; i++ )
        {
            if(spawnedEnemies[i] != null)
                spawnedEnemies[i].GetComponent<MonsterController>().OnDeath(new DamageContext());
        }
    }
}
