using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour
{
    private ParticleSystem particleSystem;
    // Use this for initialization
    void Start()
    {
        particleSystem = this.gameObject.GetComponent<ParticleSystem>();
        CoreSystem.onSoundEvent += EmitSoundPickupParticles;
        CoreSystem.onObstacleEvent += EmitObstacleHitParticles;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        CoreSystem.onSoundEvent -= EmitSoundPickupParticles;
        CoreSystem.onObstacleEvent -= EmitObstacleHitParticles;
    }

    public void EmitSoundPickupParticles()
    {
        particleSystem.startColor = Color.green;
        particleSystem.Play();
        var emission = particleSystem.emission; 
        emission.enabled = true;
    }

    public void EmitObstacleHitParticles()
    {
        particleSystem.startColor = Color.red;
        particleSystem.Play();
        var emission = particleSystem.emission;
        emission.enabled = true;
    }
}
