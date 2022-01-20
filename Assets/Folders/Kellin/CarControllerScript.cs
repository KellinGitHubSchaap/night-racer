using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerScript : MonoBehaviour
{
    [Header("References")]
    public Rigidbody m_sphereBody;

    [Header("Movement")]
    public float m_forwardAccel = 8f;       // Forward acceleration speed
    public float m_reverseAccel = 4f;       // Backward acceleration speed

    [Tooltip("Rotation Speed of the car")]
    private float m_mobility = 60f;      // Rotation speed of the car
    public float m_minMobility = 60f;      // Rotation speed of the car
    public float m_maxMobility = 180f;      // Rotation speed of the car

    private float m_speedInput;        
    private float m_rotationInput;

    private bool m_isDrifting = false;

    public float m_groundDrag = 3f;     // Drag when the car is on the ground
    public float m_airDrag = 0.3f;      // Drag when the car is in the air

    [Header("Ground Check")]
    public float m_gravityForce = 10f;      // The gravity force to the car
    public float m_checkGroundRayLength = 0.5f;     // Length of the ground Ray
    public Transform m_groundRayPos;        // Position of the ground Ray detection
    public LayerMask m_groundLayer;         // What layer is considered ground
    private bool m_isGrounded;      // Is the car grounded

    [Header("Car Parts")]
    public Transform m_frontWheelRight;
    public Transform m_backWheelRight;
    public Transform m_frontWheelLeft;
    public Transform m_backWheelLeft;

    public float m_minWheelRotation = 15f;      // If the car isn't drifting a less sharp tire angle is needed
    public float m_maxWheelRotation = 25f;      // If the car is drifting a sharper tire angle is needed

    [Header("Other Settings")]
    private float m_offsetToCenterSphere = -.18f;

    private void Start()
    {
        m_sphereBody.transform.parent = null;       // Disconnect the Sphere Collider from the Car 
    }

    private void Update()
    {
        m_speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            m_speedInput += Input.GetAxis("Vertical") * m_forwardAccel * 100f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            m_speedInput += Input.GetAxis("Vertical") * m_reverseAccel * 100f;
        }

        m_rotationInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_mobility = 130;
            m_isDrifting = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            m_mobility = 60;
            m_isDrifting = false;
        }

        if (IsCarGrounded())
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, m_rotationInput * m_mobility * Time.deltaTime * Input.GetAxis("Vertical"), 0f));

            if (!m_isDrifting)
            {
                m_frontWheelLeft.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * m_minWheelRotation), m_frontWheelLeft.localEulerAngles.z);
                m_frontWheelRight.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * m_minWheelRotation), m_frontWheelRight.localEulerAngles.z);
            }
            else
            {
                m_frontWheelLeft.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * m_maxWheelRotation), m_frontWheelLeft.localEulerAngles.z);
                m_frontWheelRight.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * m_maxWheelRotation), m_frontWheelRight.localEulerAngles.z);
            }
            
        }

        transform.position = new Vector3(m_sphereBody.transform.position.x, m_sphereBody.transform.position.y + m_offsetToCenterSphere, m_sphereBody.transform.position.z);
    }

    private void FixedUpdate()
    {
        if (IsCarGrounded())
        {
            m_sphereBody.drag = m_groundDrag;

            if (Mathf.Abs(m_speedInput) > 0)
            {
                m_sphereBody.AddForce(transform.forward * m_speedInput);
            }
        }
        else
        {
            m_sphereBody.drag = m_airDrag;

            m_sphereBody.AddForce(Vector3.up * -m_gravityForce * 100);
        }
    }

    // Is Car Grounded will check if the groundRay is hitting a ground layer, if true return a true for m_isGrounded 
    private bool IsCarGrounded()
    {
        m_isGrounded = false;

        RaycastHit hit;

        if (Physics.Raycast(m_groundRayPos.position, -transform.up, out hit, m_checkGroundRayLength, m_groundLayer))
        {
            m_isGrounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        return m_isGrounded;
    }
}