using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponType
{
    RANGED,
    MELEE
};

public class WeaponController : MonoBehaviour
{
    public InputHandler inputHandler;
    public string weaponName;
    // References

    WeaponType weaponType;

    public float swayIntensity;
    public float swaySmoothness;

    Quaternion originRotation;


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

    void LateUpdate()
    {
        
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


    void ShowWeapon()
    {

    }
}
