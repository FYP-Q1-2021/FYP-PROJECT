using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerWeaponsManager : MonoBehaviour
{
    public enum WeaponSwitchState
    {
        UP,
        DOWN,
        PUTDOWNPREVIOUS,
        PUTUPNEW,
    }

    [Tooltip("List of weapon the player will start with")]
    public List<WeaponController> startingWeapons = new List<WeaponController>();

    [Header("References")]
    [Tooltip("Position for weapons when active but not actively aiming")]
    public Transform defaultWeaponPosition;
    [Tooltip("Position for innactive weapons")]
    public Transform downWeaponPosition;
    [Tooltip("Parent transform where all weapon will be added in the hierarchy")]
    public Transform weaponParentSocket;
    [Tooltip("Position for aiming down sight")]
    public Transform aDSWeaponPosition;
    [Header("References")]
    [Tooltip("Secondary camera used to avoid seeing weapon go throw geometries")]
    public Camera weaponCamera;

    [Header("Weapon Recoil")]
    [Tooltip("This will affect how fast the recoil moves the weapon, the bigger the value, the fastest")]
    public float recoilSharpness = 50f;
    [Tooltip("Maximum distance the recoil can affect the weapon")]
    public float maxRecoilDistance = 0.5f;
    [Tooltip("How fast the weapon goes back to it's original position after the recoil is finished")]
    public float recoilRestitutionSharpness = 10f;

    [Header("Weapon Bob")]
    [Tooltip("Distance the weapon bobs when not aiming")]
    public float defaultWeaponBobAmount = 0.05f;
    [Tooltip("Distance the weapon bobs when aiming")]
    public float aimingBobAmount = 0.02f;
    [Tooltip("Frequency at which the weapon will move around in the screen when the player is in movement")]
    public float weaponBobFrequency = 10f;
    [Tooltip("How fast the weapon bob is applied, the bigger value the fastest")]
    public float weaponBobSharpness = 10f;
    float weaponBobFactor;


    [Header("Misc")]
    [Tooltip("Speed at which the aiming animatoin is played")]
    public float aimingAnimationSpeed = 10f;
    [Tooltip("Field of view when not aiming")]
    public float defaultFOV = 60f;
    [Tooltip("Portion of the regular FOV to apply to the weapon camera")]
    public float weaponFOVMultiplier = 1f;
    [Tooltip("Delay before switching weapon a second time, to avoid recieving multiple inputs from mouse wheel")]
    public float weaponSwitchDelay = 1f;

    [Tooltip("Check if there are any available weapon slots")]
    public bool slotsFull;

    public UnityAction<WeaponController> OnSwitchedToWeapon;
    public UnityAction<WeaponController, int> OnAddedWeapon;
    public UnityAction<WeaponController, int> OnRemovedWeapon;

    [Tooltip("Layer to set FPS weapon gameObjects to")]
    public LayerMask FPSWeapon;

    Vector3 lastCharacterPosition;
    Vector3 weaponBobLocalPosition;
    Vector3 weaponMainLocalPosition;
    Vector3 weaponParentOrigin;
    Vector3 weaponRecoilLocalPosition;
    Vector3 accumulatedRecoil;
    float timeStartedWeaponSwitch;
    int weaponSwitchNewWeaponIndex;

    Vector3 targetWeaponBobPosition;
    float targetWeaponBobSmoothness;
    float idleCounter;
    float movementCounter;
    float sprintCounter;

    bool isAttacking;
    PlayerCharacterController playerCharacterController;
    InputHandler inputHandler;

    public bool isAiming { get; private set; }

    WeaponSwitchState weaponSwitchState;

    int activeWeaponIndex;
    int newWeaponIndex;

    int numberOfWeapons;

    WeaponController[] weaponSlots = new WeaponController[3]; // 9 available weapon slots

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerCharacterController = GetComponent<PlayerCharacterController>();
        weaponParentOrigin = weaponParentSocket.localPosition;
        activeWeaponIndex = -1;
        weaponSwitchState = WeaponSwitchState.DOWN;
        OnSwitchedToWeapon += OnWeaponSwitched;
        weaponCamera = GameObject.Find("WeaponCamera").GetComponent<Camera>();
        SetFOV(defaultFOV);

        foreach (var weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }
        SwitchWeapon(true);
    }

    void Update()
    {

        // shoot handling
        WeaponController activeWeapon = GetActiveWeapon();

        // weapon switch handling
        if (!isAiming && (weaponSwitchState == WeaponSwitchState.UP || weaponSwitchState == WeaponSwitchState.DOWN))
        {
            int switchWeaponInput = inputHandler.GetSelectWeaponInput();
            
            if (switchWeaponInput != 0)
            {
                if (GetWeaponAtIndex(switchWeaponInput - 1) != null)
                    SwitchToWeaponAtIndex(switchWeaponInput - 1);
            }
        }

        if(activeWeapon && weaponSwitchState == WeaponSwitchState.UP)
        {
            isAttacking = activeWeapon.HandleAttackInputs(inputHandler.GetAttackInputDown(), inputHandler.GetAttackInputHeld());
            // if its a ranged weapon, it can ads
            if(activeWeapon.weaponType == WeaponType.RANGED)
            {
                isAiming = inputHandler.GetAimInputHeld();

                if (inputHandler.GetReloadInputDown())
                {
                    activeWeapon.TryReload();
                }
            }

            // Handle accumulating recoil
            if (isAttacking)
            {
                accumulatedRecoil += Vector3.back * activeWeapon.recoilForce;
                accumulatedRecoil = Vector3.ClampMagnitude(accumulatedRecoil, maxRecoilDistance);
            }

            // add weapon attack
        }
    }

    void LateUpdate()
    {
        
        UpdateWeaponBob();
        UpdateWeaponAiming();
        UpdateWeaponSwitching();
        UpdateWeaponRecoil();

        weaponParentSocket.localPosition = weaponMainLocalPosition + weaponBobLocalPosition + weaponRecoilLocalPosition;
    }

    // Sets the FOV of the main camera and the weapon camera simultaneously
    public void SetFOV(float fov)
    {
        playerCharacterController.playerCamera.fieldOfView = fov;
        weaponCamera.fieldOfView = fov * weaponFOVMultiplier;
    }

    void CheckWeaponSlots()
    {
        if(numberOfWeapons == weaponSlots.Length)
        {
            slotsFull = true;
        }
        else
        {
            slotsFull = false;
        }
    }

    WeaponController GetActiveWeapon()
    {
        return GetWeaponAtIndex(activeWeaponIndex);
    }

    WeaponController GetWeaponAtIndex(int weaponAtIndex)
    {
        if(weaponAtIndex >= 0 && weaponAtIndex < weaponSlots.Length)
        {
            return weaponSlots[weaponAtIndex];
        }
        return null;
    }

    void OnWeaponSwitched(WeaponController newWeapon)
    {
        if (newWeapon != null)
        {
            newWeapon.ShowWeapon(true);
        }
    }

    void SwitchToWeaponAtIndex(int weaponAtIndex)
    {
        // Valid input + not currently equipped weapon
        if(weaponAtIndex >= 0 && weaponAtIndex != activeWeaponIndex)
        {
            // Store data related to weapon switching animation
            weaponSwitchNewWeaponIndex = weaponAtIndex;
            timeStartedWeaponSwitch = Time.time;

            // Handle case of switching to a valid weapon for the first time (simply put it up without putting anything down first)
            if (GetActiveWeapon() == null)
            {
                weaponMainLocalPosition = downWeaponPosition.localPosition;
                weaponSwitchState = WeaponSwitchState.PUTUPNEW;
                activeWeaponIndex = weaponSwitchNewWeaponIndex;

                WeaponController newWeapon = GetWeaponAtIndex(weaponSwitchNewWeaponIndex);
                if (OnSwitchedToWeapon != null)
                {
                    OnSwitchedToWeapon.Invoke(newWeapon);
                }
            }
            // otherwise, remember we are putting down our current weapon for switching to the next one
            else
            {
                weaponSwitchState = WeaponSwitchState.PUTDOWNPREVIOUS;
            }
        }
    }

    void SwitchWeapon(bool ascendingOrder)
    {
        int newWeaponIndex = -1;
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (GetWeaponAtIndex(i) != null && i != activeWeaponIndex)
            {
                newWeaponIndex = i;
            }
        }
        SwitchToWeaponAtIndex(newWeaponIndex);
    }

    void UpdateWeaponAiming()
    {
        if (weaponSwitchState == WeaponSwitchState.UP)
        {
            WeaponController activeWeapon = GetActiveWeapon();
            if (isAiming && activeWeapon)
            {
                weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition, aDSWeaponPosition.localPosition + activeWeapon.aimOffset, aimingAnimationSpeed * Time.deltaTime);
                SetFOV(Mathf.Lerp(playerCharacterController.playerCamera.fieldOfView, activeWeapon.aimZoomRatio * defaultFOV, aimingAnimationSpeed * Time.deltaTime));
            }
            else
            {
                weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition, defaultWeaponPosition.localPosition, aimingAnimationSpeed * Time.deltaTime);
                SetFOV(Mathf.Lerp(playerCharacterController.playerCamera.fieldOfView, defaultFOV, aimingAnimationSpeed * Time.deltaTime));
            }
        }
    }

    void UpdateWeaponRecoil()
    {
        // if the accumulated recoil is further away from the current position, make the current position move towards the recoil target
        if (weaponRecoilLocalPosition.z >= accumulatedRecoil.z * 0.99f)
        {
            weaponRecoilLocalPosition = Vector3.Lerp(weaponRecoilLocalPosition, accumulatedRecoil, recoilSharpness * Time.deltaTime);
        }
        // otherwise, move recoil position to make it recover towards its resting pose
        else
        {
            weaponRecoilLocalPosition = Vector3.Lerp(weaponRecoilLocalPosition, Vector3.zero, recoilRestitutionSharpness * Time.deltaTime);
            accumulatedRecoil = weaponRecoilLocalPosition;
        }
    }

    void UpdateWeaponSwitching()
    {
        // Calculate the time ratio (0 to 1) since weapon switch was triggered
        float switchingTimeFactor = 0f;
        if (weaponSwitchDelay == 0f)
        {
            switchingTimeFactor = 1f;
        }
        else
        {
            switchingTimeFactor = Mathf.Clamp01((Time.time - timeStartedWeaponSwitch) / weaponSwitchDelay);
        }

        // Handle transiting to new switch state
        if (switchingTimeFactor >= 1f)
        {
            if (weaponSwitchState == WeaponSwitchState.PUTDOWNPREVIOUS)
            {
                // Deactivate old weapon
                WeaponController oldWeapon = GetWeaponAtIndex(activeWeaponIndex);
                if (oldWeapon != null)
                {
                    oldWeapon.ShowWeapon(false);
                }
                
                activeWeaponIndex = weaponSwitchNewWeaponIndex;
                switchingTimeFactor = 0f;
                // Activate new weapon
                WeaponController newWeapon = GetWeaponAtIndex(activeWeaponIndex);
                if (OnSwitchedToWeapon != null)
                {
                    OnSwitchedToWeapon.Invoke(newWeapon);
                }

                if (newWeapon)
                {
                    timeStartedWeaponSwitch = Time.time;
                    weaponSwitchState = WeaponSwitchState.PUTUPNEW;
                }
                else
                {
                    // if new weapon is null, don't follow through with putting weapon back up
                    weaponSwitchState = WeaponSwitchState.DOWN;
                }
            }
            else if (weaponSwitchState == WeaponSwitchState.PUTUPNEW)
            {
                weaponSwitchState = WeaponSwitchState.UP;
            }
        }

        // Handle moving the weapon socket position for the animated weapon switching
        if (weaponSwitchState == WeaponSwitchState.PUTDOWNPREVIOUS)
        {
            weaponMainLocalPosition = Vector3.Lerp(defaultWeaponPosition.localPosition,
                downWeaponPosition.localPosition, switchingTimeFactor);
        }
        else if (weaponSwitchState == WeaponSwitchState.PUTUPNEW)
        {
            weaponMainLocalPosition = Vector3.Lerp(downWeaponPosition.localPosition,
                defaultWeaponPosition.localPosition, switchingTimeFactor);
        }
    }

    void UpdateWeaponBob()
    {

        Vector3 playerCharacterVelocity = (playerCharacterController.transform.position - lastCharacterPosition) / Time.deltaTime;

        // calculate a smoothed weapon bob amount based on how close to our max grounded movement velocity we are
        float characterMovementFactor = 0f;
        if (playerCharacterController.isGrounded)
        {
            characterMovementFactor = Mathf.Clamp01(playerCharacterVelocity.magnitude / (playerCharacterController.maxSpeedOnGround * playerCharacterController.sprintSpeedModifier));
        }

        weaponBobFactor = Mathf.Lerp(weaponBobFactor, characterMovementFactor, weaponBobSharpness * Time.deltaTime);


        // Calculate vertical and horizontal weapon bob values based on a sine function
        float bobAmount = isAiming ? aimingBobAmount : defaultWeaponBobAmount;
        float hBobValue = Mathf.Sin(Time.time * weaponBobFrequency) * bobAmount * weaponBobFactor;
        float vBobValue = ((Mathf.Sin(Time.time * weaponBobFrequency * 2f) * 0.5f) + 0.5f) * bobAmount * weaponBobFactor;         
        
        // Apply weapon bob
        weaponBobLocalPosition.x = hBobValue;
        weaponBobLocalPosition.y = Mathf.Abs(vBobValue);

        lastCharacterPosition = playerCharacterController.transform.position;

    }

    public bool PickupWeapon(WeaponController weaponPrefab)
    {
        // search our weapon slots for the first free one, assign the weapon to it, and return true if we found one. Return false otherwise
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            // only add the weapon if the slot is free
            if (weaponSlots[i] == null)
            {

                // Set owner to this gameObject so the weapon can alter projectile/damage logic accordingly 
                weaponPrefab.ShowWeapon(false);

                // Assign the first person layer to the weapon
                int layerIndex = Mathf.RoundToInt(Mathf.Log(FPSWeapon.value, 2));
                numberOfWeapons++;
                weaponSlots[i] = weaponPrefab;

                if (OnAddedWeapon != null)
                {
                    OnAddedWeapon.Invoke(weaponPrefab, i);
                }

                return true;
            }
        }
        // Handle auto-switching to weapon if no weapons currently
        if (GetActiveWeapon() == null)
        {
            SwitchWeapon(true);
        }

        return false;
    }

    public bool AddWeapon(WeaponController weaponPrefab)
    {

        // search our weapon slots for the first free one, assign the weapon to it, and return true if we found one. Return false otherwise
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            // only add the weapon if the slot is free
            if (weaponSlots[i] == null)
            {
                // spawn the weapon prefab as child of the weapon socket
                WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParentSocket);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;
                numberOfWeapons++;

                // Set owner to this gameObject so the weapon can alter projectile/damage logic accordingly 
                weaponInstance.ShowWeapon(false);

                // Assign the first person layer to the weapon
                int layerIndex =   Mathf.RoundToInt(Mathf.Log(FPSWeapon.value,2));
                weaponSlots[i] = weaponInstance;

                if (OnAddedWeapon != null)
                {
                    OnAddedWeapon.Invoke(weaponInstance, i);
                }

                return true;
            }
        }
        // Handle auto-switching to weapon if no weapons currently
        if (GetActiveWeapon() == null)
        {
            SwitchWeapon(true);
        }

        return false;
    }

    public bool DropWeapon(WeaponController weaponInstance)
    {
        // Look through our slots for that weapon
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            // when weapon found, remove it
            if (weaponSlots[i] == weaponInstance)
            {
                weaponSlots[i] = null;
                if (OnRemovedWeapon != null)
                {
                    OnRemovedWeapon.Invoke(weaponInstance, i);
                }

                numberOfWeapons--;
                activeWeaponIndex = -1;

                // Handle case of removing active weapon (switch to next weapon)
                if (i == activeWeaponIndex)
                {
                    SwitchWeapon(true);
                }

                return true;
            }
        }

        return false;
    }

    public bool RemoveWeapon(WeaponController weaponInstance)
    {
        // Look through our slots for that weapon
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            // when weapon found, remove it
            if (weaponSlots[i] == weaponInstance)
            {
                weaponSlots[i] = null;

                if (OnRemovedWeapon != null)
                {
                    OnRemovedWeapon.Invoke(weaponInstance, i);
                }

                Destroy(weaponInstance.gameObject);
                numberOfWeapons--;
                activeWeaponIndex = -1;

                // Handle case of removing active weapon (switch to next weapon)
                if (i == activeWeaponIndex)
                {
                    SwitchWeapon(true);
                }

                return true;
            }
        }

        return false;
    }
}
