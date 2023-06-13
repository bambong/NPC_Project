using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserColorController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem startParticle;
    [SerializeField]
    private ParticleSystem endParticle;

    public void SetLaserColor(Color hitcolor)
    {
        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = startParticle.colorOverLifetime;
        Gradient gradient = colorOverLifetime.color.gradient;

        GradientColorKey[] colorKeys = gradient.colorKeys;
        int lastIndex = colorKeys.Length - 1;
        colorKeys[lastIndex].color = hitcolor;

        gradient.colorKeys = colorKeys;

        colorOverLifetime.color = gradient;
    }
}
