using UnityEngine;

public class Enemy : MonoBehaviour {
    public ParticleSystem m_mainParticles;
    public ParticleSystem m_smokeParticles;
    private bool m_dead = false;
	// Use this for initialization
	void Awake () {
        m_smokeParticles.Stop();
        m_mainParticles.Stop();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_dead && m_smokeParticles.particleCount == 0 && m_mainParticles.particleCount == 0)
        {
            Delete();
        }
	}

    public void Explode()
    {
        m_mainParticles.Emit(100);
        m_smokeParticles.Emit(200);
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        FindObjectOfType<GameManager>().m_enemiesKilled++;
        m_dead = true;
    }
    void Delete()
    {
        Destroy(gameObject);
    }
}
