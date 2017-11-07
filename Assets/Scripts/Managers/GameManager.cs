using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour, IStateController<GameState>
{

    public static GameManager instance = null;


    public GameState state;
    public HUDManager hud;
    public MenuManager menuManager;
    public TileMap tileMap;

    //The current Wave Number
    public int currentWave = 0;


    //TODO- Implement
    public float difficultyScale = 1;

    public int numWavesInLevel = 10;
    public bool endlessMode = false;

    private int level = 1;
    private EnemyManager enemyManager;
    private GameObject player;

    private void Start()
    {   
    }

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
        player = GameObject.FindGameObjectWithTag("Player");
        enemyManager = GetComponent<EnemyManager>();
        //hud = player.GetComponentInChildren<HUDManager>();
        //menuManager = player.GetComponentInChildren<MenuManager>();
        InitGame();
    }


    private void InitGame()
    {
        currentWave = 0;
        state.Enter();
    }

    // Update is called once per frame
    void Update ()
    {
		enemyManager.setTileMap (tileMap); //this is temporary; there is probably a better way.
	}

    public int GetNumWavesRemaining()
    {
        return numWavesInLevel - currentWave;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public EnemyManager GetEnemyManager()
    {
        return enemyManager;
    }

    public TileMap GetTileMap()
    {
        return tileMap;
    }

    public void SetState(GameState state)
    {
        if(this.state != null)
            this.state.Exit();

        this.state = state;
        if(this.state != null)
            this.state.Enter();
    }

    public GameState GetState()
    {
        return state;
    }
}
