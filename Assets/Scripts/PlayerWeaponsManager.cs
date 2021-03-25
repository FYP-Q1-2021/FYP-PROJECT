using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{

    [Tooltip("List of weapon the player will start with")]
    public List<WeaponController> startingWeapons = new List<WeaponController>();

    public Transform defaultWeaponPosition;
    public Transform aDSWeaponPosition;
    public Transform downWeaponPosition;
    public Transform weaponParentSocket;
    Vector3 weaponParentOrigin;
    PlayerCharacterController playerCharacterController;
    InputHandler inputHandler;

    [Header("Weapon Bob")]
    [Range(0f,1f)][Tooltip("Intensity of weapon bob when player is still")]
    public float idleWeaponBobIntensity;
    [Range(0f, 15f)]
    [Tooltip("Smoothness of weapon bob")]
    public float idleWeaponBobSmoothness;
    [Range(0f, 15f)]
    [Tooltip("Affects the frequency in which the weapon bobs")]
    public float idleWeaponBobSpeed;

    [Range(0f, 1f)]
    [Tooltip("Intensity of weapon bob when player is moving")]
    public float movingWeaponBobIntensity;
    [Range(0f, 15f)]
    [Tooltip("Smoothness of weapon bob")]
    public float movingWeaponBobSmoothness;
    [Range(0f, 15f)]
    [Tooltip("Affects the frequency in which the weapon bobs")]
    public float movingWeaponBobSpeed;

    [Range(0f, 1f)]
    [Tooltip("Intensity of weapon bob when player is sprinting")]
    public float sprintingWeaponBobIntensity;
    [Range(0f, 15f)]
    [Tooltip("Smoothness of weapon bob")]
    public float sprintingWeaponBobSmoothness;
    [Range(0f, 15f)]
    [Tooltip("Affects the frequency in which the weapon bobs")]
    public float sprintingWeaponBobSpeed;


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
        

        foreach (var weapon in startingWeapons)
        {
            Debug.Log("Hi");
            AddWeapon(weapon);
        }
        SwitchWeapon(true);
    }

    void Update()
    {
        GetActiveWeapon();
        int selectWeaponInput = inputHandler.GetSelectWeaponInput();
        if (selectWeaponInput != 0)
        {
            if (GetWeaponAtIndex(selectWeaponInput - 1) != null)
            {
                SwitchToWeaponAtIndex(selectWeaponInput - 1);
            }
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

    void SwitchToWeaponAtIndex(int weaponAtIndex)
    {
        if(weaponAtIndex >= 0 && weaponAtIndex != activeWeaponIndex)
        {
            newWeaponIndex = weaponAtIndex;

            if(GetActiveWeapon() != null)
            {
                weaponSlots[activeWeaponIndex].ShowWeapon(false);

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

    void LateUpdate()
    {
        UpdateWeaponBob();
        
        weaponParentSocket.localPosition = Vector3.Lerp(weaponParentSocket.localPosition, targetWeaponBobPosition, targetWeaponBobSmoothness);
    }


    void UpdateWeaponSwitching()
    {

    }

    void UpdateWeaponBob()
    {
        Vector3 moveInput = inputHandler.GetMoveInput();
        if (moveInput.x == 0 && moveInput.z == 0)
        {
            //UpdateWeaponBob(idleCounter, idleWeaponBobIntensity, idleWeaponBobIntensity);
            idleCounter += Time.deltaTime * idleWeaponBobSpeed;
            targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(idleCounter) * idleWeaponBobIntensity, Mathf.Sin(idleCounter * 2) * idleWeaponBobIntensity, 0);
            targetWeaponBobSmoothness = Time.deltaTime * idleWeaponBobSmoothness;
            
        }
        else if(playerCharacterController.isSprinting)
        {
            sprintCounter += Time.deltaTime * sprintingWeaponBobSpeed;
            targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(sprintCounter) * sprintingWeaponBobIntensity, Mathf.Sin(sprintCounter * 2) * sprintingWeaponBobIntensity, 0);
            targetWeaponBobSmoothness = Time.deltaTime * sprintingWeaponBobSmoothness;
        }
        else
        {
            //UpdateWeaponBob(movementCounter, movingWeaponBobIntensity, movingWeaponBobIntensity);
            movementCounter += Time.deltaTime * movingWeaponBobSpeed;
            targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(movementCounter) * idleWeaponBobIntensity, Mathf.Sin(movementCounter * 2) * idleWeaponBobIntensity, 0);
            targetWeaponBobSmoothness = Time.deltaTime * movingWeaponBobSmoothness;
        }
        
    }

    void SwitchWeapon(bool ascendingOrder)
    {
        int newWeaponIndex = -1;
        for(int i = 0; i < weaponSlots.Length; i++)
        {
            if(GetWeaponAtIndex(i) != null && i != activeWeaponIndex)
            {
                newWeaponIndex = i;
            }
        }
        SwitchToWeaponAtIndex(newWeaponIndex);
    }

    public bool AddWeapon(WeaponController weaponPrefab)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                // spawn the weapon prefab as child of the weapon socket
                WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParentSocket);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;
                weaponInstance.ShowWeapon(false);

                weaponSlots[i] = weaponInstance;
                return true;
            }
        }

        if(GetActiveWeapon() == null)
        {
            SwitchWeapon(true);
        }
        return false;
    }




    bool RemoveWeapon()
    {
        return false;
    }
}
