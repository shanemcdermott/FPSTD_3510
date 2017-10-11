using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera fpsCamera;
    public float placementRange;
    public GameObject map;

    public GameObject grenadeTurret;
    public GameObject cannonTurret;
    public GameObject rifleTurret;
    public GameObject aoeTurret;

    public GameObject wall;

    public Material greenMaterial;
    public Material redMaterial;


    public GameObject[] weapons;
    public GameObject rifle;
    public GameObject sniper;
    public GameObject shotgun;
    public GameObject grenadeLauncher;
    public GameObject equippedWeapon;

    int hp;
    int currentHP;
    float movementSpeed;
    float playerReloadSpeed;

    bool isPlacing;
    GameObject[] turrets;
    GameObject currentPlaceable;
    GameObject currentTurret;
    TurretType currentTurretType;

    int currentFunds;
    void Awake()
    {
        isPlacing = false;
        weapons = new GameObject[] { rifleTurret, sniper, shotgun, grenadeLauncher };
        turrets = new GameObject[] { rifleTurret, grenadeTurret, cannonTurret, aoeTurret };
        equippedWeapon = rifle;
        currentTurret = rifleTurret;
        currentTurretType = TurretType.rifleTurret;
    }

    void Update()
    {
        HandleInput();
    }
    public void HandleInput()
    {
        if (Input.GetMouseButton(0))
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
                    if (wallTarget != null)
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
                if (equippedWeapon.GetComponent<Weapon>().CanActivate())
                {
                    equippedWeapon.GetComponent<Weapon>().Shoot();
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
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKey(i.ToString()))
            {
                if (isPlacing)
                {
                    currentTurret = turrets[i - 1];
                }
                else
                {
                    if (i == 5)
                    {
                        continue;
                    }
                    equippedWeapon = weapons[i - 1];
                }
            }
        }

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
        //if(equipment != null)
        //{
        //    equipment.DisableEffects();
        //}
    }

    /*
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;


    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }
    */
}
