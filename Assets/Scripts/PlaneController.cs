using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrottle = 200f;
    public float responsiveness = 10f;
    private float responseModifier
    {
        get
        {
            return (rb.mass/10f) * responsiveness;
        }
    }
    private float roll;
    private float pitch;
    private float yaw;
    private float throttle;

    private InputAction rollAction;
    private InputAction pitchAction;
    private InputAction yawAction;
    private InputAction throttleAction;
    private InputAction breakAction;

    private Rigidbody rb;
    private void HandleInput()
    {
        roll = rollAction.ReadValue<float>();
        pitch = pitchAction.ReadValue<float>();
        yaw = yawAction.ReadValue<float>();
        if (throttleAction.IsPressed()) throttle += throttleIncrement;
        if (breakAction.IsPressed()) throttle -= throttleIncrement;

        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void OnEnable()
    {
        InputSystem.actions.FindActionMap("Plane").Enable();
    }

    private void OnDisable()
    {
        InputSystem.actions.FindActionMap("Plane").Disable();
    }
    private void Awake()
    {
        var planeMap = InputSystem.actions.FindActionMap("Plane");

        rollAction = planeMap.FindAction("Roll");
        pitchAction = planeMap.FindAction("Pitch");
        yawAction = planeMap.FindAction("Yaw");
        throttleAction = planeMap.FindAction("Throttle");
        breakAction = planeMap.FindAction("Break");

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
        Debug.Log("roll: " + roll + " pitch: " + pitch + " yaw: " + yaw);
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * throttle * maxThrottle);
        rb.AddTorque(transform.right * -pitch * responseModifier);
        rb.AddTorque(transform.forward * -roll * responseModifier);
        rb.AddTorque(transform.up * yaw * responseModifier);
    }


}
