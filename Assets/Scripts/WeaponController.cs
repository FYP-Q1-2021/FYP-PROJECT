using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum WeaponType
{
    RANGED,
    MELEE,
    BOW
};

public class WeaponController : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerWeaponsManager playerWeaponsManager;

    [Header("Information")]
    [Tooltip("The name that will be displayed in the UI for this weapon")]
    public string weaponName;
    [Tooltip("The image that will be displayed in the UI for this weapon")]
    public Sprite weaponIcon;
    [Tooltip("Tip of the weapon, where the projectiles are shot")]
    public Transform WeaponMuzzle;
    [Tooltip("The projectile prefab")]
    public Projectile ProjectilePrefab;
    public ProjectileStandard projectile;
    public GameObject defaultArrowPosition;
    public GameObject finalArrowPosition;

    [Tooltip("The parent of the entire weapon")]
    public GameObject weaponRoot;
    public WeaponType weaponType;
    BoxCollider weaponCollider;
    public Animator anim;
    public AnimationClip clip;

    [Header("Shoot Parameters")]
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

    [Header("Ammo Parameters")]
    [Tooltip("Should the player manually reload")]
    public bool AutomaticReload = true;
    [Tooltip("Has physical clip on the weapon and ammo shells are ejected when firing")]
    public bool HasPhysicalBullets = false;
    [Tooltip("Number of bullets in a clip")]
    public int ClipSize = 30;
    [Tooltip("Bullet Shell Casing")]
    public GameObject ShellCasing;
    [Tooltip("Weapon Ejection Port for physical ammo")]
    public Transform EjectionPort;
    [Tooltip("Force applied on the shell")]
    [Range(0.0f, 5.0f)] public float ShellCasingEjectionForce = 2.0f;
    [Tooltip("Maximum number of shell that can be spawned before reuse")]
    [Range(1, 30)] public int ShellPoolSize = 1;
    [Tooltip("Amount of ammo reloaded per second")]
    public float AmmoReloadRate = 1f;
    [Tooltip("Delay after the last shot before starting to reload")]
    public float AmmoReloadDelay = 2f;
    [Tooltip("Maximum amount of ammo in the gun")]
    public float MaxAmmo = 8;

    [Header("Durability")]
    [SerializeField]
    bool hasDurability;
    [SerializeField]
    float weaponDurability;
    [SerializeField]
    float weaponDecayAmount;

    [Header("Weapon Stats")]
    public float damage;
    [Tooltip("If the weapon is currently equipped")]
    public bool isWeaponActive;

    public Vector3 MuzzleWorldVelocity { get; private set; }
    public GameObject Owner { get; set; }
    public GameObject SourcePrefab { get; set; }
    float m_CurrentAmmo;
    Vector3 m_LastMuzzlePosition;

    [SerializeField]
    float attackATime;
    float attackBTime;
    float attackCTime;
    Quaternion originRotation;
    float m_LastTimeShot = Mathf.NegativeInfinity;

    public UnityAction OnShoot;
    public UnityAction OnShootProcessed;

    bool isAttacking;
    int comboStep;
    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.localRotation;
        inputHandler = GameObject.Find("Player").GetComponent<InputHandler>();
        playerWeaponsManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();
        weaponCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        m_CurrentAmmo = MaxAmmo;
        m_LastMuzzlePosition = WeaponMuzzle.position;

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

    public bool TryShoot()
    {
       
        if (m_CurrentAmmo >= 1f && m_LastTimeShot + delayBetweenShots < Time.time)
        {
            m_CurrentAmmo -= 1f;
            HandleShoot();
            
            return true;
        }
        return false;
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
            case WeaponType.BOW:
                if (inputHeld)
                {

                }
                return false;
        }
        return false;
    }

    void OnTriggerEnter(Collider col)
    {

        if (weaponType == WeaponType.MELEE)
        {

            if (col.gameObject.layer == 9)
            {
                Debug.Log("Lol pls");
                weaponDurability -= weaponDecayAmount;
                Health health = col.gameObject.GetComponent<Health>();
                if(health)
                {
                    health.Damage(damage);
                }
                else
                {
                    Hitbox hitbox = col.gameObject.GetComponent<Hitbox>();
                    if (hitbox)
                    {
                        hitbox.InflictDamage(damage);
                    }
                }
            }
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
                case "AttackA":
                    attackATime = clip.length;   
                    break;
                case "AttackB":
                    attackBTime = clip.length;
                    break;
                case "AttackAC":
                    attackCTime = clip.length;
                    break;
            }
        }
    }


    private IEnumerator DisableWeaponCollider(float time = 0f)
    {
        yield return new WaitForSeconds(time);
        weaponCollider.enabled = false;
    }

    private IEnumerator CheckComboTiming(float time = 0f)
    {
        yield return new WaitForSeconds(time);
        isAttacking = true;
    }

    public bool TryAttack()
    {
        if (comboStep == 0)
        {
            weaponCollider.enabled = true;
            anim.Play("AttackA");
            comboStep = 1;
            return true;
        }
        else if(comboStep == 1)
        {
            weaponCollider.enabled = true;
            anim.Play("AttackB");
            comboStep = 2;
            return true;
        }
        else if(comboStep == 2)
        {
            weaponCollider.enabled = true;
            anim.Play("AttackC");
            comboStep = 0;
            return true;
        }
        return false;
    }

    public void ComboPossible()
    {
        isAttacking = true;
    }

    public void Combo()
    {
        if (comboStep == 2)
        {
            weaponCollider.enabled = true;
            anim.Play("AttackB");
        }
        if(comboStep == 3)
        {
            weaponCollider.enabled = true;
            anim.Play("AttackC");
        }
    }

    public void ComboReset()
    {
        isAttacking = false;
        comboStep = 0;
        Debug.Log("Yellow");
    }

    public bool TryReload()
    {
        if(m_CurrentAmmo < ClipSize && MaxAmmo > 0)
        {
            float diff = ClipSize - m_CurrentAmmo;
            if(MaxAmmo >= diff)
            {
                MaxAmmo -= diff;
                m_CurrentAmmo += diff;

            }
            else
            {
                m_CurrentAmmo += MaxAmmo;
                MaxAmmo -= MaxAmmo;
            }
            return true;
        }

        return false;
    }


    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = bulletSpreadAngle / 180f;
        Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere,
            spreadAngleRatio);

        return spreadWorldDirection;
    }

    void HandleShoot()
    {
        int bulletsPerShotFinal = bulletsPerShot;

        // spawn all bullets with random direction
        for (int i = 0; i < bulletsPerShotFinal; i++)
        {
            Vector3 shotDirection = GetShotDirectionWithinSpread(WeaponMuzzle);
            Projectile newProjectile = Instantiate(ProjectilePrefab, WeaponMuzzle.position,
                Quaternion.LookRotation(shotDirection));
            m_LastTimeShot = Time.time;


            newProjectile.Shoot(this);

        }

        
        OnShoot?.Invoke();
        OnShootProcessed?.Invoke();
    }

    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);
        isWeaponActive = show;
    }

}
