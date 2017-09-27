using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public Animator playButton;
    public Animator settingsButton;
    public Animator exitButton;
    public Animator volumeSlider;
    public Animator levelSelect;
	
	// Update is called once per frame
	public void ChangeToScene (int sceneToChangeTo) {
        Application.LoadLevel(sceneToChangeTo);
	}

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        playButton.SetBool("isHidden", true);
        settingsButton.SetBool("isHidden", true);
        exitButton.SetBool("isHidden", true);
        volumeSlider.SetBool("isHidden", false);
    }

    public void SettingsBack()
    {
        playButton.SetBool("isHidden", false);
        settingsButton.SetBool("isHidden", false);
        exitButton.SetBool("isHidden", false);
        volumeSlider.SetBool("isHidden", true);
    }

    public void OpenLevelSelect()
    {
        playButton.SetBool("isHidden", true);
        settingsButton.SetBool("isHidden", true);
        exitButton.SetBool("isHidden", true);
        levelSelect.SetBool("isHidden", false);
    }

    public void LevelSelectBack()
    {
        playButton.SetBool("isHidden", false);
        settingsButton.SetBool("isHidden", false);
        exitButton.SetBool("isHidden", false);
        levelSelect.SetBool("isHidden", true);
    }
}
