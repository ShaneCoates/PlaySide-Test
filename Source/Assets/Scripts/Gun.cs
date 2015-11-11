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

    public float m_clipSize;
    private float m_remainingAmmo;
    private bool m_reloading = false;

    public Image[] m_ammoImages;
    public Text m_reloadingText;

    private bool m_shooting;
    private Enemy m_lastTarget;

	// Use this for initialization
	void Start () {
        m_cooldown = 0.0f;
        m_shooting = false;
        m_reloadingText.enabled = false;

        m_remainingAmmo = m_clipSize;
        UpdateUI();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_cooldown > 0) m_cooldown -= Time.deltaTime;
    
        UpdateAmmo();
        CheckForTarget();

        m_cooldownImage.fillAmount = m_cooldown / (m_reloading ? m_maxReloadTime : m_maxCooldown);
	}
    void UpdateUI()
    {
        for(int i = 0; i < m_clipSize; i++)
        {
            m_ammoImages[i].enabled = (i < m_remainingAmmo);
        }
    }
    void UpdateAmmo()
    {
        if (m_remainingAmmo <= 0 && !m_reloading)
        {
            m_reloading = true;
            m_cooldown = m_maxReloadTime;
            m_reloadingText.enabled = true;
        }
        if (m_reloading && m_cooldown <= 0)
        {
            m_reloading = false;
            m_remainingAmmo = m_clipSize;
            m_reloadingText.enabled = false;
            UpdateUI();
        }
    }
    void CheckForTarget()
    {
        if (m_cooldown <= 0 && !m_shooting)
        {
            if (m_crosshair.m_targettedObject != null)
            {
                m_lastTarget = m_crosshair.m_targettedObject;
                Invoke("Shoot", 0.1f);
                m_shooting = true;
            }
        }
    }
    void Shoot()
    {
        //if there is a target, shoot towards it, otherwise shoot in a general forward direction
        //this is to make sure we are on target when the crosshair is moving around screen
        if (m_crosshair.m_targettedObject == m_lastTarget)
        {
            m_crosshair.m_targettedObject.Hit();
            m_cooldown = m_maxCooldown;
            m_particles.Emit(15);
            m_lastTarget = null;
            
            m_remainingAmmo--;
            UpdateUI();
        }
        m_shooting = false;
    }
}
