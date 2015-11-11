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

    private AudioSource m_source;
	// Use this for initialization
	void Start () {
        m_cooldown = 0.0f;
        m_shooting = false;
        m_reloadingText.enabled = false;

        m_remainingAmmo = m_clipSize;
        m_source = GetComponent<AudioSource>();

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
            //enable 1 image for each piece of ammo in the clip
            m_ammoImages[i].enabled = (i < m_remainingAmmo);
        }
    }
    void UpdateAmmo()
    {
        //if we're out of ammo and not already reloading, start reloading!
        if (m_remainingAmmo <= 0 && !m_reloading)
        {
            m_reloading = true;
            m_cooldown = m_maxReloadTime;
            m_reloadingText.enabled = true; //show text so user knows what's happening
        }
        //if we've finished reloading
        if (m_reloading && m_cooldown <= 0)
        {
            m_reloading = false;
            m_remainingAmmo = m_clipSize;
            m_reloadingText.enabled = false;
            UpdateUI(); //update ammo ui to reflect newly filled clip
        }
    }
    void CheckForTarget()
    {
        //if we're not already shooting, and there's no cooldown - check if there's something targetted
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
        //make sure we have a target still, and that that target is the same as when we lined up the shot
        if (m_crosshair.m_targettedObject == m_lastTarget)
        {
            m_source.PlayOneShot(m_source.clip);
            m_crosshair.m_targettedObject.Hit();
            m_cooldown = m_maxCooldown;
            m_particles.Emit(15); //emit small smoke from gun area to signify shot
            m_lastTarget = null;
            
            m_remainingAmmo--;
            UpdateUI();
        }
        m_shooting = false;
    }
}
