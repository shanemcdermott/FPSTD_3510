using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRespondsToDeath
{
    public Camera fpsCamera;
    public float placementRange;

    public Weapon[] weapons; //rifle, sniper, shotgun, rocket
    public Weapon currentWeapon; //rifle, cannon, rocket, aoe

    public GameObject[] turrets;

    public Material canPlace;
    public Material cantPlace;


    GameObject currentTurret;
    public TurretType currentTurretType;

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


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentWeapon = weapons[0];
        currentTurret = turrets[0];

        SetupFocusables();
        
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
        SetCursorFocus();
        HandlePlacement();
        SwitchWeapon();
        SwitchTurret();
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
                GameObject newGameObject = hit.transform.gameObject;
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

    private void EquipWeapon(int index)
    {
        float waitTime = currentWeapon.StartUnEquipping();
        currentWeapon = weapons[index];
        currentWeapon.useRootTransform = true;
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
                    Wall wallTarget = currentFocusedGameObject.GetComponent<Wall>();

                    if (tileTarget != null)
                    {
                        if (!tileTarget.HasWall())
                        {
                            tileTarget.PlaceWall();
                            currentFunds -= 10;
                        }
                    }
                    else if (wallTarget != null)
                    {
                        if (!wallTarget.HasTurret())
                        {
                            wallTarget.PlaceTurret(currentTurret, currentTurretType);
                        }

                    }
                }   
            }
            else
            {
                if(currentWeapon.CanActivate())
                    currentWeapon.Activate();
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
                        tileTarget.DestroyWall();
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
        GameObject map = GameManager.instance.GetTileMap().gameObject;
        Component[] tiles = map.GetComponentsInChildren<Tile>();
        if (isPlacing)
        {
            foreach (Tile tile in tiles)
            {
                Material childMaterial = tile.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material;
                childMaterial.color = new Color(childMaterial.color.r + 100, childMaterial.color.g, childMaterial.color.b);
            }
        }
        else
        {
            foreach (Tile tile in tiles)
            {
                Material childMaterial = tile.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material;
                childMaterial.color = new Color(childMaterial.color.r - 100, childMaterial.color.g, childMaterial.color.b);
            }
            foreach (GameObject transparent in transparentStuff)
            {
                transparent.SetActive(false);
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
        int size = 12;
        float posx = fpsCamera.pixelWidth / 2 - size / 4;
        float posy = fpsCamera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posx, posy, size, size), "*");

        //size = 48;
        //string str = string.Format(" {0} / {1} ", weapon.GetBulletsInMag(), weapon.bulletsPerMag);
        //GUI.Label(new Rect(size, size, size, size), str);

    }
}
