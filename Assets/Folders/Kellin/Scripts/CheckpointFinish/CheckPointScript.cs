using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public int m_checkPointID;
    

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Goodbye Moon");
            CarControllerScript carScript = other.GetComponent<CarControllerScript>();
            
        }
    }
}
