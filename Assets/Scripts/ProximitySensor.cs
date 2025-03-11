using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensor : Sensor
{
    public Rigidbody rb;
    public override float[] GetData()
    {
        float[] data = new float[9];

        data[0] = transform.position.x;
        data[1] = transform.position.y;
        data[2] = transform.position.z;

        data[3] = transform.rotation.eulerAngles.x;
        data[4] = transform.rotation.eulerAngles.y;
        data[5] = transform.rotation.eulerAngles.z;

        data[6] = rb.velocity.x;
        data[7] = rb.velocity.y;
        data[8] = rb.velocity.z;



        return data;
    }
}
