using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directionalthrus : MonoBehaviour
{
    public float force = 50;
    public float x_dir_throttle = 0;
    public float z_dir_throttle = 0;

    public Rigidbody body;
    private void FixedUpdate()
    {
        body.AddForceAtPosition(force * (x_dir_throttle * transform.right + z_dir_throttle * transform.forward), transform.position);
    }
}
