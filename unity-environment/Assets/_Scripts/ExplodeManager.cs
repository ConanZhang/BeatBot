using UnityEngine;
using System.Collections;

/// <summary>
/// Creating instance of particles from code with no effort.
/// 
/// From: http://pixelnest.io/tutorials/2d-game-unity/particles/
/// </summary>
public class ExplodeManager: MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static ExplodeManager Instance;
    
    public ParticleSystem explode;

    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SpecialEffectsHelper!");
        }
        
        Instance = this;
    }

    /// <summary>
    /// Create an explosion at the given location. Size small.
    /// </summary>
    /// <param name="position"></param>
    public void Explosion(Vector3 position)
    {
        instantiate(explode, position);

    }

    /// <summary>
    /// Instantiate a Particle system from prefab
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as ParticleSystem;
		
		// Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);
		
        return newParticleSystem;
    }
}