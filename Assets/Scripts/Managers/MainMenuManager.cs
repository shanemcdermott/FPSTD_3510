using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    public Animator mainMenu;
    public Animator volumeSlider;
    public Animator levelSelect;

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

    public void ChangeToScene(int sceneToChangeTo)
    {
        Application.LoadLevel(sceneToChangeTo);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
