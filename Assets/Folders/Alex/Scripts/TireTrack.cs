using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTrack : MonoBehaviour
{
    [SerializeField] private TrailRenderer leftTrail;
    [SerializeField] private TrailRenderer rightTrail;

    public CarControllerScript car;

    private void Update()
    {
        if (car.m_isDrifting && car.m_sphereBody.velocity.magnitude * 3.6f > 1 && Input.GetAxis("Horizontal") > 0 | Input.GetAxis("Horizontal") < 0 && car.m_isGrounded)
        {
            leftTrail.emitting = true;
            rightTrail.emitting = true;
        }
        else
        {
            leftTrail.emitting = false;
            rightTrail.emitting = false;
        }
    }
}
