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

    public float pickUpRange;
    public float dropForwardForce, dropUpWardForce;

    public static bool slotFull;

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
        fpsCam = GameObject.Find("PlayerCamera").GetComponent<Transform>();

        weaponController.enabled = true;
        rb.isKinematic = true;
        weaponCollider.isTrigger = true;

        //if(!weaponController.isWeaponActive)
        //{
        //    weaponController.enabled = false;
        //    rb.isKinematic = false;
        //    weaponCollider.isTrigger = false;
        //}
        //if (weaponController.isWeaponActive)
        //{


        //}
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
            if(selection.CompareTag(selectableTag) && Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
            }  
        }

        if(weaponController.isWeaponActive && Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    public void Pickup()
    {
        weaponController.isWeaponActive = true;

        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.tag = "Melee";
        rb.isKinematic = true;
        weaponCollider.isTrigger = true;

        weaponController.enabled = true;

        weaponsManager.PickupWeapon(weaponController);

    }

    public void Drop()
    {
        weaponController.isWeaponActive = false;
        weaponController.tag = "Pickup";
        // Set parent to null
        transform.SetParent(null);

        rb.isKinematic = false;
        weaponCollider.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpWardForce, ForceMode.Impulse);
        
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
        weaponController.enabled = false;

        weaponsManager.DropWeapon(weaponController);
    }
}
