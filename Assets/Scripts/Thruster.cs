using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float force = 50;
    public float x_dir = 0, z_dir = 0, throttle = 0;
    public Rigidbody rb;
    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(x_dir, 0, z_dir);
        rb.AddForceAtPosition(transform.up * force * throttle, transform.position);
    }
}
