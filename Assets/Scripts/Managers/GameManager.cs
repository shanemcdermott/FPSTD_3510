using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    
    private TileMap tileMap;
    private int level = 1;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        tileMap = GetComponent<TileMap>();

        InitGame();
    }

	public TileMap getTileMap()
	{
		return tileMap;
	}

    private void InitGame()
    {
        //Prepare level etc.
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
