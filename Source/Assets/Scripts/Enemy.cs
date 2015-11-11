using UnityEngine;

public class Enemy : MonoBehaviour {
    public ParticleSystem m_mainParticles;
    public ParticleSystem m_smokeParticles;
    public int m_level;
    private Vector3 m_position;
    private float m_timer;
    private bool m_dead = false;
    public Color[] m_possibleColors;

    private float m_health = 1.0f;
    public float m_damagePerHit;
    public GameObject m_healthBar;

    private Renderer m_renderer;
    private Color m_colour;
    private float m_hitTimer;

    private AudioSource m_source;

	// Use this for initialization
	void Awake () {
        //Make sure particles don't start playing straight away
        m_smokeParticles.Stop();
        m_mainParticles.Stop();

        //Pick random colour from a list of selected ones that I liked
        m_colour = m_possibleColors[Random.Range(0, m_possibleColors.Length)];
        m_renderer = GetComponent<Renderer>();
        m_renderer.material.color = m_colour;
        m_mainParticles.GetComponent<Renderer>().material.color = m_colour;

        //Setup stuff for moving enemies
        if(m_level == 2)
        {
            m_position = transform.position;
            m_timer = m_position.x;
        }
        m_source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(m_hitTimer >= 0)
        {
            m_hitTimer -= Time.deltaTime * 5;
            m_renderer.material.color = Color.Lerp(m_colour, Color.red, m_hitTimer);
        }
        if(m_level == 2)
        {
            m_timer += Time.deltaTime;
            m_position.x = Mathf.Sin(m_timer) * 10;
            transform.position = m_position;
        }
        //if Enemy has been shot and all particles are gone, destroy it
        if (m_dead && 
            m_smokeParticles.particleCount == 0 && 
            m_mainParticles.particleCount == 0)
        {
            Destroy(gameObject);
        }
	}

    public void Hit()
    {
        m_health -= m_damagePerHit;
        m_hitTimer = 1.0f;
        if (m_health <= 0.0f)
        {
            Die();
        }
        
        UpdateHealthBar();
    }
    void UpdateHealthBar()
    {
        Vector3 newScale = m_healthBar.transform.localScale;
        newScale.x = m_health;
        m_healthBar.transform.localScale = newScale;

        Vector3 newPosition = m_healthBar.transform.position;
        newPosition.x -= m_damagePerHit * 0.5f; 
        m_healthBar.transform.position = newPosition;
        
    }
    void Die()
    {
        m_source.PlayOneShot(m_source.clip);
        //Emit smoke and cube particles
        m_mainParticles.Emit(100);
        m_smokeParticles.Emit(200);
        //Turn off mesh rendered so it's invisible, and collider so the particles don't collide with it
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        m_healthBar.GetComponentInChildren<MeshRenderer>().enabled = false;
        //Tell GameManager we died
        FindObjectOfType<GameManager>().m_enemiesKilled++;
        m_dead = true;
    }
}
