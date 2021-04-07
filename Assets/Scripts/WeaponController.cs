﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
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
    PlayerWeaponsManager playerWeaponsManager;

    [Header("Information")]
    [Tooltip("The name that will be displayed in the UI for this weapon")]
    public string weaponName;
    [Tooltip("The image that will be displayed in the UI for this weapon")]
    public Sprite weaponIcon;

    [Tooltip("Default data for the crosshair")]
    public CrosshairData crosshairDataDefault;
    [Tooltip("Data for the crosshair when targeting an enemy")]
    public CrosshairData crosshairDataTargetInSight;

    [Tooltip("The parent of the entire weapon")]
    public GameObject weaponRoot;
    public WeaponType weaponType;
    BoxCollider weaponCollider;
    public Animator anim;
    public AnimationClip clip;

    [Header("Shoot Parameters")]
    //[Tooltip("The projectile prefab")]
    //public ProjectileBase projectilePrefab;
    [Tooltip("Minimum duration between two shots")]
    public float delayBetweenShots = 0.5f;
    [Tooltip("Angle for the cone in which the bullets will be shot randomly (0 means no spread at all)")]
    public float bulletSpreadAngle = 0f;
    [Tooltip("Amount of bullets per shot")]
    public int bulletsPerShot = 1;
    [Tooltip("Force that will push back the weapon after each shot")]
    [Range(0f, 2f)]
    public float recoilForce = 1;
    [Tooltip("Ratio of the default FOV that this weapon applies while aiming")]
    [Range(0f, 1f)]
    public float aimZoomRatio = 1f;
    [Tooltip("Translation to apply to weapon arm when aiming with this weapon")]
    public Vector3 aimOffset;

    [Header("Weapon Sway")]
    [Range(0f,10f)]
    [Tooltip("")]
    [SerializeField]
    public float swayIntensity = 1f;
    [Range(0f, 10f)]
    [Tooltip("")]
    [SerializeField]
    float swaySmoothness = 10f;

    [Header("Durability")]
    [SerializeField]
    bool hasDurability;
    [SerializeField]
    int weaponDurability;
    [SerializeField]
    float weaponDecayChance;
    [SerializeField]
    float weaponDecayAmount;

    [Header("Weapon Stats")]
    public int weaponDamage;

    [SerializeField]
    float attackTime;

    Quaternion originRotation;

    [Tooltip("If the weapon is currently equipped")]
    public bool isWeaponActive;

    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.localRotation;
        inputHandler = GameObject.Find("Player").GetComponent<InputHandler>();
        playerWeaponsManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();
        weaponCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();

        if (anim != null)
        {
            UpdateAnimClipTimes();
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeaponSway();
        if(hasDurability)
        {
            CheckWeaponDurability();
        }
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

    public bool HandleAttackInputs(bool inputDown, bool inputHeld)
    {
        switch(weaponType)
        {
            case WeaponType.MELEE:
                if(inputDown)
                {
                    return TryAttack();
                }
                return false;
            case WeaponType.RANGED:
                if(inputHeld)
                {
                    return TryShoot();
                }
                return false;
        }
        return false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == 9)
        {
            weaponDurability--;
        }
    }

    void CheckWeaponDurability()
    {
        if(weaponDurability <= 0)
        {
            playerWeaponsManager.RemoveWeapon(this);
        }
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Attacking":
                    attackTime = clip.length;   
                    break;
            }
        }
    }

    private IEnumerator DisableWeaponCollider(float time = 0f)
    {
        yield return new WaitForSeconds(time);
        weaponCollider.enabled = false;
    }

    public bool TryAttack()
    {
        weaponCollider.enabled = true;
        StartCoroutine(DisableWeaponCollider(attackTime));

        return true;
        //anim.SetTrigger("Attack");
    }

    public bool TryShoot()
    {
        return true;
    }

    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);
        isWeaponActive = show;
    }

}
