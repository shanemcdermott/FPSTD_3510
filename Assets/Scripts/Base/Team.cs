using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamRole
{
    Grunt,
    Commander,
    Stronghold
}

public class Team : MonoBehaviour
{
    public bool bEnableFriendlyFire = false;
    public int id = 0;
    public Color teamColor = new Color(0f,0f,1f);
    public TeamRole role = TeamRole.Grunt;

    void Awake()
    {
        HealthComponent health = GetComponentInChildren<HealthComponent>();
        if(health)
        {
            health.RegisterTeam(id, bEnableFriendlyFire);
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsFriendly(Team inTeam)
    {
        return inTeam.id == id;
    }
}
