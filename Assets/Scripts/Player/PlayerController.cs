using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRespondsToDeath
{
    public Camera fpsCamera;
    public float placementRange;
    public GameObject map;

    public GameObject[] weapons; //rifle, sniper, shotgun, rocket
    public GameObject currentWeapon; //rifle, cannon, rocket, aoe

    public GameObject[] turrets;

    public Material canPlace;
    public Material cantPlace;


    GameObject currentTurret;
    public TurretType currentTurretType;

    bool isPlacing;
    int currentFunds;

    protected Ray traceRay = new Ray();
    protected RaycastHit traceHit;
    protected int buildableMask;

    public PlayerHealth health;

    float playerReloadSpeed;

    public GameObject[] transparentStuff; //wall, rifleTurr, cannon, rocket, aoe
    public GameObject transparentWall;

    public CursorFocus currentFocus = CursorFocus.Default;
    public IFocusable currentFocusable;
    public GameObject currentFocusedGameObject;
    public enum CursorFocus
    {
        Tile, Wall, Tower, Enemy, Default
    }

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
                IFocusable newFocus = newGameObject.GetComponent<IFocusable>();
                if (currentFocusable == null)
                {
                    currentFocusable = newFocus;
                    currentFocusedGameObject = newGameObject;
                    return;
                }
                if (newGameObject != currentFocusedGameObject)
                {
                    if (currentFocus == CursorFocus.Tile)
                    {
                        currentFocusable.onEndFocus(transparentStuff[0]);//hide wall
                    }
                    else if (currentFocus == CursorFocus.Wall)
                    {
                        currentFocusable.onEndFocus(transparentStuff[(int)currentTurretType+1]);//hide turret
                    }
                    else if (currentFocus == CursorFocus.Tower)
                    {
                        currentFocusable.onEndFocus(new GameObject());
                    }

                    switch (hit.transform.tag)
                    {
                        case "Tile":
                            newFocus.onBeginFocus(transparentStuff[0]);
                            currentFocus = CursorFocus.Tile;
                            break;
                        case "Wall":
                            newFocus.onBeginFocus(transparentStuff[(int)currentTurretType+1]);
                            currentFocus = CursorFocus.Wall;
                            break;
                        case "Tower":
                            newFocus.onBeginFocus(new GameObject());
                            currentFocus = CursorFocus.Tower;
                            break;
                        case "Enemy":
                            newFocus.onBeginFocus(new GameObject());
                            currentFocus = CursorFocus.Tile;
                            break;

                    }
                    currentFocusedGameObject = newGameObject;
                    currentFocusable = newFocus;
                }
            }
        }
    }
    private void SwitchWeapon()
    {
        if (!isPlacing)
        {
            currentWeapon.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Alpha1)) currentWeapon = weapons[0];
            if (Input.GetKeyDown(KeyCode.Alpha2)) currentWeapon = weapons[1];
            if (Input.GetKeyDown(KeyCode.Alpha3)) currentWeapon = weapons[2];
            if (Input.GetKeyDown(KeyCode.Alpha4)) currentWeapon = weapons[3];
            currentWeapon.SetActive(true);
        }
    }

    private void SwitchTurret()
    {
        if (isPlacing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentTurret = turrets[0];
                currentFocusable.onEndFocus(transparentStuff[(int)currentTurretType + 1]);
                currentTurretType = TurretType.rifleTurret;
                currentFocusable.onBeginFocus(transparentStuff[(int)currentTurretType + 1]);

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentTurret = turrets[1];
                currentFocusable.onEndFocus(transparentStuff[(int)currentTurretType + 1]);
                currentTurretType = TurretType.cannonTurret;
                currentFocusable.onBeginFocus(transparentStuff[(int)currentTurretType + 1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentTurret = turrets[2];
                currentFocusable.onEndFocus(transparentStuff[(int)currentTurretType + 1]);
                currentTurretType = TurretType.rocketTurret;
                currentFocusable.onBeginFocus(transparentStuff[(int)currentTurretType + 1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentTurret = turrets[3];
                currentFocusable.onEndFocus(transparentStuff[(int)currentTurretType + 1]);
                currentTurretType = TurretType.aoeTurret;
                currentFocusable.onBeginFocus(transparentStuff[(int)currentTurretType + 1]);
            }
        }
    }

    public void HandlePlacement()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isPlacing)
            {
                RaycastHit hit;
                if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, placementRange))
                {
                    Debug.Log(hit.transform.name);

                    Tile tileTarget = hit.transform.GetComponent<Tile>();
                    Wall wallTarget = hit.transform.GetComponent<Wall>();

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
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isPlacing)
            {
                RaycastHit hit;
                if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, placementRange))
                {
                    if (hit.transform.tag == "Wall")
                    {
                        Tile tileHit = hit.transform.parent.GetComponent<Tile>();
                        Wall wallHit = hit.transform.GetComponent<Wall>();
                        if (wallHit.HasTurret())
                        {
                            wallHit.DestroyTurret();
                        }
                        else
                        {
                            tileHit.DestroyWall();
                        }
                    }
                    if (hit.transform.tag == "Tower")
                    {
                        Wall wallHit = hit.transform.parent.GetComponent<Wall>();
                        if (wallHit.HasTurret())
                        {
                            wallHit.DestroyTurret();
                        }
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
        currentWeapon.SetActive(!currentWeapon.activeSelf);
    }
    public void TogglePlacementMode()
    {
        isPlacing = !isPlacing;
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
