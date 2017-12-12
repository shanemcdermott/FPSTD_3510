using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public Animator pause;
    public Animator pauseSettings;
    public Animator waveDefeat;
    public Animator waveVictory;
    public Animator levelVictory;
    public Text waveCrystals;
    public Text levelCrystals1;
    public Text levelCrystals2;
    public Text waveCountDefeat;
    public Text waveCountVictory;
    public Slider volume;
    public Animator upgradeMenu;

    public Button f10;
    public Button f20;
    public Button f30;
    public Button f40;
    public Button f50;

    public Button r10;
    public Button r20;
    public Button r30;
    public Button r40;
    public Button r50;

    public Button d10;
    public Button d20;
    public Button d30;
    public Button d40;
    public Button d50;

    public Button e10;
    public Button e20;
    public Button e30;
    public Button e40;
    public Button e50;

    public Button tc10;
    public Button tc20;
    public Button tc30;
    public Button tc40;
    public Button tc50;

    public Button td10;
    public Button td20;
    public Button td30;
    public Button td40;
    public Button td50;

    public Button tr10;
    public Button tr20;
    public Button tr30;
    public Button tr40;
    public Button tr50;

    public Button rr10;
    public Button rr20;
    public Button rr30;
    public Button rr40;
    public Button rr50;
	
    void Start()
    {
        GameManager.instance.menuManager = this;

        f10.onClick.AddListener(() => buttonUpgrade(f10, f20, 0));
        f20.onClick.AddListener(() => buttonUpgrade(f20, f30, 0));
        f30.onClick.AddListener(() => buttonUpgrade(f30, f40, 0));
        f40.onClick.AddListener(() => buttonUpgrade(f40, f50, 0));
        f50.onClick.AddListener(() => buttonUpgrade(f50, f50, 0));

        r10.onClick.AddListener(() => buttonUpgrade(r10, r20, 1));
        r20.onClick.AddListener(() => buttonUpgrade(r20, r30, 1));
        r30.onClick.AddListener(() => buttonUpgrade(r30, r40, 1));
        r40.onClick.AddListener(() => buttonUpgrade(r40, r50, 1));
        r50.onClick.AddListener(() => buttonUpgrade(r50, r50, 1));

        d10.onClick.AddListener(() => buttonUpgrade(d10, d20, 2));
        d20.onClick.AddListener(() => buttonUpgrade(d20, d30, 2));
        d30.onClick.AddListener(() => buttonUpgrade(d30, d40, 2));
        d40.onClick.AddListener(() => buttonUpgrade(d40, d50, 2));
        d50.onClick.AddListener(() => buttonUpgrade(d50, d50, 2));

        e10.onClick.AddListener(() => buttonUpgrade(e10, e20, 3));
        e20.onClick.AddListener(() => buttonUpgrade(e20, e30, 3));
        e30.onClick.AddListener(() => buttonUpgrade(e30, e40, 3));
        e40.onClick.AddListener(() => buttonUpgrade(e40, e50, 3));
        e50.onClick.AddListener(() => buttonUpgrade(e50, e50, 3));

        tc10.onClick.AddListener(() => buttonUpgrade(tc10, tc20, 4));
        tc20.onClick.AddListener(() => buttonUpgrade(tc20, tc30, 4));
        tc30.onClick.AddListener(() => buttonUpgrade(tc30, tc40, 4));
        tc40.onClick.AddListener(() => buttonUpgrade(tc40, tc50, 4));
        tc50.onClick.AddListener(() => buttonUpgrade(tc50, tc50, 4));

        td10.onClick.AddListener(() => buttonUpgrade(td10, td20, 5));
        td20.onClick.AddListener(() => buttonUpgrade(td20, td30, 5));
        td30.onClick.AddListener(() => buttonUpgrade(td30, td40, 5));
        td40.onClick.AddListener(() => buttonUpgrade(td40, td50, 5));
        td50.onClick.AddListener(() => buttonUpgrade(td50, td50, 5));

        tr10.onClick.AddListener(() => buttonUpgrade(tr10, tr20, 6));
        tr20.onClick.AddListener(() => buttonUpgrade(tr20, tr30, 6));
        tr30.onClick.AddListener(() => buttonUpgrade(tr30, tr40, 6));
        tr40.onClick.AddListener(() => buttonUpgrade(tr40, tr50, 6));
        tr50.onClick.AddListener(() => buttonUpgrade(tr50, tr50, 6));

        rr10.onClick.AddListener(() => buttonUpgrade(rr10, rr20, 7));
        rr20.onClick.AddListener(() => buttonUpgrade(rr20, rr30, 7));
        rr30.onClick.AddListener(() => buttonUpgrade(rr30, rr40, 7));
        rr40.onClick.AddListener(() => buttonUpgrade(rr40, rr50, 7));
        rr50.onClick.AddListener(() => buttonUpgrade(rr50, rr50, 7));
    }

	// Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P) && pause.GetBool("isHidden")) {
            pause.SetBool("isHidden", false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0.0f;
        }

        else if (Input.GetKeyDown(KeyCode.P) && !pause.GetBool("isHidden"))
        {
            pause.SetBool("isHidden", true);
            Cursor.visible = false;
            Time.timeScale = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && upgradeMenu.GetBool("isHidden"))
        {
            upgradeMenu.SetBool("isHidden", false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons[3].StartUnEquipping();
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons[2].StartUnEquipping();
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons[1].StartUnEquipping();
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons[0].StartUnEquipping();
            Time.timeScale = 0.0f;
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && !upgradeMenu.GetBool("isHidden"))
        {
            upgradeMenu.SetBool("isHidden", true);
            Cursor.visible = false;
            Time.timeScale = 1.0f;
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons[3].StartUnEquipping();
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons[2].StartUnEquipping();
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons[1].StartUnEquipping();
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons[0].StartUnEquipping();
            GameManager.instance.GetPlayer().GetComponent<PlayerController>().EquipWeapon(GameManager.instance.GetPlayer().GetComponent<PlayerController>().currentWeaponType);
        }
    }

    public void buttonUpgrade(Button b1, Button b2, int upg)
    {
        if (GameManager.instance.upgrades > 0)
        {
            b2.interactable = true; // Enable next upgrade
            b1.GetComponent<Image>().color = b1.colors.pressedColor;
            b1.interactable = false; // Disable current upgrade
            if(upg == 0) {
                foreach(Weapon w in GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons) {
                    w.gameObject.SetActive(true);
                    w.timeToShoot -= w.timeToShoot * 0.1f;
                }
            }
            else if (upg == 1)
            {
                foreach (Weapon w in GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons)
                {
                    w.gameObject.SetActive(true);
                    w.timeToReload -= w.timeToReload * 0.1f;
                }
            }
            else if (upg == 2)
            {
                foreach (Weapon w in GameManager.instance.GetPlayer().GetComponent<PlayerController>().weapons)
                {
                   w.gameObject.SetActive(true);
                    if (w.isTrace)
                    {
                        ((TraceWeapon)w).damagePerShot += (int)(((TraceWeapon)w).damagePerShot * 0.1f);
                        
                    }
                }
            }
            

            
            GameManager.instance.upgrades--; //Remove one upgrade point
        }
    }

	public void ChangeToScene (int sceneToChangeTo) {
        Application.LoadLevel(sceneToChangeTo);
	}

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenPauseSettings()
    {
        pause.SetBool("isHidden", true);
        pauseSettings.SetBool("isHidden", false);
    }

    public void ClosePauseSettings()
    {
        pause.SetBool("isHidden", false);
        pauseSettings.SetBool("isHidden", true);
    }

    public void ClosePause()
    {
        pause.SetBool("isHidden", true);
        Time.timeScale = 1.0f;
    }

    public void OpenWaveDefeat()
    {
        waveDefeat.SetBool("isHidden", false);
        Time.timeScale = 0.0f;
    }

    public void CloseWaveDefeat()
    {
        waveDefeat.SetBool("isHidden", true);
        Time.timeScale = 1.0f;
    }

    public void OpenWaveVictory()
    {
        waveVictory.SetBool("isHidden", false);
        Time.timeScale = 0.0f;
    }

    public void CloseWaveVictory()
    {
        waveVictory.SetBool("isHidden", true);
        Time.timeScale = 1.0f;
    }

    public void OpenLevelVictory()
    {
        levelVictory.SetBool("isHidden", false);
        Time.timeScale = 0.0f;
    }

    public void CloseLevelVictory()
    {
        levelVictory.SetBool("isHidden", true);
        Time.timeScale = 1.0f;
    }

    public void setVolume()
    {
        GameManager.instance.GetPlayer().GetComponent<PlayerController>().currentWeapon.gunAudio.volume = volume.value;
        GameManager.instance.GetPlayer().GetComponent<PlayerController>().currentWeapon.reloadAudio.volume = volume.value;
    }
}
