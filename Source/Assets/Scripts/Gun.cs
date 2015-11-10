using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Gun : MonoBehaviour {
    public Crosshair m_crosshair;

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
        //if there is a target, shoot towards it, otherwise shoot in a general forward direction
        //this is to make sure we are on target when the crosshair is moving around screen
        if (m_crosshair.m_targettedObject != null)
        {
            m_crosshair.m_targettedObject.Hit();
        }
    }
}
