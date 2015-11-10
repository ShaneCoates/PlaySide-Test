﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
public class Gun : MonoBehaviour {
    public Crosshair m_crosshair;
    public ParticleSystem m_particles;
    public float m_maxCooldown;
    private float m_cooldown;
    public Image m_cooldownImage;
    private bool m_shooting;
	// Use this for initialization
	void Start () {
        m_cooldown = 0.0f;
        m_shooting = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_cooldownImage.fillAmount = m_cooldown / m_maxCooldown;
        if (m_cooldown > 0)
        {
            m_cooldown -= Time.deltaTime;
        }

        if (m_cooldown <= 0 && !m_shooting)
        {
            
            if (m_crosshair.m_targettedObject != null)
            {
                Invoke("Shoot", 0.2f);
                m_shooting = true;
            }
                
        }
	}
    void Shoot()
    {
        //if there is a target, shoot towards it, otherwise shoot in a general forward direction
        //this is to make sure we are on target when the crosshair is moving around screen
        if (m_crosshair.m_targettedObject != null)
        {
            m_crosshair.m_targettedObject.Hit();
            m_cooldown = m_maxCooldown;
            m_particles.Emit(15);
        }
        m_shooting = false;
    }
}
