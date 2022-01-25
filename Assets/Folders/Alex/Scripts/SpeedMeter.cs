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
        //needleTransform.rotation = Quaternion.Euler(new Vector3(0, 0, ZeroSpeedAngle)); //new Vector3(0, 0, ZeroSpeedAngle);
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
        
        float speed = kmp / 1.69f;
        
        return -speed;
    }
}
