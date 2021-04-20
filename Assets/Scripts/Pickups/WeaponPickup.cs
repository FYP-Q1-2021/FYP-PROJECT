using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    PlayerWeaponsManager weaponsManager;
    WeaponController weaponController;
    BoxCollider weaponCollider;
    Rigidbody rb;
    Transform player, gunContainer, fpsCam;
    InputHandler inputHandler;

    [Header("Information")]
    [Tooltip("The range where it can be interacted")]
    public float pickUpRange;
    [Tooltip("The forward force when weapon is thrown")]
    public float dropForwardForce;
    [Tooltip("The upward force when weapon is thrown")]
    public float dropUpWardForce;

    [SerializeField] string selectableTag = "Pickup";

    // Start is called before the first frame update
    void Start()
    {
        weaponController = GetComponent<WeaponController>();
        rb = GetComponent<Rigidbody>();
        weaponCollider = GetComponent<BoxCollider>();
        gunContainer = GameObject.Find("WeaponParentSocket").GetComponent<Transform>();
        weaponsManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        inputHandler = GameObject.Find("Player").GetComponent<InputHandler>();
        fpsCam = GameObject.Find("PlayerCamera").GetComponent<Transform>();

        weaponController.enabled = true;
        rb.isKinematic = true;
        weaponCollider.isTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, pickUpRange))
        {
            var selection = hit.transform;
            if(selection.CompareTag(selectableTag) && inputHandler.GetInteractKeyDown())
            {
                Pickup();
            }  
        }

        if(weaponController.isWeaponActive && inputHandler.GetWeaponDropKeyDown())
        {
            Drop();
        }
    }

    public void Pickup()
    {
        weaponCollider.isTrigger = true;
        weaponController.enabled = true;
        weaponController.isWeaponActive = true;

        gameObject.tag = "Melee";

        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;

        weaponsManager.PickupWeapon(weaponController);
    }

    public void Drop()
    {
        weaponController.isWeaponActive = false;
        weaponCollider.isTrigger = false;

        weaponsManager.DropWeapon(weaponController);
        weaponController.enabled = false;

        gameObject.tag = "Pickup";
        // Set parent to null
        transform.SetParent(null);

        rb.isKinematic = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpWardForce, ForceMode.Impulse);
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

    }
}
