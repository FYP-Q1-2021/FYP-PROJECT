using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCarry : MonoBehaviour
{

    public InputHandler inputHandler;
    public Camera playerCamera;
    public PlayerWeaponsManager playerWeaponsManager;

    GameObject carriedObject;
    public bool carryingObject;
    [Tooltip("Adjusts the smoothness of picking up the object")]
    [SerializeField]
    float smooth;
    [Tooltip("Distance of the object from the player when picked up")]
    [SerializeField]
    float distance;

    [Tooltip("Force when thrown")]
    public float throwUpwardForce = 4f;
    [Tooltip("Force when thrown")]
    public float throwDownwardForce = 4f;
    [Tooltip("The range the player would need from the object to interact")]
    public float pickupRange = 4f;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        inputHandler = gameObject.GetComponent<InputHandler>();
        playerWeaponsManager = gameObject.GetComponent<PlayerWeaponsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(carryingObject)
        {
            CarryObject(carriedObject);
            if (inputHandler.GetInteractKeyDown())
            {
                DropObject();
            }
            if(inputHandler.GetDropKeyDown())
            {
                ThrowObject();
            }
        }
        else if(!carryingObject)
        {

            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            if (inputHandler.GetInteractKeyDown() && Physics.Raycast(ray, out hit, pickupRange))
            {
                PickupObject(hit);
            }
        }
    }

    void CarryObject(GameObject o)
    {
        o.transform.position = Vector3.Lerp(o.transform.position, playerCamera.transform.position + playerCamera.transform.forward * distance, Time.deltaTime * smooth);
    }

    void ThrowObject()
    {
        carryingObject = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * throwUpwardForce, ForceMode.Impulse);
        carriedObject.gameObject.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * throwDownwardForce, ForceMode.Impulse);
        float random = Random.Range(-1f, 1f);
        carriedObject.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(random, random, random));
        carriedObject = null;
    }

    void PickupObject(RaycastHit hit)
    {
        Pickupable p = hit.collider.GetComponent<Pickupable>();
        if (p != null)
        {
            carryingObject = true;
            carriedObject = p.gameObject;
            p.gameObject.GetComponent<Rigidbody>().useGravity = false;
            p.gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        }
    }

    void DropObject()
    {
        carryingObject = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        carriedObject = null;

    }
}
