using UnityEngine;
using System.Collections;
using System;

public class EnemyMovement : MonoBehaviour, IRespondsToDeath
{
	private int framenum;

    public float speed;

	private TileMap map;
	public GameObject target;
	private Vector3[] pathToTarget;

	public float playerTargetingDistance;


	private int lastx;
	private int lastz;
	private int currentx;
	private int currentz;

	GameObject player;
	GameObject tower;

    void Awake ()
    {
        GetComponent<HealthComponent>().RegisterDeathResponder(this);
		framenum = 0;

		lastx = 0;
		lastz = 0;
		currentx = 0;
		currentz = 0;


    }

    public void AssignTarget(GameObject targetObject)
    {
        target = targetObject;
    }


    void FixedUpdate ()
	{
		//increment framenum
		framenum++;

		//this might change if the enemy changes or if the size of the tiles are changed
		Vector3 realpos = this.transform.position + new Vector3(0.5f, 0, 0.5f);

		//check to see if the enemies position has changed
		bool poschanged = false;
		if (map != null)
			map.nodeAtLocation(realpos, out currentx, out currentz);
		if (lastx != currentx || lastz != currentz)
		{
			poschanged = true;
			lastx = currentx;
			lastz = currentz;
		}

		//select target if target is null or every 10 frames (10 was arbitrarily chosen)
		if (framenum % 10 == 0 || target == null)
			selectTarget ();

		//calculate the path when things arent null and node position has changed (or there is no path)
		if (map != null && target != null)
			if (poschanged || pathToTarget == null)
				pathToTarget = map.getVector3Path(realpos, target.transform.position);
		

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


		//if the path is less than 3, we want to move straight there
		Vector3 nextPosition = target.transform.position;

		//if the path is 3 or greater, move to the second path location (path is recalculated every time location changes)
		if (pathToTarget.Length > 2)
			nextPosition = pathToTarget[1];
        
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

	public void OnDrawGizmos()
	{
		if (pathToTarget != null) {
			for (int i = 1; i < pathToTarget.Length; i++) {
				Gizmos.color = new Color (1f, 1f, 0f);
				Gizmos.DrawCube (pathToTarget[i], new Vector3(0.5f, 0.5f, 0.5f));
				Gizmos.color = new Color (0.7f, 0.7f, 0.7f);
			}
		}

//		Gizmos.color = new Color (1f, 1f, 0f);
//		Gizmos.DrawCube (this.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
		Gizmos.color = new Color (1f, 0f, 0f);

		int x, z;
		map.nodeAtLocation(this.transform.position + new Vector3(0.5f, 0, 0.5f), out x, out z);
		Gizmos.DrawCube (new Vector3(x, 0, z), new Vector3(0.5f, 0.5f, 0.5f));


	}

	public int getPathLen()
	{
		if (pathToTarget == null)
			return 0;
		return pathToTarget.Length;
	}
}
