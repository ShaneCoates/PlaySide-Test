using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {
    public Door m_targetDoor;

    void OnTriggerEnter(Collider other)
    {
        //Make sure it's the player enterring the trigger volume
        if (other.gameObject.CompareTag("Player"))
        {
            //Close the door
            m_targetDoor.Close();
        }
    }
}
