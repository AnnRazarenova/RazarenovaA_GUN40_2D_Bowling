using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBall : MonoBehaviour
{
    public float thrust = 10f;
    
    private Rigidbody rb;
    private Camera playerCam;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
    }

    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.m))
    }

    private void TrowBall()
    {
        Vector3 direction = playerCam.transform.forward;
        rb.AddForce(direction *  thrust);
    }
}
