using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lander : MonoBehaviour
{
    public List<Thruster> thrusters = new List<Thruster>();
    public List<Directionalthrus> rotators= new List<Directionalthrus>();
    public List<Sensor> sensors = new List<Sensor>();
    public Rigidbody rb;
    public struct State
    {
        public Vector3 pos;
        public Vector3 eulerRot;
        public Vector3 vel;
        public List<ThrusterThrottle> thrusters_throttle;
        public List<RotatorThrottle> rotator_throttle;
        public List<float> sensor_data;
    }

    [Serializable]
    public struct ThrottleInput
    {
        public List<ThrusterThrottle> thrusters_throttle;
        public List<RotatorThrottle> rotator_throttle;
    }
    [Serializable]
    public struct ThrusterThrottle
    {
        public float throttle;
        public float x_dir;
        public float z_dir;
    }

    [Serializable]
    public struct RotatorThrottle
    {
        public float x_dir;
        public float z_dir;
    }

    public void ApplyThrottleInput(ThrottleInput input)
    {
        for (int i = 0; i < thrusters.Count && i < input.thrusters_throttle.Count; i++) {
            thrusters[i].throttle = Mathf.Clamp(input.thrusters_throttle[i].throttle, 0, 1);
            thrusters[i].x_dir = Mathf.Clamp(input.thrusters_throttle[i].x_dir, -15, 15);
            thrusters[i].z_dir = Mathf.Clamp(input.thrusters_throttle[i].z_dir, -15, 15);
        }
        for (int i = 0; i < rotators.Count && i < input.rotator_throttle.Count; i++)
        {
            rotators[i].x_dir_throttle = Mathf.Clamp(input.rotator_throttle[i].x_dir, -1, 1);
            rotators[i].z_dir_throttle = Mathf.Clamp(input.rotator_throttle[i].z_dir, -1, 1);
        }
    }

    public State GetState()
    {
        State state = new State();
        state.thrusters_throttle = new List<ThrusterThrottle>();
        state.sensor_data = new List<float>();
        state.rotator_throttle = new List<RotatorThrottle>();
        state.vel = rb.velocity;
        state.pos = rb.position;
        state.eulerRot = rb.rotation.eulerAngles;


        foreach(Sensor sensor in sensors)
        {
            state.sensor_data.AddRange(sensor.GetData());
        }

        foreach (Thruster thruster in thrusters) {
            ThrusterThrottle thrusterThrottle = new ThrusterThrottle();
            thrusterThrottle.throttle = thruster.throttle;
            thrusterThrottle.x_dir = thruster.x_dir;
            thrusterThrottle.z_dir = thruster.z_dir;
            state.thrusters_throttle.Add(thrusterThrottle);
        }
        foreach (Directionalthrus directionalthrus in rotators)
        {
            RotatorThrottle throttle = new RotatorThrottle();
            throttle.x_dir = directionalthrus.x_dir_throttle;
            throttle.z_dir = directionalthrus.z_dir_throttle;

            state.rotator_throttle.Add(throttle);
        }
        return state;
    }
}
