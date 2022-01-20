using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Tooltip("How much score you get")]
    [SerializeField] private float score = 250;
    [Tooltip("The destroyed version of the prefab")]
    [SerializeField] private GameObject destroyedVersion;

    public void Break()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
