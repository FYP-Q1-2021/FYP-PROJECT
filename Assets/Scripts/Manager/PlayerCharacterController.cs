using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    // References
    [Header("References")]
    public Camera playerCamera;
    Transform playerBody;
    InputHandler inputHandler;
    CharacterController characterController;

    Vector3 velocity;

    [Tooltip("Affects the speed the player falls at")]
    public float gravity = -9.81f;

    [Header("Player Movement")]

    [Tooltip("Player base walking speed")]
    public float basePlayerSpeed = 12f;
    [Tooltip("Player base sprint speed modifier relative to base speed")]
    public float sprintSpeedModifier = 1.5f;
    [Tooltip("Affects the height in which the player will jump")]
    public float jumpStrength = 1.0f;

    [Header("Ground Check")]
    public Transform ground;
    public LayerMask groundMask;
    public float groundDistance = 0.3f;

    [Range(0f, 15f)]
    public float maxSpeedOnGround;

    public bool isGrounded;
    public bool isSprinting;

    float playerSpeed;
    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        characterController = GetComponent<CharacterController>();
        playerBody = GetComponent<Transform>();
        ground = GameObject.Find("GroundCheck").gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseLook();

        GroundCheck();

        bool isSprinting = inputHandler.GetSprintInputHeld();
        float speedModifier = isSprinting ? sprintSpeedModifier : 1f;
        playerSpeed = basePlayerSpeed * speedModifier;

        Vector3 worldspaceMoveInput = transform.TransformVector(inputHandler.GetMoveInput());
        characterController.Move(worldspaceMoveInput * playerSpeed * Time.deltaTime);

        if (isGrounded && inputHandler.GetJumpInputDown())
        {
            velocity.y = Mathf.Sqrt(jumpStrength * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(ground.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
    }

    void MouseLook()
    {
        float mouseX = inputHandler.GetLookInputsHorizontal();
        float mouseY = inputHandler.GetLookInputsVertical();

        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
