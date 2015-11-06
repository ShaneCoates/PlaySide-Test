using UnityEngine;

public class Enemy : MonoBehaviour {
    ParticleSystem[] m_particles;
	// Use this for initialization
	void Awake () {
        //Get all ParticleSystems for later
        m_particles = GetComponentsInChildren<ParticleSystem>();
        GetComponentInChildren<Light>().enabled = false; //Turn light off until we get hit
        foreach (ParticleSystem p in m_particles)
        {
            //Turn off all particle systems
            p.Stop();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Explode()
    {

        GetComponentInChildren<Light>().enabled = true;//Turn on light
        FindObjectOfType<GameManager>().m_enemiesKilled++;
        foreach (ParticleSystem p in m_particles)
        {
            //Start playing Fire Particles >:D
            p.Play();
        }
        //Wait a little while, then hide
        Invoke("Hide", 0.5f);
    }
    void Hide()
    {
        //hide object, but let particles play out, then delete
        foreach (ParticleSystem p in m_particles)
        {
            p.enableEmission = false;
        }
        GetComponentInChildren<MeshRenderer>().enabled = false;
        Invoke("Delete", 1.0f);
    }
    void Delete()
    {
        Destroy(gameObject);
    }
}
