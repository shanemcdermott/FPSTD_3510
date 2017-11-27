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

        f10.onClick.AddListener(() => buttonUpgrade(f10, f20, GameManager.instance.fireRate));
        f20.onClick.AddListener(() => buttonUpgrade(f20, f30, GameManager.instance.fireRate));
        f30.onClick.AddListener(() => buttonUpgrade(f30, f40, GameManager.instance.fireRate));
        f40.onClick.AddListener(() => buttonUpgrade(f40, f50, GameManager.instance.fireRate));
        f50.onClick.AddListener(() => buttonUpgrade(f50, f50, GameManager.instance.fireRate));

        r10.onClick.AddListener(() => buttonUpgrade(r10, r20, GameManager.instance.reload));
        r20.onClick.AddListener(() => buttonUpgrade(r20, r30, GameManager.instance.reload));
        r30.onClick.AddListener(() => buttonUpgrade(r30, r40, GameManager.instance.reload));
        r40.onClick.AddListener(() => buttonUpgrade(r40, r50, GameManager.instance.reload));
        r50.onClick.AddListener(() => buttonUpgrade(r50, r50, GameManager.instance.reload));

        d10.onClick.AddListener(() => buttonUpgrade(d10, d20, GameManager.instance.damage));
        d20.onClick.AddListener(() => buttonUpgrade(d20, d30, GameManager.instance.damage));
        d30.onClick.AddListener(() => buttonUpgrade(d30, d40, GameManager.instance.damage));
        d40.onClick.AddListener(() => buttonUpgrade(d40, d50, GameManager.instance.damage));
        d50.onClick.AddListener(() => buttonUpgrade(d50, d50, GameManager.instance.damage));

        e10.onClick.AddListener(() => buttonUpgrade(e10, e20, GameManager.instance.equipTime));
        e20.onClick.AddListener(() => buttonUpgrade(e20, e30, GameManager.instance.equipTime));
        e30.onClick.AddListener(() => buttonUpgrade(e30, e40, GameManager.instance.equipTime));
        e40.onClick.AddListener(() => buttonUpgrade(e40, e50, GameManager.instance.equipTime));
        e50.onClick.AddListener(() => buttonUpgrade(e50, e50, GameManager.instance.equipTime));

        tc10.onClick.AddListener(() => buttonUpgrade(tc10, tc20, GameManager.instance.turretCost));
        tc20.onClick.AddListener(() => buttonUpgrade(tc20, tc30, GameManager.instance.turretCost));
        tc30.onClick.AddListener(() => buttonUpgrade(tc30, tc40, GameManager.instance.turretCost));
        tc40.onClick.AddListener(() => buttonUpgrade(tc40, tc50, GameManager.instance.turretCost));
        tc50.onClick.AddListener(() => buttonUpgrade(tc50, tc50, GameManager.instance.turretCost));

        td10.onClick.AddListener(() => buttonUpgrade(td10, td20, GameManager.instance.turretDamage));
        td20.onClick.AddListener(() => buttonUpgrade(td20, td30, GameManager.instance.turretDamage));
        td30.onClick.AddListener(() => buttonUpgrade(td30, td40, GameManager.instance.turretDamage));
        td40.onClick.AddListener(() => buttonUpgrade(td40, td50, GameManager.instance.turretDamage));
        td50.onClick.AddListener(() => buttonUpgrade(td50, td50, GameManager.instance.turretDamage));

        tr10.onClick.AddListener(() => buttonUpgrade(tr10, tr20, GameManager.instance.turretRadius));
        tr20.onClick.AddListener(() => buttonUpgrade(tr20, tr30, GameManager.instance.turretRadius));
        tr30.onClick.AddListener(() => buttonUpgrade(tr30, tr40, GameManager.instance.turretRadius));
        tr40.onClick.AddListener(() => buttonUpgrade(tr40, tr50, GameManager.instance.turretRadius));
        tr50.onClick.AddListener(() => buttonUpgrade(tr50, tr50, GameManager.instance.turretRadius));

        rr10.onClick.AddListener(() => buttonUpgrade(rr10, rr20, GameManager.instance.RocketRadius));
        rr20.onClick.AddListener(() => buttonUpgrade(rr20, rr30, GameManager.instance.RocketRadius));
        rr30.onClick.AddListener(() => buttonUpgrade(rr30, rr40, GameManager.instance.RocketRadius));
        rr40.onClick.AddListener(() => buttonUpgrade(rr40, rr50, GameManager.instance.RocketRadius));
        rr50.onClick.AddListener(() => buttonUpgrade(rr50, rr50, GameManager.instance.RocketRadius));
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
            Time.timeScale = 0.0f;
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && !upgradeMenu.GetBool("isHidden"))
        {
            upgradeMenu.SetBool("isHidden", true);
            Cursor.visible = false;
            Time.timeScale = 1.0f;
        }
    }

    public void buttonUpgrade(Button b1, Button b2, int upg)
    {
        if (GameManager.instance.upgrades > 0)
        {
            b2.interactable = true; // Enable next upgrade
            b1.GetComponent<Image>().color = b1.colors.pressedColor;
            b1.interactable = false; // Disable current upgrade
            upg += 10; // Increase by 10 percent
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
