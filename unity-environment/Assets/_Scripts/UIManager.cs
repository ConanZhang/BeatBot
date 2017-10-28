using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public ParticleSystem ps;
    public ParticleSystem.EmissionModule ps_emission;

    public List<ParticleSystem> burstParticles;

    public float testVal = 0.0f;

    void Start ()
    {
        ps_emission = ps.emission;
	}

    public void SetBounceRate(float rate)
    {
        rate = Mathf.Clamp(rate, 0.05f, 1.0f);

        ps_emission.rateOverTime = rate * 100.0f;

        var psShape = ps.shape;
        psShape.arcSpeed = rate * 1.0f;
    }
    public void SetArcLength(float arcLength, float orientation)
    {
        var psShape = ps.shape;
        psShape.arc = arcLength;
        psShape.rotation = new Vector3(0f, 0f, orientation);
    }

    public void SetBurstRate(float rate)
    {
        rate = Mathf.Clamp(rate, 0.05f, 1.0f);
        foreach (var effect in burstParticles)
        {
            var main = effect.main;
            main.simulationSpeed = rate;
        }
    }
	
	void Update ()
    {
        SetBounceRate(testVal);
        SetBurstRate(testVal);
    }
}
