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

    public Transform defaultWeaponPosition;
    public Transform aDSWeaponPosition;
    public Transform downWeaponPosition;
    public Transform weaponParentSocket;

    PlayerCharacterController playerCharacterController;
    InputHandler inputHandler;

    WeaponSwitchState weaponSwitchState;
    [Header("Weapon Bob")]
    [Range(0f, 1f)]
    public float weaponBobAmount = 0.05f;
    [Range(0f, 10f)]
    public float weaponBobFrequency = 10f;
    [Range(0f, 10f)]
    public float weaponBobSharpness = 10f;
    float weaponBobFactor;


    [Header("Misc")]
    [Tooltip("Speed at which the aiming animatoin is played")]
    public float aimingAnimationSpeed = 10f;

    [Tooltip("Delay before switching weapon a second time, to avoid recieving multiple inputs from mouse wheel")]
    public float weaponSwitchDelay = 1f;

    public UnityAction<WeaponController> OnSwitchedToWeapon;
    public UnityAction<WeaponController, int> OnAddedWeapon;
    public UnityAction<WeaponController, int> OnRemovedWeapon;

    Vector3 lastCharacterPosition;
    Vector3 weaponBobLocalPosition;
    Vector3 weaponMainLocalPosition;
    Vector3 weaponParentOrigin;
    float timeStartedWeaponSwitch;
    int weaponSwitchNewWeaponIndex;

    Vector3 targetWeaponBobPosition;
    float targetWeaponBobSmoothness;
    float idleCounter;
    float movementCounter;
    float sprintCounter;


    int activeWeaponIndex;
    int newWeaponIndex;

    WeaponController[] weaponSlots = new WeaponController[9]; // 9 available weapon slots

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerCharacterController = GetComponent<PlayerCharacterController>();
        weaponParentOrigin = weaponParentSocket.localPosition;
        activeWeaponIndex = -1;
        weaponSwitchState = WeaponSwitchState.DOWN;
        OnSwitchedToWeapon += OnWeaponSwitched;

        foreach (var weapon in startingWeapons)
        {
            Debug.Log("Hi");
            AddWeapon(weapon);
        }
        SwitchWeapon(true);
    }

    void Update()
    {
        // shoot handling
        WeaponController activeWeapon = GetActiveWeapon();
        if (weaponSwitchState == WeaponSwitchState.DOWN)
            Debug.Log("LMAO");
        // weapon switch handling
        if ((weaponSwitchState == WeaponSwitchState.UP || weaponSwitchState == WeaponSwitchState.DOWN))
        {
            int switchWeaponInput = inputHandler.GetSelectWeaponInput();
            

            if (switchWeaponInput != 0)
            {
                if (GetWeaponAtIndex(switchWeaponInput - 1) != null)
                    SwitchToWeaponAtIndex(switchWeaponInput - 1);
            }
            
        }
    }

    void LateUpdate()
    {
        UpdateWeaponBob();
        UpdateWeaponAiming();
        UpdateWeaponSwitching();

        weaponParentSocket.localPosition = weaponMainLocalPosition + weaponBobLocalPosition;
        //weaponParentSocket.localPosition = Vector3.Lerp(weaponParentSocket.localPosition, targetWeaponBobPosition, targetWeaponBobSmoothness);
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
        Debug.Log("Enter");
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
            activeWeaponIndex = weaponAtIndex;
            weaponSlots[weaponAtIndex].ShowWeapon(true);

        }
        else if (weaponAtIndex == activeWeaponIndex)
        {
            weaponSlots[weaponAtIndex].ShowWeapon(false);
            activeWeaponIndex = -1;
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
        //if(weaponSwitchState == WeaponSwitchState.UP)
        weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition,defaultWeaponPosition.localPosition, aimingAnimationSpeed * Time.deltaTime);
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
                    Debug.Log("EnterTwo");
                    oldWeapon.ShowWeapon(false);
                }

                Debug.Log(weaponSwitchNewWeaponIndex);
                
                activeWeaponIndex = weaponSwitchNewWeaponIndex;
                switchingTimeFactor = 0f;
                Debug.Log(activeWeaponIndex);
                // Activate new weapon
                WeaponController newWeapon = GetWeaponAtIndex(activeWeaponIndex);
                //Debug.Log(activeWeaponIndex);
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
        float hBobValue = Mathf.Sin(Time.time * weaponBobFrequency) * weaponBobAmount * weaponBobFactor;
        float vBobValue = ((Mathf.Sin(Time.time * weaponBobFrequency * 2f) * 0.5f) + 0.5f) * weaponBobAmount * weaponBobFactor;

        // Apply weapon bob
        weaponBobLocalPosition.x = hBobValue;
        weaponBobLocalPosition.y = Mathf.Abs(vBobValue);

        lastCharacterPosition = playerCharacterController.transform.position;

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

                // Set owner to this gameObject so the weapon can alter projectile/damage logic accordingly 
                weaponInstance.ShowWeapon(false);

                // Assign the first person layer to the weapon

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
