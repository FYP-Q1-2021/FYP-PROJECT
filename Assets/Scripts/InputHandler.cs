using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [Tooltip("Sensitivity multiplier for moving the camera around")]
    public float lookSensitivity = 2f;
    [Tooltip("Limit to consider an input when using a trigger on a controller")]
    public float triggerAxisThreshold = 0.4f;

    bool m_FireInputWasHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        m_FireInputWasHeld = GetAttackInputHeld();
    }

    bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    public bool GetAttackInputDown()
    {
        return GetAttackInputHeld() && !m_FireInputWasHeld;
    }

    public bool GetAttackInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(0);
        }
        return false;
    }

    public bool GetAttackInputReleased()
    {
        return !GetAttackInputHeld() && m_FireInputWasHeld;
    }

    public bool GetAimInputHeld()
    {
        if (CanProcessInput())
        {
           
            bool i = Input.GetMouseButton(1);
            return i;
        }

        return false;
    }

    public bool GetDropKeyDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown("Drop");
        }
        return false;
    }

    public bool GetBlockInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(1);
        }
        return false;
    }

    public bool GetInteractKeyDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown("Interact");
        }
        return false;
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown("Jump");
        }

        return false;
    }

    public bool GetJumpInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton("Jump");
        }

        return false;

    }
    public bool GetSprintInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton("Sprint");
        }

        return false;
    }

    public int GetSelectWeaponInput()
    {
        if(CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                return 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                return 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                return 3;
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                return 4;
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                return 5;
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                return 6;
            else
                return 0;
        }
        return 0;
    }
    public Vector3 GetMoveInput()
    {

        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector3.ClampMagnitude(move, 1);
            //Debug.Log(move);
            return move;
        }

        return Vector3.zero;

    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseOrStickLookAxis("Mouse X", "Look X");
    }

    public float GetLookInputsVertical()
    {
        return GetMouseOrStickLookAxis("Mouse Y", "Look Y");
    }

    float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName)
    {
        if (CanProcessInput())
        {

            bool isGamepad = Input.GetAxis(stickInputName) != 0f;
            float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

            i *= lookSensitivity;
            if (isGamepad)
            {
                // since mouse input is already delt    aTime-dependant, only scale input with frame time if it's coming from sticks
                i *= Time.deltaTime;
            }
            else
            {
                // reduce mouse input amount to be equivalent to stick movement
                //i *= 0.01f;
            }

            return i;
        }
        return 0f;
    }



}
