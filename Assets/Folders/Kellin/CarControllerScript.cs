using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerScript : MonoBehaviour
{
    [Header("References")]
    public Rigidbody m_sphereBody;

    [Header("Movement")]

    public float m_accelBoost = 3;          // If the speed < 0 we want to get back to driving forward really quick
    public float m_maxForwardAccel = 1;     // Forward acceleration speed
    public float m_reverseAccel = 4f;       // Backward acceleration speed

    public float m_accel;

    public float m_rollOffStrength = 3f;                 // How quick does the car lose speed when only rolling

    [Tooltip("Rotation Speed of the car")]
    [SerializeField] private float m_mobility = 60f;      // Rotation speed of the car
    [Space(8, order = 1)]
    public float m_minMobility = 30f;                    // Minimal Rotation speed of the car
    public float m_maxMobility = 90f;                    // Maximum Rotation speed of the car

    public float m_speedInput;
    private float m_rotationInput;

    private bool m_isDrifting = false;
    public float m_driftRotation = 180f;            // Rotation when in drift mode 

    public float m_groundDrag = 3f;                 // Drag when the car is on the ground
    public float m_airDrag = 0.3f;                  // Drag when the car is in the air

    [Header("Ground Check")]
    public float m_gravityForce = 10f;              // The gravity force to the car
    public float m_checkGroundRayLength = 0.5f;     // Length of the ground Ray
    public Transform m_groundRayPos;                // Position of the ground Ray detection
    public LayerMask m_groundLayer;                 // What layer is considered ground
    private bool m_isGrounded;                      // Is the car grounded

    [Header("Car Parts")]
    public Transform m_frontWheelRight;
    public Transform m_frontWheelLeft;

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

        // GOAL:
        // IF the car is moving forwards but stops giving gas both m_speedInput && m_mobility need to go down
        // IF the cars speed == 0 only then the m_mobility will be 0 aswell 

        if (Input.GetAxis("Vertical") > 0)
        {
            m_speedInput += Input.GetAxis("Vertical") * m_accel * 100f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            m_accel = m_reverseAccel;
            m_speedInput += Input.GetAxis("Vertical") * m_accel * 100f;
        }
        else
        {
            m_speedInput = Mathf.Lerp(m_speedInput, 0f, Time.deltaTime * m_rollOffStrength);

            if (m_speedInput < 250)
            {
                m_speedInput = 0;
            }

            m_mobility = Mathf.Lerp(m_mobility, m_minMobility, Time.deltaTime * 5);

            if (m_mobility < m_minMobility + 1)
            {
                m_mobility = m_minMobility;
            }
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            m_mobility += Input.GetAxis("Horizontal") * 2;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            m_mobility -= Input.GetAxis("Horizontal") * 2;
        }
        else
        {
            m_mobility = m_minMobility;
        }

        m_rotationInput = Input.GetAxis("Horizontal");

        m_accel = m_speedInput < 0 ? m_accelBoost : m_maxForwardAccel;          // Change accel based on the current SpeedInput of the car 

        m_speedInput = Mathf.Clamp(m_speedInput, -3000, 5000);                  // Clamp the speed
        
        if (!m_isDrifting)
        {
            m_mobility = Mathf.Clamp(m_mobility, m_minMobility, m_maxMobility);     // Clamp the mobility
        }
        else
        {
            m_mobility = Mathf.Clamp(m_mobility, m_minMobility, m_driftRotation);
        }

        // Car gets higher mobility when in Drift mode
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_mobility = m_driftRotation;
            m_isDrifting = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            m_mobility = m_minMobility;
            m_isDrifting = false;
        }

        if (IsCarGrounded())
        {
            if (m_speedInput > 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, m_rotationInput * m_mobility * Time.deltaTime * 1, 0f));
            }
            else if (m_speedInput < 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, m_rotationInput * m_mobility * Time.deltaTime * -1, 0f));
            }

            // If the car is drifting turn the frontwheels
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
