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
    public GameObject trialInfoPrefab;

    public static GameManager instance;

    private int currentAmountOfTrialSaves;

    public GameState State { get { return state; } set { state = value; } }
    public int CurrentAmountOfTrialSaves { get { return currentAmountOfTrialSaves; } set { currentAmountOfTrialSaves = value; } }

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
            //DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //State = GameState.Menu;

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
        if(state == GameState.Finish)
        {
            InterfaceManager.instance.ShowWinMenu();
            FinishedRace();
        }
    }

    /// <summary>
    /// This is called when the player reaches the finish line
    /// </summary>
    private void FinishedRace()
    {
        state = GameState.Menu;

        if (currentAmountOfTrialSaves < maxAmountOfTrialSaves)
        {
            currentAmountOfTrialSaves++;
            TimeManager.instance.SaveTrialTime();
            TimeManager.instance.CheckFastestTime();
            if (currentAmountOfTrialSaves == 1)
            {
                TimeManager.instance.SaveFastestTime();
                TimeManager.instance.UpdateFastestTimeAndScore();
            }
        }
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
