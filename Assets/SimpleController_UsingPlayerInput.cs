using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

// Use a separate PlayerInput component for setting up input.
public class SimpleController_UsingPlayerInput : MonoBehaviour
{
    public float rotateSpeed;
    public float burstSpeed;

    private bool m_Charging;
    private Vector2 m_Rotation;
    private Vector2 m_Look;

    private void Awake()
    {
        //hook up the Input Control delegates
        GetComponent<PlayerInput>().onActionTriggered += HandleEvents;
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        m_Look = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        Look(m_Look);
    }

    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        m_Rotation.y += rotate.x * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = m_Rotation;
    }

    private void HandleEvents(InputAction.CallbackContext context)
    {
        string actionName = context.action.name;
        switch(actionName)
        {
            case"TestWin":
            break;
            case"TestLose":
            break;
            case"Pause":
            Debug.Log("Pausing Game!");
            break;
            case"PlayFishing":
            break;
            case"ResetToMenu":
            break;

        }
    }

}
