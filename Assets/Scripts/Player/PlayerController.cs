﻿using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRespondsToDeath
{
    public Camera fpsCamera;
    public float placementRange;

    public Weapon[] weapons; //rifle, sniper, shotgun, rocket
    public Weapon currentWeapon; //rifle, cannon, rocket, aoe
    public int currentWeaponType;
    public GameObject[] turrets;

    public Material canPlace;
    public Material cantPlace;


    GameObject currentTurret;
    public TurretType currentTurretType;
    public int turretCost = 100;
    public int wallCost = 50;

    public bool isPlacing;
    int currentFunds;

    protected Ray traceRay = new Ray();
    protected RaycastHit traceHit;
    protected int buildableMask;

    public PlayerHealth health;

    float playerReloadSpeed;

    public GameObject[] transparentStuff; //wall, rifleTurr, cannon, rocket, aoe
    public GameObject transparentWall;

    public IFocusable currentFocusable;
    public GameObject currentFocusedGameObject;

	public TileMap tileMap;

    public int damageMod = 0;
    public float attackRange = 0;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentWeapon = weapons[0];
        currentTurret = turrets[0];
        SetupFocusables();
        EquipWeapon(0);
    }

    void Awake()
    {
        health = GetComponent<PlayerHealth>();
        health.RegisterDeathResponder(this);
        isPlacing = false;
        buildableMask = LayerMask.GetMask("Buildable");
    }

    void Update()
    {
        HandleShooting();
        SetCursorFocus();
        HandlePlacement();
        SwitchWeapon();
        SwitchTurret();
        ConsiderFocusRotation();
    }
    private Quaternion t;
    private void HandleShooting()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isPlacing && currentWeapon.CanActivate())
            {
                currentWeapon.recoil += 0.001f;
                currentWeapon.Activate();
            }
        }
        if (Input.GetKey(KeyCode.R))
        {
            currentWeapon.StartReloading();
        }
        currentWeapon.Recoil();
    }

    
    private void SetupFocusables()
    {
        transparentStuff = new GameObject[turrets.Length+1];
        for (int i = 0; i < transparentStuff.Length; i++) {
            if (i == 0)
            {
                transparentStuff[i] = GameObject.Instantiate(transparentWall);
                Renderer renderer = transparentStuff[i].GetComponent<Renderer>();
                Color transparentColor = renderer.sharedMaterial.color;
                transparentColor.a = 0.1f;
                renderer.sharedMaterial.SetColor("_Color", transparentColor);
            }
            else
            {
                transparentStuff[i] = GameObject.Instantiate(turrets[i-1]);
                Renderer[] renderers = transparentStuff[i].GetComponentsInChildren<Renderer>();
                foreach(Renderer renderer in renderers)
                {
                    Color transparentColor = renderer.sharedMaterial.color;
                    transparentColor.a = 0.1f;
                    renderer.sharedMaterial.SetColor("_Color", transparentColor);
                }
            }
            transparentStuff[i].SetActive(false);
        }
    }
    private void SetCursorFocus()
    {
        if (isPlacing)
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, placementRange))
            {
				Vector3 hitloc;
				if (hit.transform.gameObject.GetComponentInChildren<Wall>() == null)
				{
					hitloc = new Vector3(hit.point.x, 0, hit.point.z);
				}
				else
				{
					hitloc = hit.transform.position;
				}



				int x, z;
				tileMap.nodeAtLocation(hitloc, out x, out z);
				Tile t = tileMap.getTileAt(x, z);
                GameObject newGameObject = null;
                if(t != null)
				    newGameObject = t.gameObject;
                if(newGameObject != currentFocusedGameObject)
                {
                    if (currentFocusable != null)
                        currentFocusable.onEndFocus(this);

                    currentFocusedGameObject = newGameObject;
                    if (currentFocusedGameObject == null)
                    {
                        currentFocusable = null;
                    }
                    else
                    { 
                        currentFocusable = currentFocusedGameObject.GetComponent<IFocusable>();
                        if(currentFocusable != null)
                        {
                            currentFocusable.onBeginFocus(this);
                        }
                    }
                    
                }

            }
        }
    }
    private void SwitchWeapon()
    {
        if (!isPlacing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) EquipWeapon(3);
        }
    }

    public void EquipWeapon(int index)
    {
        currentWeaponType = index;
        float waitTime = currentWeapon.StartUnEquipping();
        currentWeapon = weapons[index];
        currentWeapon.mainCamera = fpsCamera;
        currentWeapon.aimTransform = fpsCamera.transform;
        //currentWeapon.useRootTransform = true;
        Invoke("FinishedUnequipping", waitTime);
    }

    private void FinishedUnequipping()
    {
        Invoke("FinishedEquipping", currentWeapon.StartEquipping());
    }

    private void FinishedEquipping()
    {
        if (!isPlacing)
        {
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.SetCurrentState(WeaponState.Idle);
        }
    }

    private void SwitchTurret()
    {
        if (isPlacing)
        {

            int prevTurretType = -1;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                prevTurretType = (int)currentTurretType + 1;
                currentTurret = turrets[0];
                currentTurretType = TurretType.rifleTurret;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                prevTurretType = (int)currentTurretType + 1;
                currentTurret = turrets[1];
                currentTurretType = TurretType.cannonTurret;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                prevTurretType = (int)currentTurretType + 1;
                currentTurret = turrets[2];
                currentTurretType = TurretType.rocketTurret;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                prevTurretType = (int)currentTurretType + 1;
                currentTurret = turrets[3];    
                currentTurretType = TurretType.aoeTurret;
            }
            if (prevTurretType != -1)
            {
                transparentStuff[prevTurretType].SetActive(false);
                if(currentFocusable != null)
                    currentFocusable.onBeginFocus(this);
            }
        }
    }

    public void HandlePlacement()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isPlacing) 
            {
                if (currentFocusedGameObject != null)
                {
					
                    Tile tileTarget = currentFocusedGameObject.GetComponent<Tile>();
                    Wall wallTarget = currentFocusedGameObject.GetComponentInChildren<Wall>();

					if (wallTarget != null)
					{
						if (!wallTarget.HasTurret())
						{
							if (turretCost <= GameManager.instance.crystals)
							{
								wallTarget.PlaceTurret(currentTurret, currentTurretType, damageMod, attackRange);
								GameManager.instance.crystals -= turretCost;
							}
						}

					}
                    else if (tileTarget != null)
                    {
						
                        if (!tileTarget.HasWall())
                        {
                            if (wallCost <= GameManager.instance.crystals)
                            {
                                tileTarget.getParentTileMap().PlaceWallHere(tileTarget.getXPos(), tileTarget.getZPos());
                                GameManager.instance.crystals -= wallCost;
                            }
                        }
                    }
                    
                }   
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isPlacing && currentFocusedGameObject != null)
            {
                Tile tileTarget = currentFocusedGameObject.GetComponent<Tile>();
                Wall wallTarget = currentFocusedGameObject.GetComponent<Wall>();
                if (wallTarget != null)
                {
                    if (wallTarget.HasTurret())
                    {
                        wallTarget.DestroyTurret();
                    }
                    else if(tileTarget != null)
                    {
                        tileTarget.getParentTileMap().DestroyWallHere(tileTarget.getXPos(), tileTarget.getZPos());
                    }

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePlacementMode();
            ToggleWeapon();
        }
    }
    private void ToggleWeapon()
    {
        if (isPlacing)
        {
            currentWeapon.SetCurrentState(WeaponState.Unequipped);
            currentWeapon.DisableEffects();
            currentWeapon.gameObject.SetActive(false);

        }
        else
        {
            currentWeapon.SetCurrentState(WeaponState.Idle);
            currentWeapon.gameObject.SetActive(true);
        }

    }
    public void TogglePlacementMode()
    {
        isPlacing = !isPlacing;

		if (!isPlacing)
        {
            foreach (GameObject transparent in transparentStuff)
            {
                transparent.SetActive(false);
            }
        }
    }

    private void ConsiderFocusRotation()
    {
        if(Input.GetAxis("Mouse ScrollWheel") != 0 && currentFocusable != null)
        {
            Turret t = currentFocusedGameObject.GetComponentInChildren<Turret>();
            if(t)
            {
                t.rotateFocus();
            }
        }


    }

    public void DisableEffects()
    {
        currentWeapon.GetComponent<Weapon>().DisableEffects();
    }

    public void OnDeath(DamageContext context)
    {
        DisableEffects();
    }

    void OnGUI()
    {
//        int size = 12;
//        float posx = fpsCamera.pixelWidth / 2 - size / 4;
//        float posy = fpsCamera.pixelHeight / 2 - size / 2;
//        GUI.Label(new Rect(posx, posy, size, size), "*");

        //size = 48;
        //string str = string.Format(" {0} / {1} ", weapon.GetBulletsInMag(), weapon.bulletsPerMag);
        //GUI.Label(new Rect(size, size, size, size), str);

    }
}
