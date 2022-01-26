using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GraphicsManager : MonoBehaviour
{
    [Header("Toggles")]
    [Tooltip("The full screen toggle")]
    [SerializeField] private Toggle fullscreenToggle;
    [Tooltip("The fps counter toggle")]
    [SerializeField] private Toggle fpsCounterToggle;
    [Tooltip("The VSync toggle")]
    [SerializeField] private Toggle vSyncToggle;
    [Tooltip("The motion blur toggle")]
    [SerializeField] private Toggle motionBlurToggle;
    [Tooltip("The ray tracing toggle")]
    [SerializeField] private Toggle rayTracingToggle;

    [Header("DropDowns")]
    [Tooltip("The bloom drop down")]
    [SerializeField] private TMP_Dropdown bloomDropDown;
    [Tooltip("The motion blur drop down")]
    [SerializeField] private TMP_Dropdown motionBlurDropDown;
    [Tooltip("The ScreenSpaceReflection drop down")]
    [SerializeField] private TMP_Dropdown ssrDropDown;
    [Tooltip("The AmbientOcclusion drop down")]
    [SerializeField] private TMP_Dropdown aoDropDown;

    [Header("Sliders")]
    [Tooltip("The FOV slider")]
    [SerializeField] private Slider fovSlider;

    [Header("Texts")]
    [Tooltip("The FOV value text")]
    [SerializeField] private TextMeshProUGUI fovAmountText;
    [Tooltip("The resolution text")]
    [SerializeField] private TextMeshProUGUI resText;
    [Tooltip("The fps text")]
    [SerializeField] private TextMeshProUGUI fpsText;

    [Header("Volume")]
    [Tooltip("The volume")]
    [SerializeField] private Volume volume;

    private Resolution[] resolution;
    private int selectedResolution;
    private Camera cam;

    //Post processing
    private Bloom bloom;
    private MotionBlur motionBlur;
    private ScreenSpaceReflection ssr;
    private AmbientOcclusion AO;

    private void Start()
    {
        volume.sharedProfile.TryGet(out bloom);
        volume.sharedProfile.TryGet(out motionBlur);
        volume.sharedProfile.TryGet(out ssr);
        volume.sharedProfile.TryGet(out AO);

        cam = Camera.main;

        //GameManager.instance.UpdateFPSText = fpsCounterToggle.isOn;

        if (rayTracingToggle)
        {
            if (ssr.rayTracing.value == true && AO.rayTracing.value == true)
                rayTracingToggle.isOn = true;
            else
                rayTracingToggle.isOn = false;
        }

        if (fovAmountText)
            fovAmountText.text = fovSlider.value.ToString();

        resolution = Screen.resolutions;

        for (int i = 0; i < resolution.Length; i++)
        {
            if (resolution[i].width == Screen.width & resolution[i].height == Screen.height)
            {
                selectedResolution = i;
                SetResolutionText(resolution[selectedResolution]);
            }
        }

        motionBlurToggle.isOn = motionBlur.active;
        fullscreenToggle.isOn = Screen.fullScreen;
        vSyncToggle.isOn = QualitySettings.vSyncCount == 0 ? false : true;

        //if (PlayerPrefs.HasKey("SelectedResolution"))
        LoadData();
    }

    /// <summary>
    /// Go to the previous resolution
    /// </summary>
    public void ResolutionLeft()
    {
        if (selectedResolution > 0)
            selectedResolution--;
        if (selectedResolution <= 0)
            selectedResolution = resolution.Length - 1;

        SetResolutionText(resolution[selectedResolution]);
    }

    /// <summary>
    /// Go to next resolution
    /// </summary>
    public void ResolutionRight()
    {
        if (selectedResolution < resolution.Length - 1)
            selectedResolution++;
        else if (selectedResolution >= resolution.Length - 1)
            selectedResolution = 0;

        SetResolutionText(resolution[selectedResolution]);
    }

    /// <summary>
    /// Apply the resolution to screen
    /// </summary>
    public void SetResolution()
    {
        Screen.SetResolution(resolution[selectedResolution].width, resolution[selectedResolution].height, fullscreenToggle.isOn);
        PlayerPrefs.SetInt("SelectedResolution", selectedResolution);
    }

    /// <summary>
    /// Set the resolution value to text
    /// </summary>
    /// <param name="resolution"></param>
    public void SetResolutionText(Resolution resolution)
    {
        resText.text = resolution.width + " x " + resolution.height;
    }

    /// <summary>
    /// Set the FOV value to camera
    /// </summary>
    private void SetFOV()
    {
        cam.fieldOfView = fovSlider.value;
        PlayerPrefs.SetFloat("FOVSliderValue", fovSlider.value);
    }

    /// <summary>
    /// Adjust your FOV view
    /// </summary>
    public void UpDateFOVText()
    {
        fovAmountText.text = fovSlider.value.ToString();
    }

    /// <summary>
    /// Enable or disable the fps counter
    /// </summary>
    public void EnableFPSCounter()
    {
        fpsText.enabled = fpsCounterToggle.isOn;
        //GameManager.instance.UpdateFPSText = fpsCounterToggle.isOn;
        PlayerPrefs.SetInt("FPSCounter", BoolToInt(fpsCounterToggle.isOn));
    }

    /// <summary>
    /// Set the quality of bloom
    /// </summary>
    public void SetBloom()
    {
        bloom.quality.value = bloomDropDown.value;
        PlayerPrefs.SetInt("BloomValue", bloomDropDown.value);
    }

    /// <summary>
    /// Enable or disable or set the quality of motion blur
    /// </summary>
    public void SetMotionBlur()
    {
        motionBlur.active = motionBlurToggle.isOn;
        motionBlur.quality.value = motionBlurDropDown.value;
        PlayerPrefs.SetInt("MotionBlur", BoolToInt(motionBlurToggle.isOn));
        PlayerPrefs.SetInt("MotionBlurValue", motionBlurDropDown.value);
    }

    /// <summary>
    /// Set the quality of screen space reflections
    /// </summary>
    public void SetSSR()
    {
        ssr.quality.value = ssrDropDown.value;
        PlayerPrefs.SetInt("SSRValue", ssrDropDown.value);
    }

    /// <summary>
    /// Set the quality of ambient occlusion
    /// </summary>
    public void SetAO()
    {
        AO.quality.value = aoDropDown.value;
        PlayerPrefs.SetInt("AOValue", aoDropDown.value);
    }

    /// <summary>
    /// Enable or disable ray tracing
    /// </summary>
    public void EnableRayTracing()
    {
        ssr.rayTracing.value = rayTracingToggle.isOn;
        AO.rayTracing.value = rayTracingToggle.isOn;
        PlayerPrefs.SetInt("RayTracing", BoolToInt(rayTracingToggle.isOn));
    }

    /// <summary>
    /// Enable or disable full screen
    /// </summary>
    public void ApplyFullScreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn ? true : false;
        PlayerPrefs.SetInt("FullScreen", BoolToInt(fullscreenToggle.isOn));
    }

    /// <summary>
    /// Enable or disable VSync
    /// </summary>
    public void ApplyVSync()
    {
        //1 = on 0 = off
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", BoolToInt(vSyncToggle.isOn));
    }

    /// <summary>
    /// Applies all settings
    /// </summary>
    public void ApplyChanges()
    {
        SetResolution();
        ApplyFullScreen();
        ApplyVSync();
        //SetFOV();
        SetBloom();
        SetMotionBlur();
        //SetSSR();
        //SetAO();
        //EnableRayTracing();
        EnableFPSCounter();
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load the settings data
    /// </summary>
    private void LoadData()
    {
        selectedResolution = PlayerPrefs.GetInt("SelectedResolution");
        if (PlayerPrefs.HasKey("FOVSliderValue"))
            fovSlider.value = PlayerPrefs.GetInt("FOVSliderValue");
        fpsText.enabled = IntToBool(PlayerPrefs.GetInt("FPSCounter"));
        bloomDropDown.value = PlayerPrefs.GetInt("BloomValue");
        motionBlur.active = IntToBool(PlayerPrefs.GetInt("MotionBlur"));
        motionBlurDropDown.value = PlayerPrefs.GetInt("MotionBlurValue");
        if (PlayerPrefs.HasKey("RayTracing"))
            ssrDropDown.value = PlayerPrefs.GetInt("SSRValue");
        if (PlayerPrefs.HasKey("AOValue"))
            aoDropDown.value = PlayerPrefs.GetInt("AOValue");
        if (PlayerPrefs.HasKey("RayTracing"))
            rayTracingToggle.isOn = IntToBool(PlayerPrefs.GetInt("RayTracing"));
        fullscreenToggle.isOn = IntToBool(PlayerPrefs.GetInt("FullScreen"));
        vSyncToggle.isOn = IntToBool(PlayerPrefs.GetInt("VSync"));
        ApplyChanges();
    }

    /// <summary>
    /// Convert a bool to int
    /// </summary>
    /// <param name="value">The bool value</param>
    /// <returns></returns>
    private int BoolToInt(bool value)
    {
        if (value)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// Convert int to bool
    /// </summary>
    /// <param name="value">The int value</param>
    /// <returns></returns>
    private bool IntToBool(int value)
    {
        if (value != 0)
            return true;
        else
            return false;
    }
}
