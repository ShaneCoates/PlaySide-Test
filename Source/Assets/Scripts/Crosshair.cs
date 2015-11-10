﻿using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    public Image m_crosshair;
    private Vector3 m_startPosition;
    public Vector2 m_halfScreen;
    public Enemy m_targettedObject { get; private set; }
    private Vector3 m_fullScale;
    private Vector3 m_halfScale;
    private float m_timer = 0.0f;
    private float m_lastTimer;
	// Use this for initialization
	void Start () 
    {
        //Initialise all variables
        m_startPosition = m_crosshair.rectTransform.transform.position; //Position that crosshair begins in - used to reset when not targetting an enemy
        m_halfScreen = new Vector2(Screen.width / 2, Screen.height / 2); //Point in center of screen
        m_fullScale = new Vector3(1.0f, 1.0f, 1.0f);
        m_halfScale = new Vector3(0.5f, 0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        CheckForEnemy();
        UpdateSize();
	}

    void UpdateSize()
    {
        //set lastTimer to value of timer from last value. Used below to check if there has been a change 
        m_lastTimer = m_timer;

        //if there's a target, move over towards it. otherwise move towards the center
        if (m_targettedObject != null)
        {
            if (m_timer <= 1) m_timer += Time.deltaTime * 5;
        }
        else
        {
            if (m_timer >= 0) m_timer -= Time.deltaTime * 5;
        }

        //If there was no noticeable change in value for m_timer, don't transform or change colour of crosshair
        if (Mathf.Abs(m_lastTimer - m_timer) > 0.01f)
        {
            m_crosshair.transform.localScale = Vector3.Lerp(m_fullScale, m_halfScale, m_timer);
            m_crosshair.color = Color.Lerp(Color.white, Color.red, m_timer);
        }
        
    }

    void CheckForEnemy()
    {
        RaycastHit hit;
        bool hitEnemy = false; //set to true if spherecast hits an enemy - otherwise resets crosshair position
        
        //Spherecase from center of camera to world space with radius of 2
        if (Physics.SphereCast(Camera.main.ScreenPointToRay(m_halfScreen).origin, 2f, Camera.main.ScreenPointToRay(m_halfScreen).direction, out hit, 30f))
        {
            //if SphereCast hits an enemy, move crosshair to hover over that particular enemy
            //moving with dual joystick controls on a mobile isn't easy to be entirely precise, so we help a little
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //Move crosshair towards enemy
                m_crosshair.rectTransform.position = Vector3.MoveTowards(m_crosshair.rectTransform.position, Camera.main.WorldToScreenPoint(hit.rigidbody.transform.position), 10);
                hitEnemy = true;

                //Targetted Object saved for use when shooting
                m_targettedObject = hit.rigidbody.gameObject.GetComponent<Enemy>();
            }
        }
        if (!hitEnemy)
        {
            //If no enemy was hit, send crosshair back to center and release m_targettedObject
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
