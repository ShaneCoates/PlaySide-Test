using UnityEngine;

public class Bullet : MonoBehaviour {
    private Vector3 m_direction;
    public float m_speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Move towards target
        transform.position += m_direction * Time.deltaTime * m_speed;
	}
    public void Shoot(Transform _target)
    {
        //Gets direction from current position and targets position
        Shoot(_target.position - transform.position);
    }
    public void Shoot(Vector3 _direction)
    {
        m_direction = Vector3.Normalize(_direction);
    }
    void Explode()
    {
        //TODO: add explosion particle effect
        m_direction = new Vector3();
        Destroy(this.gameObject);
    }
    void OnCollisionEnter(Collision _collision)
    {
        //if collided with enemy, destroy it
        GameObject other = _collision.rigidbody.gameObject;
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Explode();
        }
        Explode();
    }
}
