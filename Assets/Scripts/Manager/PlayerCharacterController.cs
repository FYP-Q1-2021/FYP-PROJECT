using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    // References
    [Header("References")]
    public Camera playerCamera;
    [Tooltip("Audio source for footsteps, jump, etc...")]
    public AudioSource AudioSource;
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
    public float buffSpeedModifier = 1.25f;

    [Header("Ground Check")]
    public Transform ground;
    public LayerMask groundMask;
    public float groundDistance = 0.3f;

    [Header("Audio")]
    [Tooltip("Amount of footstep sounds played when moving one meter")]
    public float FootstepSfxFrequency = 1f;
    [Tooltip("Amount of footstep sounds played when moving one meter while sprinting")]
    public float FootstepSfxFrequencyWhileSprinting = 1f;
    [Tooltip("Sound played for footsteps")]
    public AudioClip FootstepSfx;
    [Tooltip("Sound played when jumping")] 
    public AudioClip JumpSfx;
    [Tooltip("Sound played when landing")] 
    public AudioClip LandSfx;

    [Range(0f, 15f)]
    public float maxSpeedOnGround;

    public bool isGrounded;
    public bool isSprinting;
    public bool isSpeedBuffed;
    public bool isDamageBuffed;
    public bool isAtackSpeedBuffed;
    public bool isInvincible;

    float m_FootstepDistanceCounter;

    [Range(0f,90f)]
    [Tooltip("Locks the rotation to prevent going over when looking down")]
    public float bottomCameraLock;
    [Range(0f, -90f)]
    [Tooltip("Locks the rotation to prevent going over when looking up")]
    public float upCameraLock;
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
        bool wasGrounded = isGrounded;
        MouseLook();

        GroundCheck();

        bool isSprinting = inputHandler.GetSprintInputHeld();
        float speedModifier = isSprinting ? sprintSpeedModifier : 1f;
        speedModifier = isSpeedBuffed ? buffSpeedModifier : speedModifier;
        playerSpeed = basePlayerSpeed * speedModifier;

        Vector3 worldspaceMoveInput = transform.TransformVector(inputHandler.GetMoveInput());
        characterController.Move(worldspaceMoveInput * playerSpeed * Time.deltaTime);

        if (isGrounded && inputHandler.GetJumpInputDown())
        {
            velocity.y = Mathf.Sqrt(jumpStrength * -2f * gravity);
            // play sound
            AudioSource.PlayOneShot(JumpSfx);
        }

        // landing
        if (isGrounded && !wasGrounded)
        {
            AudioSource.PlayOneShot(LandSfx);
        }

        // footsteps sound
        float chosenFootstepSfxFrequency =
            (isSprinting ? FootstepSfxFrequencyWhileSprinting : FootstepSfxFrequency);
        if (m_FootstepDistanceCounter >= 1f / chosenFootstepSfxFrequency)
        {
            m_FootstepDistanceCounter = 0f;
            AudioSource.PlayOneShot(FootstepSfx);
        }

        // keep track of distance traveled for footsteps sound
        m_FootstepDistanceCounter += velocity.magnitude * Time.deltaTime;

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
        xRotation = Mathf.Clamp(xRotation, upCameraLock, bottomCameraLock);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void Buff(Powerup buff, bool flag)
    {
        if(buff == Powerup.DAMAGE)
        {
            isDamageBuffed = flag;
        }
        else if(buff == Powerup.MOVEMENT_SPEED)
        {
            isSpeedBuffed = flag;
        }
        else if(buff == Powerup.INVINCIBLE)
        {
            isInvincible = flag;
        }
    }
}
