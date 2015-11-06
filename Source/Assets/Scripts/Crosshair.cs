using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    public Image m_crosshair;
    private Vector3 m_startPosition;
    public Color m_lockedOnColor;
    private Color m_originalColor;
    private Vector2 m_halfScreen;
	// Use this for initialization
	void Start () {
        m_startPosition = m_crosshair.rectTransform.transform.position;
        m_originalColor = m_crosshair.color;
        m_halfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        bool hitEnemy = false;
        if (Physics.SphereCast(Camera.main.ScreenPointToRay(m_halfScreen).origin, 
            2f,
            Camera.main.ScreenPointToRay(m_halfScreen).direction, 
            out hit))
        {
            if (hit.rigidbody.gameObject.CompareTag("Enemy"))
            {
                m_crosshair.rectTransform.position = Vector3.MoveTowards(m_crosshair.rectTransform.position, Camera.main.WorldToScreenPoint(hit.rigidbody.transform.position), 10);
                SetColour(m_lockedOnColor);
                hitEnemy = true;
            }
        }
        if(!hitEnemy)
        {
            ResetCrosshair();
        }
	}

    void ResetCrosshair()
    {
        Vector3 crossHairPosition = m_crosshair.rectTransform.transform.position;
        if (Vector3.SqrMagnitude(crossHairPosition - m_startPosition) > 0.01f)
        {
            m_crosshair.rectTransform.transform.position = Vector3.MoveTowards(crossHairPosition, m_startPosition, 10);
        }
        if(m_originalColor != m_crosshair.color)
        {
            SetColour(m_originalColor);
        }
    }

    void SetColour(Color _newColour)
    {
        m_crosshair.color = _newColour;
    }
}
