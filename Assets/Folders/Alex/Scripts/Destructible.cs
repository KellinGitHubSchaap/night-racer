using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Tooltip("How much score you get")]
    [SerializeField] private float score = 250;
    [Tooltip("The destroyed version of the prefab")]
    [SerializeField] private GameObject destroyedVersion;
    [Tooltip("How fast a object must go to break it")]
    [SerializeField] private float breakSpeed = 20;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("DDDDDDDDDDDDDDDDDDDDDDD");
        if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f > breakSpeed)
        {
            Debug.Log("AAAAAAAAAAAAAAA");
            Break();
        }
    }

    public void Break()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
