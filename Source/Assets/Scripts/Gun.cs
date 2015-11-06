using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
public class Gun : MonoBehaviour {

    public Text m_debugText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //m_debugText.text = "NOT FIRING";
        if (CrossPlatformInputManager.GetButtonDown("Shoot"))
        {
            Shoot();
        }
        else
        {
            m_debugText.text = "NOT FIRING";
        }
	}
    void Shoot()
    {
        m_debugText.text = "FIRING";
    }
}
