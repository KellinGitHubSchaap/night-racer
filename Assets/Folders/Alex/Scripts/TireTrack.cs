using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TireTrack : MonoBehaviour
{
    [SerializeField] private TrailRenderer leftTrail;
    [SerializeField] private TrailRenderer rightTrail;

    [SerializeField] private VisualEffect leftTrailDust;
    [SerializeField] private VisualEffect rightTrailDust;

    public CarControllerScript car;

    private void Update()
    {
        if (car.m_sphereBody.velocity.magnitude * 3.6f > 1 && car.m_isGrounded)
        {
            leftTrail.emitting = true;
            rightTrail.emitting = true;

            leftTrailDust.Play();
            rightTrailDust.Play();
        }
        else
        {
            leftTrail.emitting = false;
            rightTrail.emitting = false;

            leftTrailDust.Stop();
            rightTrailDust.Stop();
        }
    }
}
