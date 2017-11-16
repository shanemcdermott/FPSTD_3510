using UnityEngine;
using System.Collections;
using System;

public class EnemyMovement : MonoBehaviour, IRespondsToDeath
{
	private int framenum;

    public float speed;
	public float nodeChangeValue;

	private TileMap map;
	public GameObject target;
	private Vector3[] pathToTarget; //path to target

	public float playerTargetingDistance = 10f;

	GameObject player;
	GameObject tower;

	private int pathIndex;

    void Awake ()
    {
        GetComponent<HealthComponent>().RegisterDeathResponder(this);
		framenum = 0;
    }

    public void AssignTarget(GameObject targetObject)
    {
        target = targetObject;
    }


    //Move Towards Target
    void FixedUpdate ()
    {

		//choose between the tower and the player based on player targeting distance
		selectTarget ();

		if (map != null && target != null) {
			//Debug.Log ("Enemy Pos:" + this.transform.position);
			if (framenum % 20 == 0)
				pathToTarget = map.getVector3Path(this.transform.position, target.transform.position);
			pathIndex = 1;
		}

		if (pathToTarget == null) {

			if (target == null)
				Debug.Log ("null target!!");

			if (map == null)
				Debug.Log ("null map!!");
			
			if (map != null && target != null)
				Debug.Log ("No path to target");
			GetComponent<Animator>().SetBool("isWalking", false);
			return;

		}

		//we have a path to the target
		//TODO edge cases //TODO squared mag
		while (pathToTarget.Length >= pathIndex + 1 && (gameObject.transform.position - pathToTarget[pathIndex]).magnitude < map.getTileWidth () * nodeChangeValue)
			pathIndex++;
		Vector3 nextPosition = pathToTarget [pathIndex]; //enemy will move towards this location
        
		float zDiff = nextPosition.z - this.transform.position.z;
		float xDiff = nextPosition.x - this.transform.position.x;
        this.transform.localEulerAngles = new Vector3(0, (Mathf.Atan2(xDiff, zDiff) / Mathf.PI * 180), 0);
        this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
		GetComponent<Animator>().SetBool("isWalking", true);
 
    }

	private void selectTarget()
	{

		if (player == null)
			player = GameObject.FindGameObjectWithTag ("Player");
		if (tower == null)
			tower = GameObject.FindGameObjectWithTag ("Tower");
		
		if ((player.transform.position - this.transform.position).sqrMagnitude < playerTargetingDistance * playerTargetingDistance) {
			target = player;
			//Debug.Log ("Player selected as target");
		} else {
			target = tower;
			//Debug.Log ("Tower selected as target");
		}
		

	}

    public void OnDeath(DamageContext context)
    {
        enabled = false;
    }

	public void setTileMap(TileMap tm)
	{
		map = tm;
	}
}
