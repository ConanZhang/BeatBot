using UnityEngine;
using System.Collections;

/// <summary>
/// Creating instance of particles from code with no effort.
/// 
/// From: http://pixelnest.io/tutorials/2d-game-unity/particles/
/// </summary>
public class ExplodeManager: MonoBehaviour
{
    public static ExplodeManager Instance;
    
    public ParticleSystem explode;

    [SerializeField]
    private float moodScale;


    public float originalSpawnRate;
    private float spawnRate;

    private float spawnTime;

    public Vector3 minPosition;
    public Vector3 maxPosition;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SpecialEffectsHelper!");
        }
        
        Instance = this;
        spawnRate = originalSpawnRate;
        spawnTime = spawnRate;
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0)
        {
            Vector3 position = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z) );
            Explosion(position);

            if (spawnRate < originalSpawnRate * 0.33)
            {
                Vector3 position2 = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z) );
                Explosion(position2);
            }

            if (spawnRate < originalSpawnRate * 0.66)
            {
                Vector3 position3 = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z) );
                Explosion(position3);
            }


            spawnTime = spawnRate;
        }
    }

    /// <summary>
    /// Create an explosion at the given location. Size small.
    /// </summary>
    /// <param name="position"></param>
    void Explosion(Vector3 position)
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

    public void SetMoodScale(float moodScale)
    {
        this.moodScale = moodScale;

        if (moodScale > 0)
        {
            this.spawnRate = originalSpawnRate - (originalSpawnRate * moodScale);
        }
    }
}