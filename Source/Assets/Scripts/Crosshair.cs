using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    public Image m_crosshair;

    private Vector2 m_halfScreen;
    
    //Current enemy in sights
    public Enemy m_targettedObject { get; private set; }
	// Use this for initialization
	void Start () 
    {
        //Initialise all variables
        m_halfScreen = new Vector2(Screen.width / 2, Screen.height / 2); //Point in center of screen

	}
	
	// Update is called once per frame
	void Update () 
    {
        CheckForEnemy();
	}

   
    void CheckForEnemy()
    {
        RaycastHit hit;
        bool hitEnemy = false; //set to true if spherecast hits an enemy - otherwise resets crosshair position
        
        //Spherecase from center of camera to world space with radius of 2
        if (Physics.Raycast(Camera.main.ScreenPointToRay(m_halfScreen).origin, Camera.main.ScreenPointToRay(m_halfScreen).direction, out hit, 30f, 1, QueryTriggerInteraction.Ignore))
        {
            //if SphereCast hits an enemy, move crosshair to hover over that particular enemy
            //moving with dual joystick controls on a mobile isn't easy to be entirely precise, so we help a little
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //Move crosshair towards enemy
                hitEnemy = true;

                //Targetted Object saved for use when shooting
                m_targettedObject = hit.rigidbody.gameObject.GetComponent<Enemy>();
                m_crosshair.color = Color.red;
            }
        }
        if (!hitEnemy)
        {
            //If no enemy was hit, send crosshair back to center and release m_targettedObject
            m_targettedObject = null;
            m_crosshair.color = Color.white;
        }
    }

   
}
