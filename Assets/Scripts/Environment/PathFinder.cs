using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {


	public TileMap map;

	private GameObject startIndicator;
	private GameObject targetIndicator;

	public int startX;
	public int startZ;
	public int targetX;
	public int targetZ;

	public void setMap(TileMap tm)
	{
		map = tm;


		//temporary values, check if you can get from one corner to the other
		startX = 0;
		startZ = 0;
		targetX = map.getWidth() - 1;
		targetZ = map.getLength() - 1;

		startIndicator = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		//startIndicator.GetComponent<Material> ().SetColor ("red", new Color(1f, 0f, 0f));
		startIndicator.transform.position = map.getTileAt(startX, startZ).transform.position + new Vector3(0f, 2f, 0f);
		targetIndicator = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		//startIndicator.GetComponent<Material> ().SetColor ("red", new Color(1f, 0f, 0f));
		targetIndicator.transform.position = map.getTileAt(targetX, targetZ).transform.position + new Vector3(0f, 2f, 0f);

	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
