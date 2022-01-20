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

    private void Start()
    {
        carController = GetComponent<CarControllerScript>();
    }

    private void Update()
    {
        
    }

    //private float GetSpeedRotation()
    //{
    //    //float speed = carController.m_speedInput / 
    //}
}
