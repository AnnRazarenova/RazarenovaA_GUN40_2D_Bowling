using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(TrajectoryDrawer))]
public class PushBall : MonoBehaviour
{
    [Header("Настройка силы броска шара")]
    [SerializeField, Tooltip("Минимальная сила броска")]
    private float minThrust = 5f;
    [SerializeField, Tooltip("Максимальная сила броска")]
    private float maxThrust = 15f;
    private float currentThrust = 10f;
    [SerializeField, Tooltip("Скорость изменения силы броска")]
    private float thrustSpeedChange = 1f;
    
    private Rigidbody rb;
    private Camera playerCam;
    private TrajectoryDrawer trajectoryDrawer;

    private bool isChanging = false;
    private bool increasing = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;

        trajectoryDrawer = GetComponent<TrajectoryDrawer>();
    }

    private void FixedUpdate()
    {
        if (isChanging)
        {
            UpdateThrust();

            trajectoryDrawer.DrawTrajectory();
        }
    }

    private void OnThrowBall(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartChanging();
        }
        else if (context.canceled)
        {
            ThrowBall();
        }
    }

    private void StartChanging()
    {
        isChanging = true;
        currentThrust = minThrust;
        increasing = true;

        trajectoryDrawer.ShowTrajectory();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void UpdateThrust()
    {
        if (increasing)
        {
            currentThrust += thrustSpeedChange * Time.fixedDeltaTime;

            if (currentThrust < maxThrust)
            {
                currentThrust = maxThrust;
                increasing = false;
            }
        }
        else
        {
            currentThrust -= thrustSpeedChange * Time.fixedDeltaTime;

            if (currentThrust < minThrust)
            {
                currentThrust = minThrust;
                increasing = true;
            }
        }
    }

    private void ThrowBall()
    {
        isChanging = false;

        trajectoryDrawer.ShowTrajectory();

        Vector3 direction = playerCam.transform.forward;
        rb.AddForce(direction *  currentThrust);
        //rb.AddForce(direction *  currentThrust, ForceMode.Impulse);
    }
}
