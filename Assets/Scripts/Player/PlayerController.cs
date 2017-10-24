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
    TurretType currentTurretType;

    bool isPlacing;
    int currentFunds;

    protected Ray traceRay = new Ray();
    protected RaycastHit traceHit;
    protected int buildableMask;

    public PlayerHealth health;

    float playerReloadSpeed;

    Material[] defaultTileMat;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentWeapon = weapons[0];
        currentTurret = turrets[0];
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
        HandlePlacement();
        SwitchWeapon();
        SwitchTurret();
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
                currentTurretType = TurretType.rifleTurret;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentTurret = turrets[1];
                currentTurretType = TurretType.cannonTurret;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentTurret = turrets[2];
                currentTurretType = TurretType.rocketTurret;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentTurret = turrets[3];
                currentTurretType = TurretType.aoeTurret;
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
