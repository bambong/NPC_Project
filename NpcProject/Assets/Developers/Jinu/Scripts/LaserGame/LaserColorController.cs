using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserColorController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem blueParticle;
    [SerializeField]
    private ParticleSystem yellowParticle;
    [SerializeField]
    private ParticleSystem purpleParticle;

    public void SetLaserColor(ParticleSystem baseParticle, Color hitcolor)
    {
        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = baseParticle.colorOverLifetime;
        Gradient gradient = colorOverLifetime.color.gradient;

        GradientColorKey[] colorKeys = gradient.colorKeys;

        gradient.colorKeys = colorKeys;

        colorOverLifetime.color = gradient;
    }
}
