using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioScript : MonoBehaviour
{
    public AudioSource m_carEngine;

    private void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            m_carEngine.pitch += Input.GetAxis("Vertical") * Time.deltaTime;
            m_carEngine.pitch = Mathf.Clamp(m_carEngine.pitch, 1, 1.75f);
        }
        else
        {
            if (m_carEngine.pitch > 1.05)
            {
                m_carEngine.pitch = Mathf.Lerp(m_carEngine.pitch, 1, Time.deltaTime);
            }
            else
            {
                m_carEngine.pitch = 1;
            }
        }
    }
}
