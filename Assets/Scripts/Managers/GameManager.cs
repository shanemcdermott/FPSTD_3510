using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    Build,
    Defend,
    Complete
}

public class GameManager : MonoBehaviour, IStateController<GameState>
{

    public static GameManager instance = null;

    public GamePhase phase;
    private TileMap tileMap;
    private int level = 1;
    
    private GameState state;
    private EnemyManager enemyManager;

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

        InitGame();
    }

    private void InitGame()
    {
        //Prepare level etc.
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public TileMap GetTileMap()
    {
        return tileMap;
    }

    public void SetState(GameState state)
    {
        this.state.Exit();
        this.state = state;
        this.state.Enter();
    }

    public GameState GetState()
    {
        return state;
    }
}
