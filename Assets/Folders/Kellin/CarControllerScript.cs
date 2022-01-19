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

    public float m_rotateSpeed = 180f;      // Rotation speed of the car

    private float m_speedInput;        
    private float m_rotationInput;

    public float m_groundDrag = 3f;     // Drag when the car is on the ground
    public float m_airDrag = 0.3f;      // Drag when the car is in the air

    private bool m_isDrifting = false;  // Is the car using a drift
    private float m_driftDirection;     // In which direction does the car drift to

    [Header("Ground Check")]
    public float m_gravityForce = 10f;      // The gravity force to the car
    public float m_checkGroundRayLength = 0.5f;     // Length of the ground Ray
    public Transform m_groundRayPos;        // Position of the ground Ray detection
    public LayerMask m_groundLayer;         // What layer is considered ground
    private bool m_isGrounded;      // Is the car grounded

    private void Start()
    {
        m_sphereBody.transform.parent = null;       // Disconnect the Sphere Collider from the Car 
    }

    private void Update()
    {
        m_speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            m_speedInput = Input.GetAxis("Vertical") * m_forwardAccel * 1000f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            m_speedInput = Input.GetAxis("Vertical") * m_reverseAccel * 1000f;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !m_isDrifting && Input.GetAxis("Horizontal") != 0)
        {
            m_isDrifting = true;
            m_driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;     // If Input.Axis > 0, return 1 else return -1
        }

        if (m_isDrifting)
        {
            float control;
            
            if(m_driftDirection == 1)
            {
                control = ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 0, 2);
            }
            else
            {
                control = ExtensionMethods.Remap(Input.GetAxis("Horizontal"), -1, 1, 2, 0);
            }

        }


        m_rotationInput = Input.GetAxis("Horizontal");

        if (IsCarGrounded())
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, m_rotationInput * m_rotateSpeed * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }

        transform.position = m_sphereBody.transform.position;
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
