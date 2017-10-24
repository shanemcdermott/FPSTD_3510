using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour, IStateController<GameState>
{

    public static GameManager instance = null;


    public GameState state;
    public HUDManager HUD;
    public MenuManager MenuManager;

    private TileMap tileMap;
    private int level = 1;

    private EnemyManager enemyManager;
    private GameObject player;

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
        enemyManager = GetComponent<EnemyManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        InitGame();
    }


    private void InitGame()
    {
        if (state != null)
            state.Enter();
    }

    // Update is called once per frame
    void Update ()
    {
		
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
        this.state.Enter();
    }

    public GameState GetState()
    {
        return state;
    }
}
