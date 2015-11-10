using UnityEngine;
using System.Collections.Generic;
public class Door : MonoBehaviour {
    public List<GameObject> m_doorPieces;
    private List<Vector3> m_closedPositions;
    private List<Vector3> m_openPositions;
    private bool m_opening;
    private bool m_closing;
    private float m_position;
    private float m_lastPosition;
	// Use this for initialization
	void Awake () 
    {
        //Initialise lists for Door positions
        m_closedPositions = new List<Vector3>();
        m_openPositions = new List<Vector3>();

	    foreach(GameObject d in m_doorPieces)
        {
            m_closedPositions.Add(d.transform.position);
        }
        Vector3[] newPositions = { m_closedPositions[0], m_closedPositions[1] };
        newPositions[0].x += 10;
        newPositions[1].x -= 10;
        m_openPositions.Add(newPositions[0]);
        m_openPositions.Add(newPositions[1]);
	}
	
	// Update is called once per frame
	void Update () 
    {
        //track last position for later
        m_lastPosition = m_position;
        if (m_opening)
        {
            //If door is opening, increment the timer
            if(m_position >= 1.0f)
                m_opening = false;
            else
                m_position += Time.deltaTime * 5;
        }
        else if(m_closing)
        {
            //If door is closing, decrement the timer
            if (m_position <= 0.0f)
                m_closing = false;
            else
                m_position -= Time.deltaTime * 5;
        }
        //If there has been a noticeable change in the position variable, move the door positions
        //If not, we don't do anything - this saves unnecessary function calls
        if (Mathf.Abs(m_lastPosition - m_position) > 0.01f)
        {
            for (int i = 0; i < 2; i++)
            {
                m_doorPieces[i].transform.position = Vector3.Lerp(m_closedPositions[i], m_openPositions[i], m_position);
            }
        }
	}

    public void Open()
    {
        m_opening = true;
        m_closing = false;
    }
    public void Close()
    {
        m_closing = true;
        m_opening = false;
    }
}
