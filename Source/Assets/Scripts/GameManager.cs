using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public int m_enemiesKilled;
    public Door[] m_doors;
    private int m_doorsOpened;
    public CanvasGroup m_gameOverPanel;

    private float m_timer;
    public Text m_timerText;
    private float m_fastestTime;
    public Text m_fastestTimeText;
	// Use this for initialization
	void Start () 
    {
        m_doors[0].Open();
        m_doorsOpened = 1;
        m_timer = 0.0f;

        if (PlayerPrefs.HasKey("FastestTime"))
        {
            m_fastestTime = PlayerPrefs.GetFloat("FastestTime");
            m_fastestTimeText.enabled = true;
            m_fastestTimeText.text = "Best: " + m_fastestTime.ToString("F2");

        }
        else
        {
            m_fastestTime = 0;
            m_fastestTimeText.enabled = false;
        }
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
            m_timer += Time.deltaTime;
            m_timerText.text = m_timer.ToString("F2");
        }
	}
    public void ShowGameOverPanel()
    {
        //Shows game over panel with text and reset button
        m_gameOverPanel.alpha = 1;
        m_gameOverPanel.interactable = true;
        m_gameOverPanel.blocksRaycasts = true;

        //If we got a new fastest time, replace it in player prefs
        if (m_timer < m_fastestTime || !PlayerPrefs.HasKey("FastestTime"))
        {
            PlayerPrefs.SetFloat("FastestTime", m_timer);
            PlayerPrefs.Save();
        }
    }
    public void ResetGame()
    {
        //reloads game
        Application.LoadLevel("main");
    }
}
