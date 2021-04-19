using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimator : MonoBehaviour
{

    public Transform item;
    [Header("Item Bob")]
    [Tooltip("Distance the item bobs")]
    public float itemBobAmount = 0.05f;
    [Tooltip("Frequency at which the item will move around")]
    public float itemBobFrequency = 10f;

    [Header("Item Rotation")]
    [Tooltip("Determines the speed of the item rotation on the Y Axis")]
    public float xRotationIntensity = 10f;
    [Tooltip("Determines the speed of the item rotation on the X Axis")]
    public float yRotationIntensity = 10f;
    [Tooltip("")]
    public float rotationSmoothness = 10f;

    Quaternion targetAdjustmentX;
    Quaternion targetAdjustmentY;
    [Tooltip("Enables rotation of item on the Y axis")]
    [SerializeField]
    bool yRotation;
    [Tooltip("Enables rotation of item on the X axis")]
    [SerializeField]
    bool xRotation;

    Quaternion originRotation;

    Vector3 localMainPosition;
    Vector3 localBobPosition;
    // Start is called before the first frame update

    void Start()
    {
        localMainPosition = transform.localPosition;
        originRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemAnimation();   
    }

    void UpdateItemAnimation()
    {
        targetAdjustmentX = Quaternion.identity;
        targetAdjustmentY = Quaternion.identity;
        // item bob
        float vBobValue = Mathf.Sin(Time.time * itemBobFrequency * 2f) * itemBobAmount;

        localBobPosition.y = Mathf.Abs(vBobValue);
        transform.localPosition = localMainPosition + localBobPosition;

        // item rotation
        if(xRotation)
        {
            targetAdjustmentX = Quaternion.AngleAxis(xRotationIntensity * Time.time, Vector3.right);
        }
        if(yRotation)
        {
            targetAdjustmentY = Quaternion.AngleAxis(yRotationIntensity * Time.time, Vector3.up);
        }
       
        Quaternion targetRotation = originRotation * targetAdjustmentX * targetAdjustmentY;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSmoothness);
    }

}
