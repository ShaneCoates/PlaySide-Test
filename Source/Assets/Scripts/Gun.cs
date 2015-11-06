using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Gun : MonoBehaviour {
    public Crosshair m_crosshair;
    public Bullet m_bullet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Check for button input
        if (CrossPlatformInputManager.GetButtonDown("Shoot"))
        {
            Shoot();
        }
	}
    void Shoot()
    {
        //Create new bullet at player's location
        Bullet newBullet = Bullet.Instantiate(m_bullet);
        newBullet.transform.position = transform.position;

        //if there is a target, shoot towards it, otherwise shoot in a general forward direction
        //this is to make sure we are on target when the crosshair is moving around screen
        if (m_crosshair.m_targettedObject != null)
        {
            newBullet.Shoot(m_crosshair.m_targettedObject.transform);
        }
        else
        {
            newBullet.Shoot(Camera.main.ScreenPointToRay(m_crosshair.m_halfScreen).direction);
        }
        
    }
}
