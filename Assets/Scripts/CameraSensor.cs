using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSensor : Sensor
{
    public float horizontal_fov = 50;
    public float vertical_fov = 50;
    public int horizontal_ray_count = 5;
    public int vertical_ray_count = 5;
    float width, height;
    public float far_plane = 100;
    public LayerMask rocketLayer;
    public override float[] GetData()
    {
        width = Mathf.Tan(horizontal_fov * Mathf.Deg2Rad);
        height = Mathf.Tan(vertical_fov * Mathf.Deg2Rad);
        float[] data = new float[horizontal_ray_count * vertical_ray_count];
        for (int i = 0; i < horizontal_ray_count; i++) {
            for(int k = 0; k < vertical_ray_count; k++)
            {
                RaycastHit hit = new RaycastHit();
                Vector3 rayDir = transform.forward + transform.right * (i - horizontal_ray_count / 2.0f) * width + transform.up * (k-vertical_ray_count/2.0f) * height;
                rayDir.Normalize();
                Debug.DrawRay(transform.position, rayDir * far_plane, Color.yellow);
                if (Physics.Raycast(transform.position, rayDir, out hit, far_plane,rocketLayer))
                    data[k * horizontal_ray_count + i] = hit.distance;
                else data[k * horizontal_ray_count + i] = -1;

            }
        }
        return data;
    }
}
