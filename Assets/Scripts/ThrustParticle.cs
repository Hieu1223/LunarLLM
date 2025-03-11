using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustParticle : MonoBehaviour
{
    public ParticleSystem thrustParticle;

    public Thruster thruster;
    public float particleSpeed = 5;

    private void Update()
    {
        if(thruster.throttle <= 0)
            thrustParticle.Pause();
        if(thruster.throttle > 0)
        {
            thrustParticle.Play();
        }
    }
}
