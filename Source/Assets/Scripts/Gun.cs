using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
public class Gun : MonoBehaviour {

    public ParticleSystem m_particles;

    public Crosshair m_crosshair;
    private float m_cooldown;
    public Image m_cooldownImage;

    public float m_maxCooldown;
    public float m_maxReloadTime;

    private bool m_shooting;
    private Enemy m_lastTarget;

    private AudioSource m_source;
	// Use this for initialization
	void Start () {
        m_cooldown = 0.0f;
        m_shooting = false;
        m_source = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_cooldown > 0) m_cooldown -= Time.deltaTime;
    
        CheckForTarget();

        m_cooldownImage.fillAmount = m_cooldown / m_maxCooldown;
	}

    void CheckForTarget()
    {
        //if we're not already shooting, and there's no cooldown - check if there's something targetted
        if (m_cooldown <= 0 && !m_shooting)
        {
            if (m_crosshair.m_targettedObject != null)
            {
                m_lastTarget = m_crosshair.m_targettedObject;
                Invoke("Shoot", 0.01f);
                m_shooting = true;
            }
        }
    }
    void Shoot()
    {
        //make sure we have a target still, and that that target is the same as when we lined up the shot
        if (m_crosshair.m_targettedObject == m_lastTarget)
        {
            m_source.Stop();
            m_source.PlayOneShot(m_source.clip);
            m_crosshair.m_targettedObject.Hit();
            m_cooldown = m_maxCooldown;
            m_particles.Emit(15); //emit small smoke from gun area to signify shot
            m_lastTarget = null;
            
        }
        m_shooting = false;
    }
}
