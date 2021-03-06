using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [Tooltip("How many seconds it takes before the race starts")]
    [SerializeField] private int countdownTime = 3;
    [Tooltip("How much space there is between the trial info's")]
    [SerializeField] private Vector2 trialOffset;

    private GameManager gameManager;
    private InterfaceManager interFace;
    private bool IsCountingDown;
    private float countdownTimer;
    private float time;
    private float minutes;
    private float seconds;
    private float fraction;
    private float xOffset;
    private float yOffset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        interFace = InterfaceManager.instance;
        countdownTimer = countdownTime;

        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetInt("AmountOfSaves") > 0)
            LoadTrialTimes();
    }

    private void Update()
    {
        if (gameManager.State == GameManager.GameState.Race)
        {
            time += Time.deltaTime;

            minutes = Mathf.FloorToInt(time / 60);
            seconds = Mathf.FloorToInt(time % 60);
            fraction = Mathf.FloorToInt((time * 100) % 100);

            interFace.trialTimerText.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
        }

        if (gameManager.State == GameManager.GameState.Start)
            if (!IsCountingDown)
                StartCoroutine(CountdownStart());
    }

    /// <summary>
    /// Call this to start the countdown
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountdownStart()
    {
        IsCountingDown = true;

        while (countdownTimer > 0)
        {
            interFace.countdownTimerText.text = countdownTimer.ToString();
            interFace.countdownTimerText.text = string.Format("{0}", countdownTimer);

            yield return new WaitForSeconds(1);

            countdownTimer--;
        }

        interFace.countdownTimerText.text = string.Format("RACE");
        gameManager.State = GameManager.GameState.Race;

        yield return new WaitForSeconds(1);

        interFace.countdownTimerText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Save the current trial time
    /// </summary>
    public void SaveTrialTime()
    {
        PlayerPrefs.SetInt("AmountOfSaves", gameManager.CurrentAmountOfTrialSaves);
        PlayerPrefs.SetInt("Minutes" + gameManager.CurrentAmountOfTrialSaves, (int)minutes);
        PlayerPrefs.SetInt("Seconds" + gameManager.CurrentAmountOfTrialSaves, (int)seconds);
        PlayerPrefs.SetInt("Fractions" + gameManager.CurrentAmountOfTrialSaves, (int)fraction);
    }

    /// <summary>
    /// Load the saved trial times
    /// </summary>
    public void LoadTrialTimes()
    {
        gameManager.CurrentAmountOfTrialSaves = PlayerPrefs.GetInt("AmountOfSaves");

        UpdateFastestTimeAndScore();

        for (int i = 1; i <= gameManager.CurrentAmountOfTrialSaves; i++)
        {
            
            if (i == 6)
            {
                xOffset = trialOffset.x;
                yOffset = 0;
            }

            GameObject info = Instantiate(gameManager.trialInfoPrefab, new Vector3(interFace.generalTransform.position.x + xOffset, interFace.generalTransform.position.y - yOffset, interFace.generalTransform.position.z), Quaternion.identity, interFace.generalTransform.parent);
            info.transform.SetParent(interFace.generalTransform);

            yOffset += trialOffset.y;

            float minutes = PlayerPrefs.GetInt("Minutes" + i);
            float seconds = PlayerPrefs.GetInt("Seconds" + i);
            float fraction = PlayerPrefs.GetInt("Fractions" + i);

            TrialInfo trialInfo = info.GetComponent<TrialInfo>();
            trialInfo.trialTimeText.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
        }
    }

    /// <summary>
    /// Checks if current trial time is less then fastest trial time
    /// </summary>
    public void CheckFastestTime()
    {
        if (minutes <= PlayerPrefs.GetInt("FastestMinutes") &&
            seconds <= PlayerPrefs.GetInt("FastestSeconds") &&
            fraction <= PlayerPrefs.GetInt("FastestFractions"))
        {
            SaveFastestTime();
            UpdateFastestTimeAndScore();
        }
    }

    /// <summary>
    /// Call this to save the new fastest time and highscore
    /// </summary>
    public void SaveFastestTime()
    {
        PlayerPrefs.SetInt("FastestMinutes", (int)minutes);
        PlayerPrefs.SetInt("FastestSeconds", (int)seconds);
        PlayerPrefs.SetInt("FastestFractions", (int)fraction);
    }

    /// <summary>
    /// Call this to update the fastest time and highscore
    /// </summary>
    public void UpdateFastestTimeAndScore()
    {
        float fastestMinutes = PlayerPrefs.GetInt("FastestMinutes");
        float fastestSeconds = PlayerPrefs.GetInt("FastestSeconds");
        float fastestFraction = PlayerPrefs.GetInt("FastestFractions");
        if (interFace.fastestTrialTimerText)
            interFace.fastestTrialTimerText.text = string.Format("{0:00} : {1:00} : {2:00}", fastestMinutes, fastestSeconds, fastestFraction);
    }
}
