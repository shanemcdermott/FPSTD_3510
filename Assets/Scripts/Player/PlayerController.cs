using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRespondsToDeath
{
    public Camera fpsCamera;
    public float placementRange;
    public GameObject map;

    public GameObject wall;

    public Material greenMaterial;
    public Material redMaterial;


    public Weapon[] weapons;
    public int equippedIndex;

    public GameObject[] turrets;
    public int turretIndex;

    public GameObject[] placeableObjects;
    public int placeableIndex;

    private CharacterController _characterController;
    public float movementSpeed = 40f;
    public float gravity = -9.8f;

    public PlayerHealth health;
    
    float playerReloadSpeed;

    bool isPlacing;

    GameObject currentPlaceable;
    GameObject currentTurret;
    TurretType currentTurretType;

    int currentFunds;

    protected Ray traceRay = new Ray();
    protected RaycastHit traceHit;
    protected int buildableMask;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        health = GetComponent<PlayerHealth>();
        health.RegisterDeathResponder(this);
        isPlacing = false;
        equippedIndex = 0;
        turretIndex = 0;
        placeableIndex = 0;
        buildableMask = LayerMask.GetMask("Buildable");
    }

    void Update()
    {
        HandleInput();
    }
    public void HandleInput()
    {
        if (Input.GetButton("Fire1"))
        {
            if (isPlacing)
            {
                traceRay.origin = transform.position;
                traceRay.direction = transform.forward;
                if (Physics.Raycast(traceRay, out traceHit, placementRange, buildableMask))
                {
                    Debug.Log("Placing...");

                    Tile tileTarget = traceHit.collider.GetComponent<Tile>();
                    //Tile tileTarget = hit.transform.GetComponent<Tile>();
                  
                    if (tileTarget != null)
                    {
                        if (!tileTarget.HasWall())
                        {
                            tileTarget.PlaceWall();
                            currentFunds -= 10;
                        }
                    }
                    else
                    {
                        Wall wallTarget = traceHit.transform.GetComponent<Wall>();
                        if (wallTarget != null)
                        {
                            if (!wallTarget.HasTurret())
                            {
                                wallTarget.PlaceTurret(currentTurret, currentTurretType);
                            }
                        }
                    }
                }
            }
            else
            {
                if(weapons[equippedIndex].CanActivate())
                {
                    weapons[equippedIndex].Activate();
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPlacing)
            {
                Debug.Log("turning placement mode off");
            }
            else
            {
                Debug.Log("turning placement mode on");
            }
            TogglePlacementMode();
        }
        if(isPlacing)
        {
            for(int i = 1; i <= turrets.Length; i++)
            {
                if(Input.GetKey(i.ToString()))
                {
                    turretIndex = i - 1;
                    currentTurret = turrets[turretIndex];
                }
            }
        }
        else
        {
            for(int i = 1; i <= weapons.Length; i++)
            {
                if(Input.GetKey(i.ToString()))
                {
                    EquipWeapon(i - 1);
                }
            }
        }

        float deltaX = Input.GetAxis("Horizontal");// * movementSpeed;
        float deltaZ = Input.GetAxis("Vertical");// * movementSpeed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = movement.normalized * movementSpeed;


        movement = transform.TransformDirection(movement);

        movement.y = gravity;
        movement *= Time.deltaTime;

        _characterController.Move(movement);
        }

    public void EquipWeapon(int weaponIndex)
    {
        //TODO:
        //Unequip current weapon
        //Wait for amount of time.
        float timeToUnEquip = weapons[equippedIndex].StartUnEquipping();
        //Equip new weapon after timeToUnEquip has passed
        equippedIndex = weaponIndex;
        weapons[equippedIndex].StartEquipping();
    }

    public void TogglePlacementMode()
    {
        isPlacing = !isPlacing;
        Component[] tiles = map.GetComponentsInChildren<Tile>();
        if (isPlacing)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.IsPlaceable())
                {
                    Material childMaterial = tile.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material;
                    childMaterial.color = new Color(childMaterial.color.r + 100, childMaterial.color.g, childMaterial.color.b);
                }
            }
        }
        else
        {
            foreach (Tile tile in tiles)
            {
                if (tile.IsPlaceable())
                {
                    Material childMaterial = tile.transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material;
                    childMaterial.color = new Color(childMaterial.color.r - 100, childMaterial.color.g, childMaterial.color.b);
                }
            }
        }
    }

    public void DisableEffects()
    {
        if(weapons[equippedIndex] != null)
            weapons[equippedIndex].DisableEffects();
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
