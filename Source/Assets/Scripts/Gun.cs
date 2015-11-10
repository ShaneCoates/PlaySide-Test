using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Gun : MonoBehaviour {
    public Crosshair m_crosshair;
    public ParticleSystem m_particles;
    public float m_maxCooldown;
    private float m_cooldown;

	// Use this for initialization
	void Start () {
        m_cooldown = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_cooldown > 0)
        {
            m_cooldown -= Time.deltaTime;
        }

        if (m_cooldown <= 0)
        {
            Shoot();
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
    }
}
