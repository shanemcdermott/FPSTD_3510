using UnityEngine;
using System.Collections;
using System;

public class EnemyMovement : MonoBehaviour, IRespondsToDeath
{
    
    public float speed	;
	public float nodeChangeValue;

	private TileMap map;
	public GameObject target;
	private Vector3[] pathToTarget; //path to target
	//private Vector3 nextPosition; //position of the next node to travel to

    void Awake ()
    {
        GetComponent<HealthComponent>().RegisterDeathResponder(this);
    }

    public void AssignTarget(GameObject targetObject)
    {
        target = targetObject;
    }


    //Move Towards Target
    void Update ()
    {

		if (map != null && target != null) {
			Debug.Log ("Enemy Pos:" + this.transform.position);
			pathToTarget = map.getVector3Path(this.transform.position, target.transform.position);
		}

		if (pathToTarget == null) {

			if (target == null)
				Debug.Log ("null target!!");

			if (map == null)
				Debug.Log ("null map!!");
			
			if (map != null && target != null)
				Debug.Log ("No path to target");
			return;

		}

		//we have a path to the target
		//TODO edge cases
		int pathIndex = 1;
		if ((gameObject.transform.position - pathToTarget[pathIndex]).magnitude < map.getTileWidth () * nodeChangeValue)
			pathIndex++;
		Vector3 nextPosition = pathToTarget [pathIndex]; //enemy will move towards this location

		//if past the threshold, target next node in path
		//TODO: use square mag
//		while (pathIndex + 1 < pathToTarget.Length && (gameObject.transform.position - nextPosition).magnitude < map.getTileWidth () * nodeChangeValue)
//			pathIndex++;
		
		nextPosition = pathToTarget [pathIndex];
        
		float zDiff = nextPosition.z - this.transform.position.z;
		float xDiff = nextPosition.x - this.transform.position.x;
        this.transform.localEulerAngles = new Vector3(0, (Mathf.Atan2(xDiff, zDiff) / Mathf.PI * 180), 0);
        this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
		GetComponent<Animator>().SetBool("isWalking", true);
 
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
