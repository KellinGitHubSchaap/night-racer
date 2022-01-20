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

        kmp = rb.velocity.magnitude * 3.6f;
        Debug.Log(kmp);
    }

    private float GetSpeedRotation()
    {
        float totalAngle = ZeroSpeedAngle - MaxSpeedAngle;

        float speed = carController.m_speedInput / 5000;

        return ZeroSpeedAngle - speed * totalAngle;
    }
}
