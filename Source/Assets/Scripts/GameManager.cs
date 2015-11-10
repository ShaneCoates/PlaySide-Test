using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public int m_enemiesKilled;
    public Door[] m_doors;
    private int m_doorsOpened;
    public CanvasGroup m_gameOverPanel;
	// Use this for initialization
	void Start () 
    {
        m_doors[0].Open();
        m_doorsOpened = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Open relevant door after killing 5 enemies
        //Keep track of how many doors have been opened so we're not calling Open() every frame
        if(m_doorsOpened < 3)
        {
            for (int i = m_doorsOpened; i < 3; i++)
            {
                if (m_enemiesKilled == 5 * i)
                {
                    m_doors[i].Open();
                    m_doorsOpened++;
                }
            }
        }
	}
    public void ShowGameOverPanel()
    {
        //Shows game over panel with text and reset button
        m_gameOverPanel.alpha = 1;
        m_gameOverPanel.interactable = true;
        m_gameOverPanel.blocksRaycasts = true;
    }
    public void ResetGame()
    {
        //reloads game
        Application.LoadLevel("main");
    }
}
