﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponType
{
    RANGED,
    MELEE
};

[System.Serializable]
public struct CrosshairData
{
    [Tooltip("The image that will be used for this weapon's crosshair")]
    public Sprite crosshairSprite;
    [Tooltip("The size of the crosshair image")]
    public int crosshairSize;
    [Tooltip("The color of the crosshair image")]
    public Color crosshairColor;
}

public class WeaponController : MonoBehaviour
{
    InputHandler inputHandler;

    [Header("Information")]
    [Tooltip("The name that will be displayed in the UI for this weapon")]
    public string weaponName;
    [Tooltip("The image that will be displayed in the UI for this weapon")]
    public Sprite weaponIcon;

    [Tooltip("Default data for the crosshair")]
    public CrosshairData crosshairDataDefault;
    [Tooltip("Data for the crosshair when targeting an enemy")]
    public CrosshairData crosshairDataTargetInSight;

    public GameObject weaponRoot;
    WeaponType weaponType;

    [Header("Weapon Sway")]
    [Range(0f,10f)]
    [Tooltip("")]
    public float swayIntensity = 1f;
    [Range(0f, 10f)]
    [Tooltip("")]
    public float swaySmoothness = 10f;
    

    Quaternion originRotation;

    [Tooltip("If the weapon is currently equipped")]
    public bool isWeaponActive;

    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.localRotation;
        inputHandler = GameObject.Find("Player").GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponSway();
    }

    void UpdateWeaponSway()
    {

        float xMouse = inputHandler.GetLookInputsHorizontal();
        float yMouse = inputHandler.GetLookInputsVertical();

        Quaternion targetAdjustmentX = Quaternion.AngleAxis(swayIntensity * xMouse, Vector3.up);
        Quaternion targetAdjustmentY = Quaternion.AngleAxis(swayIntensity * yMouse, Vector3.right);
        Quaternion targetRotation = originRotation * targetAdjustmentX * targetAdjustmentY;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmoothness);
    }

    public void HandleAttackInputs(bool inputDown, bool inputHeld, bool inputUp)
    {

    }

    void TryAttack()
    {
        
    }


    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);
        isWeaponActive = show;
    }

}
