﻿using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour
{
    private ParticleSystem ps ;
    // Use this for initialization
    void Start()
    {
        ps = this.gameObject.GetComponent<ParticleSystem>();
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
        var main = ps.main;
        main.startColor = Color.green;
        main.startSpeed = -5.0f;
        ps.Play();
        var emission = ps.emission; 
        emission.enabled = true;
    }

    public void EmitObstacleHitParticles()
    {
        var main = ps.main;
        main.startColor = Color.red;
        main.startSpeed = 5.0f;
        ps.Play();
        var emission = ps.emission;
        emission.enabled = true;
    }
}
