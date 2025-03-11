using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeSensor : Sensor
{
    public override float[] GetData()
    {
        float[] data = {-1};
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity))
            data[0] = hit.distance;
        return data;

    }
    private void Update()
    {
        Debug.DrawRay(transform.position, -Vector3.up * 8);
    }
}
