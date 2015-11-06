using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    public Image m_crosshair;
    private Vector3 m_startPosition;
    public Vector2 m_halfScreen;
    public GameObject m_targettedObject { get; private set; }
	// Use this for initialization
	void Start () 
    {
        m_startPosition = m_crosshair.rectTransform.transform.position; //Position that crosshair begins in - used to reset when not targetting an enemy
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
        if (Physics.SphereCast(Camera.main.ScreenPointToRay(m_halfScreen).origin, 2f, Camera.main.ScreenPointToRay(m_halfScreen).direction, out hit))
        {
            //if SphereCast hits an enemy, move crosshair to hover over that particular enemy
            //moving with dual joystick controls on a mobile isn't easy to be entirely precise, so we help a little
            if (hit.rigidbody.gameObject.CompareTag("Enemy"))
            {
                m_crosshair.rectTransform.position = Vector3.MoveTowards(m_crosshair.rectTransform.position, Camera.main.WorldToScreenPoint(hit.rigidbody.transform.position), 10);
                hitEnemy = true;
                //Targetted Object saved for use when shooting
                m_targettedObject = hit.rigidbody.gameObject;
            }
        }
        if (!hitEnemy)
        {
            ResetCrosshair();
            m_targettedObject = null;
        }
    }

    void ResetCrosshair()
    {
        //Move crosshair back towards center
        Vector3 crossHairPosition = m_crosshair.rectTransform.transform.position;
        //If crosshair is close enough, don't bother - saves unnecesary calculations each frame
        if (Vector3.SqrMagnitude(crossHairPosition - m_startPosition) > 0.01f) 
        {
            m_crosshair.rectTransform.transform.position = Vector3.MoveTowards(crossHairPosition, m_startPosition, 10);
        }
    }
}
