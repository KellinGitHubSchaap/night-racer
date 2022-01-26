using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private Transform rotatedObject;
    [SerializeField] private Vector3 rotationDirection;

    private void Update()
    {
        rotatedObject.Rotate(rotationDirection, rotationSpeed * Time.deltaTime);
    }
}
