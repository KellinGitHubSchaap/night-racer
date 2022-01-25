using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public int m_checkPointID;
    private GameManager m_gameRef;

    private void Start()
    {
        m_gameRef = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Goodbye Moon");
            m_gameRef.StoreCurrentCheckPoint(this.gameObject, m_checkPointID);
        }
    }
}
