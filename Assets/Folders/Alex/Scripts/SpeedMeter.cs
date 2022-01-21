using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedoMeterText;
    [SerializeField] private Transform needleTransform;
    [SerializeField] private float MaxSpeedAngle = -20;
    [SerializeField] private float ZeroSpeedAngle = 220;

    private CarControllerScript carController;
    public Rigidbody rb;

    float kmp;

    private void Start()
    {
        carController = GetComponent<CarControllerScript>();
    }

    private void Update()
    {
        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());

        kmp = Mathf.FloorToInt(rb.velocity.magnitude * 3.6f);
        speedoMeterText.text = string.Format("{000}", kmp);
    }

    private float GetSpeedRotation()
    {
        float totalAngle = ZeroSpeedAngle - MaxSpeedAngle;

        //float speed = rb.velocity.magnitude * 3.6f;
        float speed = carController.m_speedInput / (carController.maxSpeed * 67.629750f) / 1.03f;

        return ZeroSpeedAngle - speed * totalAngle;
    }
}
