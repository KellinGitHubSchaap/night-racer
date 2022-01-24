using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

    [Header("Menu's")]
    [Tooltip("The gameover menu")]
    [SerializeField] private GameObject gameOverMenu;
    [Tooltip("The win menu")]
    [SerializeField] private GameObject winMenu;
    [Tooltip("The paused menu")]
    [SerializeField] private GameObject pauseMenu;
    [Tooltip("Menu's to deactivate")]
    [SerializeField] private GameObject[] menus;

    [Header("TrialTime")]
    [Tooltip("Where the trialinfo's are placed")]
    [SerializeField] private Transform generalTransform;
    [Tooltip("The countdown timer text")]
    [SerializeField] private TextMeshProUGUI countdownTimerText;
    [Tooltip("The trial timer text")]
    [SerializeField] private TextMeshProUGUI trialTimerText;
    [Tooltip("The fastest trial timer text")]
    [SerializeField] private TextMeshProUGUI fastestTrialTimerText;

    [Header("Fade")]
    [Tooltip("The fade panel")]
    [SerializeField] private Image fadeImage;
    [Tooltip("How long the fading takes")]
    [SerializeField] private float fadeDuration = 3;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Call this to deactivate menu's
    /// </summary>
    private void EnableDisableMenus(bool value)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(value);
        }
    }

    /// <summary>
    /// Call this to enable or disable a menu
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="value"></param>
    public void EnableDisableMenu(GameObject menu, bool value)
    {
        menu.SetActive(value);
    }

    /// <summary>
    /// Enable the win menu
    /// </summary>
    public void ShowWinMenu()
    {
        winMenu.SetActive(true);
    }

    /// <summary>
    /// Start the game over coroutine
    /// </summary>
    public void ShowGameOver()
    {
        StartCoroutine(ShowMenuWithFade(1, fadeDuration, 1, 0, gameOverMenu, null));
        EnableDisableMenus(false);
    }

    /// <summary>
    /// Call this to start a fade
    /// </summary>
    /// <param name="fadeDuration">How long it takes for fading</param>
    /// <param name="alphaValue">What value the aplha needs to be</param>
    /// <param name="alphaTarget">Set alpha to 0 or 1</param>
    public void StartFade(float fadeDuration, float alphaValue, float alphaTarget)
    {
        StartCoroutine(ShowMenuWithFade(0.5f, fadeDuration, alphaValue, alphaTarget, null, null));
    }

    /// <summary>
    /// Enable or disable the pause menu
    /// </summary>
    public void ShowPauseMenu(bool isActive)
    {
        if (pauseMenu)
            pauseMenu.SetActive(isActive);
    }

    /// <summary>
    /// Call this to activate the game over
    /// </summary>
    /// <param name="delay">How much delay the fucntion has</param>
    /// <param name="duration">How long it takes for fading</param>
    /// <param name="alphaValue">What value the aplha needs to be</param>
    /// <param name="alphaTarget">Set alpha to 0 or 1</param>
    /// <param name="menu">What menu to enable</param>
    /// <param name="animator">The animator to play animation</param>
    /// <returns></returns>
    private IEnumerator ShowMenuWithFade(float delay, float duration, float alphaValue, float alphaTarget, GameObject menu, Animator animator)
    {
        yield return new WaitForSecondsRealtime(delay);
        CrossFadeAlpha(duration, alphaValue, alphaTarget);
        yield return new WaitForSecondsRealtime(duration);
        if (menu)
            menu.SetActive(true);
        if (animator)
            animator.SetTrigger("Win");
    }

    /// <summary>
    /// THe working CrossFadeAlpha for making image alpha go to 0 or 1 over time
    /// </summary>
    /// <param name="duration">How long it takes for fading</param>
    /// <param name="alphaValue">What value the aplha needs to be</param>
    /// <param name="alphaTarget">Set alpha to 0 or 1</param>
    private void CrossFadeAlpha(float duration, float alphaValue, float alphaTarget)
    {
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;
        fadeImage.CrossFadeAlpha(alphaTarget, 0, true);
        fadeImage.CrossFadeAlpha(alphaValue, duration, true);
    }
}
