using UnityEngine;

public class Door : MonoBehaviour {
    public GameObject[] m_doorPieces;
    private bool m_opening;
    private float m_totalMoved = 0;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_opening)
        {
            //Stop after the door has opened 10 units
            if (m_totalMoved > 10)
            {
                m_opening = false;
            }
            else
            {
                //Translate each door piece away from the center
                m_doorPieces[0].transform.Translate(Time.deltaTime * 10, 0, 0);
                m_doorPieces[1].transform.Translate(Time.deltaTime * -10, 0, 0);
                m_totalMoved += Time.deltaTime * 10;
            }
        }
	}

    public void Open()
    {
        //Make sure we are set up correctly
        if (m_doorPieces.Length > 1)
        {
            m_opening = true;
        }
    }
}
