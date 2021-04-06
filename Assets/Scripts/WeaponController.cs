using System.Collections;
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
    WeaponType weaponType;
    BoxCollider weaponCollider;
    public Animator anim;
    public AnimationClip clip;

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
        CheckWeaponDurability();
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

    public void TryAttack()
    {
        weaponCollider.enabled = true;
        //anim.SetTrigger("Attack");
        StartCoroutine(DisableWeaponCollider(attackTime));
    }

    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);
        isWeaponActive = show;
    }

}
