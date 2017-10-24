using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    private PlayerHealth health;
    private Weapon weapon;
    Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();

        weapon = GetComponentInChildren<Weapon>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        int size = 12;
        float posx = _camera.pixelWidth / 2 - size / 4;
        float posy = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posx, posy, size, size), "*");

        size = 48;
		string str = "temp";//string.Format(" {0} / {1} ", weapon.GetBulletsInMag(), weapon.bulletsPerMag);
        GUI.Label(new Rect(size, size, size, size), str);

    }

    void Awake()
    {
        health = GetComponent<PlayerHealth>();

    }

}
