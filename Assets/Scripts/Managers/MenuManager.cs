using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public Animator mainMenu;
    public Animator volumeSlider;
    public Animator levelSelect;
    public Animator pause;
    public Animator pauseSettings;
    public Animator waveDefeat;
    public Animator waveVictory;
    public Animator levelVictory;
    public Text waveCrystals;
    public Text levelCrystals1;
    public Text levelCrystals2;
	
    void Start()
    {
        GameManager.instance.menuManager = this;
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
        
    }

	public void ChangeToScene (int sceneToChangeTo) {
        Application.LoadLevel(sceneToChangeTo);
	}

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenu.SetBool("isHidden", true);
        volumeSlider.SetBool("isHidden", false);
    }

    public void SettingsBack()
    {
        mainMenu.SetBool("isHidden", false);
        volumeSlider.SetBool("isHidden", true);
    }

    public void OpenLevelSelect()
    {
        mainMenu.SetBool("isHidden", true);
        levelSelect.SetBool("isHidden", false);
    }

    public void LevelSelectBack()
    {
        mainMenu.SetBool("isHidden", false);
        levelSelect.SetBool("isHidden", true);
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
}
