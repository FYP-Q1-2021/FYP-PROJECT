using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldDown : MonoBehaviour
{

    public PlayerCharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GameObject.Find("Player").GetComponent<PlayerCharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(characterController.hasKey)
        {
            gameObject.SetActive(false);
        }
    }
}
