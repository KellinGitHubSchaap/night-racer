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
    [Tooltip("The breakable layer")]
    [SerializeField] private LayerMask destructibleLayer;

    private bool broken;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("DDDDDDDDDDDDDDDDDDDDDDD");
        if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f > breakSpeed)
        {
            Debug.Log("AAAAAAAAAAAAAAA");
            //Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (broken)
            return;

        if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f > breakSpeed)
        {
            broken = true;
            Break(other.bounds.ClosestPoint(other.gameObject.transform.position));
        }
    }

    /// <summary>
    /// This is called to spawn the broken object and delete the current object
    /// </summary>
    public void Break(Vector3 hitPosition)
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Collider[] colliderPieces = Physics.OverlapSphere(hitPosition, 4, destructibleLayer);

        foreach (Collider item in colliderPieces)
        {
            Rigidbody rb = item.gameObject.GetComponent<Rigidbody>();
            Debug.Log(rb.gameObject);
            rb.AddExplosionForce(250, hitPosition, 5);
        }
        Destroy(gameObject);
    }
}
