using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles a monster's sub processes.
public class MonsterController : MonoBehaviour, IRespondsToDeath
{

    public Team team;
    public HealthComponent health;
    public EnemyMovement movement;
    public EnemyAttack attack;

    
    private GameObject target;
	private TileMap tileMap;
	private Vector3[] pathToTarget;
	private int pathIndex;

	private bool playerIsTarget;

	GameObject player;
	GameObject tower;

	Vector3 offset = new Vector3(0.5f, 0, 0.5f);

	public float playerTargetingDistance = 20f;
	private int player_last_x = 0;
	private int player_last_z = 0;
	private int player_curr_x = 0;
	private int player_curr_z = 0;

    void Awake()
    {
        team = GetComponent<Team>();
        health = GetComponent<EnemyHealth>();

        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();

        health.RegisterDeathResponder(this);
        health.RegisterDeathResponder(movement);
        health.RegisterDeathResponder(attack);

		tileMap = null;
		player = GameObject.FindGameObjectWithTag("Player");
		tower = GameObject.FindGameObjectWithTag("Tower");
		target = null;
		pathToTarget = null;
		pathIndex = 0;
		playerIsTarget = false;
    }

    private void Start()
    {
    }

    public void FindNextTarget()
    {
		if (playerIsTargetable())
		{
			if (!playerIsTarget)
			{
				AssignTarget(player);
				calculatePathToTarget();
				playerIsTarget = true;
				tileMap.nodeAtLocation(player.transform.position, out player_last_x, out player_last_z);
			}
			else
			{
				//if the player moves and is being targeted, recalculate the path
				tileMap.nodeAtLocation(player.transform.position, out player_curr_x, out player_curr_z);
				if (player_last_x != player_curr_x || player_last_z != player_curr_z)
					calculatePathToTarget();
			}
		}
		else
		{
			if (playerIsTarget)
			{
				AssignTarget(tower);
				calculatePathToTarget();
				playerIsTarget = false;
			}
			else
			{
				if (target == null)
				{
					AssignTarget(tower);
					calculatePathToTarget();
				}
			}
		}
    }

    public void AssignTarget(GameObject nextTarget)
    {
		this.target = nextTarget;
        attack.AssignTarget(nextTarget);
    }

    // Update is called once per frame
    void Update ()
    {
		if(!health.IsDead())
        {

			FindNextTarget();


			Vector3 realpos = this.transform.position + offset;
			int x, z;
			tileMap.nodeAtLocation(realpos, out x, out z);

			int pathx, pathz;
			tileMap.nodeAtLocation(pathToTarget[pathIndex], out pathx, out pathz);
			if (pathx == x && pathz == z)
			{

				if (pathIndex + 1 < pathToTarget.Length)
				{
					pathIndex++;
					movement.setTargetPosition(pathToTarget[pathIndex]);
				}
				else
				{
					movement.setTargetPosition(target.transform.position);
				}
			}

        }
	}
    public void OnDeath(DamageContext context)
    {
        
        GetComponent<Rigidbody>().isKinematic = true;
       // isSinking = true;
        //ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }


	private void calculatePathToTarget()
	{
		if (target == null)
			return;
		Vector3 realpos = this.transform.position + offset;
		pathToTarget = tileMap.getVector3Path(realpos, target.transform.position);
		pathToTarget = tileMap.SmoothPath(pathToTarget);
		pathIndex = 0;


	}

	private bool playerIsTargetable()
	{
		if (player == null)
			return false;

		if ((player.transform.position - this.transform.position).sqrMagnitude < playerTargetingDistance * playerTargetingDistance) 
		{
			return true;
		}

		return false;
	}

	public void setTileMap(TileMap tm)
	{
		tileMap = tm;
	}
		
	public float getPathLength()
	{
		float total = 0f;

		total += (this.transform.position - pathToTarget[pathIndex]).magnitude;

		for (int i = pathIndex; i + 1 < pathToTarget.Length; i++)
		{
			total += (pathToTarget[i] - pathToTarget[i+1]).magnitude;
		}

		return total;
	}

	public void OnDrawGizmos()
	{
		if (pathToTarget != null) {
			for (int i = 1; i < pathToTarget.Length; i++) {
				Gizmos.color = new Color (1f, 0f, 0f);
				Gizmos.DrawCube (pathToTarget[i], new Vector3(2, 2, 2));
			}
		}

	}

}
