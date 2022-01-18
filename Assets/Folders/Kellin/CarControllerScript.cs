using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerScript : MonoBehaviour
{
    [Header("References")]
    public Transform m_carModel;
    public Rigidbody m_sphere;

    [Header("Movement")]
    private float m_speed;
    private float m_currentSpeed;

    private float m_rotate;
    private float m_currentRotate;

    public float m_acceleration = 30f;
    public float m_steering = 80f;
    public float m_gravity = 10f;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position = m_sphere.transform.position - new Vector3(0, .2f, 0);

        if (Input.GetKey(KeyCode.W))
        {
            m_speed = m_acceleration;
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(dir, amount);
        }

        m_currentSpeed = Mathf.SmoothStep(m_currentSpeed, m_speed, Time.deltaTime * 12f);
        m_speed = 0f;
        m_currentRotate = Mathf.Lerp(m_currentRotate, m_rotate, Time.deltaTime * 4f);
        m_rotate = 0f;
    }

    private void FixedUpdate()
    {
        m_sphere.AddForce(m_carModel.transform.forward * m_currentSpeed, ForceMode.Acceleration);

        m_sphere.AddForce(Vector3.down * m_gravity, ForceMode.Acceleration);

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + m_currentRotate, 0), Time.deltaTime * 5);
    }

    public void Steer(int direction, float amount)
    {
        m_rotate = (m_steering * direction) * amount;
    }


}
