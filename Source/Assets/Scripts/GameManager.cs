using UnityEngine;

public class GameManager : MonoBehaviour {
    public int m_enemiesKilled;
    public Door m_door;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (m_enemiesKilled == 5)
        {
            m_door.Open();
        }
        if (m_enemiesKilled == 10)
        {
            Application.LoadLevel("main");
            //Win
        }
	}
}
