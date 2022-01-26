//______________________________________________//
//___________Realistic Engine Sounds____________//
//______________________________________________//
//_______Copyright © 2019 Yugel Mobile__________//
//______________________________________________//
//_________ http://mobile.yugel.net/ ___________//
//______________________________________________//
//________ http://fb.com/yugelmobile/ __________//
//______________________________________________//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoController : MonoBehaviour // "Slider" demo scene script, only used for demonstration and for testing, do not use it in live products! This scipt is designed to work only in "Slider" demo scenes, it will not work in other scenes. For other scenes import the right *.unitypackage for your car controller or contact me in email for adding support for your custom car controller.
{
    public Transform[] allChildren;
    public GameObject gasPedalButton; // UI button
    public Slider rpmSlider; // UI slider to set RPM
    public Slider pitchSlider; // UI sliter to set maximum pitch
    public Text pitchText; // UI text to show pitch multiplier value
    public Text rpmText; // UI text to show current RPM
    public Toggle isReversing; // UI checkbox for is reversing
    public Toggle gasPedalPressing;
    public GameObject accelerationSpeed; // UI slider for acceleration speed
    public bool simulated = true; // is rpm simulated with gaspedal button or with rpm slider by hand
    public bool isMobile = false; // for mobile RES prefabs
    CarSimulator carSimulator;
    private void Start()
    {
        allChildren = GetComponentsInChildren<Transform>();
        carSimulator = gasPedalButton.GetComponent<CarSimulator>();
        // turn off all interior prefabs
        for (int i = 1; i < allChildren.Length; i++)
        {
            if(i%2==0)
            allChildren[i].gameObject.SetActive(false);
        }
        if (isMobile)
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound_mobile>().carMaxSpeed = 7000;
            }
        }
        else
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound>().carMaxSpeed = 7000;
            }
        }
    }

    private void Update()
    {
        rpmText.text = "Engine RPM: " + (int)rpmSlider.value; // show current RPM - this creates garbage
        pitchText.text = "" + pitchSlider.value; // set pitch multiplier value for ui text
        // rpm values
        if (isMobile)
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound_mobile>().pitchMultiplier = pitchSlider.value;
                if (simulated)
                {
                    allChildren[i].GetComponent<RealisticEngineSound_mobile>().engineCurrentRPM = carSimulator.rpm;
                    rpmSlider.value = carSimulator.rpm; // set ui sliders value to rpm
                }
                else
                {
                    allChildren[i].GetComponent<RealisticEngineSound_mobile>().engineCurrentRPM = rpmSlider.value;
                    carSimulator.rpm = rpmSlider.value;
                }
                allChildren[i].GetComponent<RealisticEngineSound_mobile>().carCurrentSpeed = rpmSlider.value/127; // for referse gear fx
            }
        }
        else
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound>().pitchMultiplier = pitchSlider.value;
                if (simulated)
                {
                    allChildren[i].GetComponent<RealisticEngineSound>().engineCurrentRPM = carSimulator.rpm;
                    rpmSlider.value = carSimulator.rpm; // set ui sliders value to rpm
                }
                else
                {
                    allChildren[i].GetComponent<RealisticEngineSound>().engineCurrentRPM = rpmSlider.value;
                    carSimulator.rpm = rpmSlider.value;
                }
                allChildren[i].GetComponent<RealisticEngineSound>().carCurrentSpeed = rpmSlider.value/127; // for reverse gear fx
            }
        }
        if (simulated) // update is gas pedal pressing toggle
            if (gasPedalPressing != null)
                gasPedalPressing.isOn = carSimulator.gasPedalPressing;
    }

    // enable/disable rev limiter
    public void UpdateRPM(Toggle togl)
    {
        if (isMobile)
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound_mobile>().useRPMLimit = togl.isOn;
            }
        }
        else
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound>().useRPMLimit = togl.isOn;
            }
        }
    }
    // enable/disable reverse gear sound fx
    public void UpdateReverseGear(Toggle togl)
    {
        if (isMobile)
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound_mobile>().enableReverseGear = togl.isOn;
            }
        }
        else
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound>().enableReverseGear = togl.isOn;
            }
        }
        // show/hide isReversing checkbox
        if (togl.isOn == false)
        {
            isReversing.isOn = false;
            isReversing.gameObject.SetActive(false);
        }
        else
        {
            if (isReversing.gameObject.activeSelf == false)
                isReversing.gameObject.SetActive(true);
        }
    }
    // is reversing
    public void IsReversing(Toggle togl)
    {
        if (isMobile)
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound_mobile>().isReversing = togl.isOn;
            }
        }
        else
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<RealisticEngineSound>().isReversing = togl.isOn;
            }
        }
    }
    // is simulated rpm
    public void IsSimulated(Dropdown drpDown)
    {
        if (drpDown.value == 0)
        {
            simulated = true;
            accelerationSpeed.SetActive(true);
            gasPedalButton.SetActive(true);
        }
        if (drpDown.value == 1)
        {
            simulated = false;
            accelerationSpeed.SetActive(false);
            gasPedalButton.SetActive(false);
            if (!isMobile)
                gasPedalPressing.isOn = true;
        }
    }
    // change car sound buttons
    public void ChangeCarSound(int a) // a = exterior, a+1 = interior prefabs id numbers in allChildren[]
    {
        if (isMobile)
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                if (i != a && i != a + 1)
                    allChildren[i].GetComponent<RealisticEngineSound_mobile>().enabled = false;
                allChildren[a].GetComponent<RealisticEngineSound_mobile>().enabled = true;
                allChildren[a+1].GetComponent<RealisticEngineSound_mobile>().enabled = true;
            }
        }
        else
        {
            for (int i = 1; i < allChildren.Length; i++)
            {
                if (i != a && i != a + 1)
                    allChildren[i].GetComponent<RealisticEngineSound>().enabled = false;
                allChildren[a].GetComponent<RealisticEngineSound>().enabled = true;
                allChildren[a + 1].GetComponent<RealisticEngineSound>().enabled = true;
            }
        }
    }
    // gas pedal checkbox
    public void UpdateGasPedal(Toggle togl)
    {
        for (int i = 1; i < allChildren.Length; i++)
        {
            allChildren[i].GetComponent<RealisticEngineSound>().gasPedalPressing = togl.isOn;
        }
    }
}
