using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerScript : MonoBehaviour
{
    #region References
    [Header("References")]
    public Rigidbody m_sphereBody;
    #endregion

    #region Movement
    [Header("Movement")]
    public float maxSpeed = 130;           //How fast the car goes in kmp
    public float reverseSpeed = 60;           //How fast the car goes backwards in kmp

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

    public bool m_isDrifting = false;
    public float m_driftRotation = 180f;            // Rotation when in drift mode 

    public float m_groundDrag = 3f;                 // Drag when the car is on the ground
    public float m_airDrag = 0.3f;                  // Drag when the car is in the air
    #endregion

    #region Ground Check
    [Header("Ground Check")]
    public float m_gravityForce = 10f;              // The gravity force to the car
    public float m_checkGroundRayLength = 0.5f;     // Length of the ground Ray
    public Transform m_groundRayPos;                // Position of the ground Ray detection
    public LayerMask m_groundLayer;                 // What layer is considered ground
    public bool m_isGrounded;                       // Is the car grounded

    public float m_roofRayLength = 0.6f;            // Length of the roofRay checking if the car has landed on its roof
    #endregion

    #region Car Parts
    [Header("Car Parts : Wheels")]
    public Transform m_frontWheelRight;
    public Transform m_frontWheelLeft;

    public float m_minWheelRotation = 15f;          // If the car isn't drifting a less sharp tire angle is needed
    public float m_maxWheelRotation = 25f;          // If the car is drifting a sharper tire angle is needed

    [Header("Car Parts : Body")]
    public Transform m_carBodyModel;                // Body of the Car that is used for the Banking effect when steering

    public float m_minBanking = 5f;                 // How much does the car bank to the side when not drifting
    public float m_maxBanking = 10f;                // How much does the car bank to the side when drifting

    public float m_restoreRotationSpeed = 0.5f;     // A variable designed to make the car flip back to a wanted rotation in a slower way
    #endregion

    #region Other Settings
    [Header("Other Settings")]
    private float m_offsetToCenterSphere = -.2f;
    #endregion

    private void Start()
    {
        m_sphereBody.transform.parent = null;       // Disconnect the Sphere Collider from the Car 
    }

    private void Update()
    {
        GetForwardInput(Input.GetAxis("Vertical"));                             // Forward speed of the car uses the Input.GetAxis(Vertical)
        m_accel = m_speedInput < 0 ? m_accelBoost : m_maxForwardAccel;          // Change accel based on the current SpeedInput of the car 

        GetSidewaysInput(Input.GetAxis("Horizontal"));                          // Horizontal speed (AKA turning) uses the Input.GetAxis(Horizontal)

        // When drifting the Car gets a higher mobility, else a m_minMobility is given
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformDriftInput(m_driftRotation, true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            PerformDriftInput(m_minMobility, false);
        }

        // When the car is grounded it is allowed to turn and rotate
        if (IsCarGrounded())
        {
            m_rotationInput = Input.GetAxis("Horizontal");

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
                m_frontWheelLeft.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * m_minWheelRotation + 180), 0);
                m_frontWheelRight.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * m_minWheelRotation) + 180, 0);

                m_carBodyModel.localEulerAngles = new Vector3(0, m_carBodyModel.localEulerAngles.y, Input.GetAxis("Horizontal") * m_minBanking);
            }
            else
            {
                m_frontWheelLeft.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * m_maxWheelRotation) + 180, 0);
                m_frontWheelRight.localEulerAngles = new Vector3(0, (Input.GetAxis("Horizontal") * m_maxWheelRotation) + 180, 0);

                m_carBodyModel.localEulerAngles = new Vector3(0, m_carBodyModel.localEulerAngles.y, Input.GetAxis("Horizontal") * m_maxBanking);
            }
        }

        // Player can reset his/her car when stuck
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToCheckPoint();
        }

        // Move the position of the Player to the body of the Sphere it is supposed to follow.
        transform.position = new Vector3(m_sphereBody.transform.position.x, m_sphereBody.transform.position.y + m_offsetToCenterSphere, m_sphereBody.transform.position.z);
    }

    #region GetForwardInput(), GetSidewaysInput() and PerformDriftInput() 
    // GetForwardInput(float verticalAxisDirection) will receive the Input.GetAxis("Vertical") to perform the forward movement for the car
    private void GetForwardInput(float verticalAxisDirection)
    {
        if (verticalAxisDirection > 0)
        {
            m_speedInput += verticalAxisDirection * m_accel * 100f;
        }
        else if (verticalAxisDirection < 0)
        {
            m_accel = m_reverseAccel;
            m_speedInput += verticalAxisDirection * m_accel * 100f;
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

        m_speedInput = Mathf.Clamp(m_speedInput, (-reverseSpeed * 67.629750f) / 1.03f, (maxSpeed * 67.629750f) / 1.03f);                  // Clamp the speed  //1kmp 67,629750
    }

    // GetSidewaysInput(float horizontalAxisDirection) will receive the Input.GetAxis("Horizontal") to perform the steering movement for the car
    private void GetSidewaysInput(float horizontalAxisDirection)
    {
        if (horizontalAxisDirection > 0)
        {
            m_mobility += horizontalAxisDirection * 2;
        }
        else if (horizontalAxisDirection < 0)
        {
            m_mobility -= horizontalAxisDirection * 2;
        }
        else
        {
            m_mobility = m_minMobility;
        }


        if (!m_isDrifting)
        {
            m_mobility = Mathf.Clamp(m_mobility, m_minMobility, m_maxMobility);     // Clamp the mobility
        }
        else
        {
            m_mobility = Mathf.Clamp(m_mobility, m_minMobility, m_driftRotation);
        }
    }

    // PerformDriftInput(float driftStrength, bool isCarDrifting) will be used to allow drifting to happen, meanwhile allow the car different mobility
    private void PerformDriftInput(float driftStrength, bool isCarDrifting)
    {
        m_mobility = driftStrength;
        m_isDrifting = isCarDrifting;
    }
    #endregion

    private void FixedUpdate()
    {
        // If the car is on the ground it needs to receive a different amount of drag and you can drive
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

        // If the roofRay touches the ground it means that the car has flipped over and thereby crashed
        if (Physics.Raycast(transform.position, transform.up, out RaycastHit roofRay, m_roofRayLength, m_groundLayer))
        {
            ResetToCheckPoint();
        }
    }

    #region IsCarGrounded()
    // IsCarGrounded() will check if the groundRay is hitting a ground layer, if true return a true for m_isGrounded.
    // Else the car needs to stablize itself back to a horizontal rotation. 
    private bool IsCarGrounded()
    {
        m_isGrounded = false;

        RaycastHit hit;

        if (Physics.Raycast(m_groundRayPos.position, -transform.up, out hit, m_checkGroundRayLength, m_groundLayer))
        {
            m_isGrounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;  // Set the rotation of the car to be that of the rotation of the face
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(20, transform.eulerAngles.y, transform.eulerAngles.z), Time.deltaTime * m_restoreRotationSpeed);
        }

        return m_isGrounded;
    }
    #endregion

    #region ResetToCheckPoint()
    // When the car crashes or when the player hits the "R" Key the car will be reset to the last checkpoint's position
    public void ResetToCheckPoint()
    {
        m_sphereBody.velocity = Vector3.zero;   // Set the velocity of the car to be 0
        m_sphereBody.transform.position = new Vector3(GameManager.instance.m_currentCheckPoint.transform.position.x, GameManager.instance.m_currentCheckPoint.transform.position.y + 2, GameManager.instance.m_currentCheckPoint.transform.position.z);     // Since the car is following the m_sphereBody, it is best to move that instead of the car
        transform.eulerAngles = GameManager.instance.m_currentCheckPoint.transform.eulerAngles;     // The car needs to be facing towards the direction forward to the finish each time this function is run. 
    }
    #endregion
}
