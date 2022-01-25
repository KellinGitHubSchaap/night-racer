using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Start,
        Race,
        Finish
    }

    [Tooltip("The state of the game")]
    [SerializeField] private GameState state;
    [Tooltip("How many saves you can have")]
    [SerializeField] private int maxAmountOfTrialSaves = 10;
    [Tooltip("The trialinfo prefab")]
    [SerializeField] private GameObject trialInfoPrefab;
    [Tooltip("Where the trialinfo's are placed")]
    [SerializeField] private Transform generalTransform;
    [Tooltip("The countdown timer text")]
    [SerializeField] private TextMeshProUGUI countdownTimerText;
    [Tooltip("How many seconds it takes before the race starts")]
    [SerializeField] private int countdownTime = 3;
    [Tooltip("The trial timer text")]
    [SerializeField] private TextMeshProUGUI trialTimerText;
    [Tooltip("The fastest trial timer text")]
    [SerializeField] private TextMeshProUGUI fastestTrialTimerText;

    public static GameManager instance;

    private float countdownTimer;
    private float time;
    private int currentAmountOfTrialSaves;
    private float minutes;
    private float seconds;
    private float fraction;
    private bool IsCountingDown;

    public GameState State { get { return state; } set { state = value; } }

    [Header("Checkpoint Settings")]
    public GameObject m_checkPointHolder;
    public List<GameObject> m_checkPoints = new List<GameObject>();
    public GameObject m_previousCheckPoint;
    public GameObject m_currentCheckPoint;

    private int m_previousCheckPointID;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //State = GameState.Menu;
        countdownTimer = countdownTime;

        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetInt("AmountOfSaves") > 0)
            LoadTrialTimes();

        // Get All checkpoints
        if (m_checkPointHolder != null)
        {
            for (int i = 0; i < m_checkPointHolder.transform.childCount; i++)
            {
                m_checkPoints.Add(m_checkPointHolder.transform.GetChild(i).gameObject);
            }
        }
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.Menu:
                break;
            case GameState.Start:
                if (Input.GetKeyDown(KeyCode.B))
                    StartCoroutine(CountdownStart());

                if (!IsCountingDown)
                    StartCoroutine(CountdownStart());
                break;
            case GameState.Race:
                if (Input.GetKeyDown(KeyCode.V))
                    FinishedRace();

                time += Time.deltaTime;

                minutes = Mathf.FloorToInt(time / 60);
                seconds = Mathf.FloorToInt(time % 60);
                fraction = Mathf.FloorToInt((time * 100) % 100);

                trialTimerText.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
                break;
            case GameState.Finish:
                InterfaceManager.instance.ShowWinMenu();
                FinishedRace();
                break;
        }
    }

    /// <summary>
    /// This is called when the player reaches the finish line
    /// </summary>
    private void FinishedRace()
    {
        State = GameState.Finish;

        if (currentAmountOfTrialSaves < maxAmountOfTrialSaves)
        {
            currentAmountOfTrialSaves++;
            SaveTrialTime();
            CheckFastestTime();
            if (currentAmountOfTrialSaves == 1)
            {
                SaveFastestTimeAndHighScore();
                UpdateFastestTimeAndScore();
            }
        }
    }

    /// <summary>
    /// Save the current trial time
    /// </summary>
    private void SaveTrialTime()
    {
        PlayerPrefs.SetInt("AmountOfSaves", currentAmountOfTrialSaves);
        PlayerPrefs.SetInt("Minutes" + currentAmountOfTrialSaves, (int)minutes);
        PlayerPrefs.SetInt("Seconds" + currentAmountOfTrialSaves, (int)seconds);
        PlayerPrefs.SetInt("Fractions" + currentAmountOfTrialSaves, (int)fraction);
    }

    /// <summary>
    /// Load the saved trial times
    /// </summary>
    private void LoadTrialTimes()
    {
        currentAmountOfTrialSaves = PlayerPrefs.GetInt("AmountOfSaves");

        UpdateFastestTimeAndScore();

        for (int i = 1; i <= currentAmountOfTrialSaves; i++)
        {
            GameObject info = Instantiate(trialInfoPrefab, generalTransform.position, Quaternion.identity, generalTransform.parent);
            info.transform.SetParent(generalTransform);

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
    private void CheckFastestTime()
    {
        if (minutes <= PlayerPrefs.GetInt("FastestMinutes") &&
            seconds <= PlayerPrefs.GetInt("FastestSeconds") &&
            fraction <= PlayerPrefs.GetInt("FastestFractions"))
        {
            SaveFastestTimeAndHighScore();
            UpdateFastestTimeAndScore();
        }
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
            countdownTimerText.text = countdownTimer.ToString();
            countdownTimerText.text = string.Format("{0}", countdownTimer);

            yield return new WaitForSeconds(1);

            countdownTimer--;
        }

        countdownTimerText.text = string.Format("RACE");
        State = GameState.Race;

        yield return new WaitForSeconds(1);

        countdownTimerText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Call this to save the new fastest time and highscore
    /// </summary>
    private void SaveFastestTimeAndHighScore()
    {
        PlayerPrefs.SetInt("FastestMinutes", (int)minutes);
        PlayerPrefs.SetInt("FastestSeconds", (int)seconds);
        PlayerPrefs.SetInt("FastestFractions", (int)fraction);
    }

    /// <summary>
    /// Call this to update the fastest time and highscore
    /// </summary>
    private void UpdateFastestTimeAndScore()
    {
        float fastestMinutes = PlayerPrefs.GetInt("FastestMinutes");
        float fastestSeconds = PlayerPrefs.GetInt("FastestSeconds");
        float fastestFraction = PlayerPrefs.GetInt("FastestFractions");
        fastestTrialTimerText.text = string.Format("{0:00} : {1:00} : {2:00}", fastestMinutes, fastestSeconds, fastestFraction);
    }

    // Store the new triggered checkpoint 
    public void StoreCurrentCheckPoint(GameObject checkPointPos, int ID)
    {
        if (m_checkPoints.Contains(checkPointPos))
        {
            m_previousCheckPoint = m_currentCheckPoint;

            if (checkPointPos != m_previousCheckPoint && ID > m_previousCheckPointID || m_currentCheckPoint == null)    // If the checkPoint triggered isn't the same as the previous one you are allowed to store it.
            {
                m_currentCheckPoint = checkPointPos;
                m_previousCheckPointID = ID;
            }
            Debug.Log("Checkpoint Set");
        }
        else
        {
            Debug.Log("Error in Sending Checkpoint");
        }
    }

    public void SetGameState(int state)
    {
        State = (GameState)state;
    }
}
