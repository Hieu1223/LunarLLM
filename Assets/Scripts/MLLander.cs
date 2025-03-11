using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MLLander : Agent
{
    public Lander lander;
    public Transform landingSite;
    int rotator_count = 0;
    int thrust_count = 0;
    public Vector3 spawnRangeStart;
    public Vector3 spawnRangeEnd;
    float time = 0;
    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(Random.Range(spawnRangeStart.x, spawnRangeEnd.x),
                                         Random.Range(spawnRangeStart.y, spawnRangeEnd.y),
                                         Random.Range(spawnRangeStart.z, spawnRangeEnd.z));
        lander.rb.velocity = Vector3.zero;
    }
    private void Start()
    {
        var state = lander.GetState();
        Debug.Log("Sensor count: " +  state.sensor_data.Count);
        Debug.Log("Action count: " +  (state.rotator_throttle.Count *2  + state.thrusters_throttle.Count *3));
        rotator_count = state.rotator_throttle.Count;
        thrust_count = state.thrusters_throttle.Count;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Lander.State state = lander.GetState();
        sensor.AddObservation(state.sensor_data);
    }

    public float AtanClamp(float val)
    {
        return Mathf.Atan(val) / Mathf.PI + 1 / 2;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        Lander.ThrottleInput input = new Lander.ThrottleInput();
        input.thrusters_throttle = new List<Lander.ThrusterThrottle>();
        input.rotator_throttle = new List<Lander.RotatorThrottle>();

        int count = 0;

        for (int i = 0; i < thrust_count; i++) {
            Lander.ThrusterThrottle thrusterThrottle = new Lander.ThrusterThrottle();
            thrusterThrottle.x_dir = actions.ContinuousActions[count++] * 15;
            thrusterThrottle.z_dir = actions.ContinuousActions[count++] * 15;
            thrusterThrottle.throttle = actions.ContinuousActions[count++];
            input.thrusters_throttle.Add(thrusterThrottle);
        }

        for (int i = 0; i < rotator_count; i++)
        {
            Lander.RotatorThrottle rotatorThrottle = new Lander.RotatorThrottle();
            rotatorThrottle.x_dir = actions.ContinuousActions[count++];
            rotatorThrottle.z_dir = actions.ContinuousActions[count++];
            input.rotator_throttle.Add(rotatorThrottle);
        }
        lander.ApplyThrottleInput(input);

    }
    private void Update()
    {
        time += Time.deltaTime;
        if(time > 5)
        {
            SetReward(-1);
            EndEpisode();
            return;
        }
        if(transform.position.y > 200)
        {
            AddReward(-1);
            EndEpisode();
            return;
        }
        if(Mathf.Abs(transform.position.x) > 200)
        {
            AddReward(-1);
            EndEpisode();
            return;
        }
        if (Mathf.Abs(transform.position.z) > 200)
        {
            AddReward(-1);
            EndEpisode();
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "LandingSite")
        {
            Debug.Log("Touched");
            SetReward(10);
            EndEpisode();
            return;
        }
        else
        {
            SetReward(-1);
            EndEpisode();
        }
    }
}
