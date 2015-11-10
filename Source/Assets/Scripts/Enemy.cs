using UnityEngine;

public class Enemy : MonoBehaviour {
    public ParticleSystem m_mainParticles;
    public ParticleSystem m_smokeParticles;
    public int m_level;
    private Vector3 m_position;
    private float m_timer;
    private bool m_dead = false;
    public Color[] m_possibleColors;

	// Use this for initialization
	void Awake () {
        //Make sure particles don't start playing straight away
        m_smokeParticles.Stop();
        m_mainParticles.Stop();

        //Pick random colour from a list of selected ones that I liked
        Color enemyColor = m_possibleColors[Random.Range(0, m_possibleColors.Length)];
        GetComponent<Renderer>().material.color = enemyColor;
        m_mainParticles.GetComponent<Renderer>().material.color = enemyColor;

        //Setup stuff for moving enemies
        if(m_level == 2)
        {
            m_position = transform.position;
            m_timer = m_position.x;
        }
        
	}
	
	// Update is called once per frame
	void Update () 
    {
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
        //Emit smoke and cube particles
        m_mainParticles.Emit(100);
        m_smokeParticles.Emit(200);
        //Turn off mesh rendered so it's invisible, and collider so the particles don't collide with it
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        //Tell GameManager we died
        FindObjectOfType<GameManager>().m_enemiesKilled++;
        m_dead = true;
    }
}
