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
    [Tooltip("The countdown timer text")]
    [SerializeField] private TextMeshProUGUI countdownTimerText;
    [Tooltip("How many seconds it takes before the race starts")]
    [SerializeField] private int countdownTime = 3;
    [Tooltip("The trial timer text")]
    [SerializeField] private TextMeshProUGUI trialTimerText;

    private float countdownTimer;
    private float time;

    public GameState State { get { return state; } set { state = value; } }

    private void Start()
    {
        //State = GameState.Menu;
        countdownTimer = countdownTime;
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
                break;
            case GameState.Race:
                if (Input.GetKeyDown(KeyCode.V))
                    State = GameState.Finish;

                time += Time.deltaTime;

                float minutes = time / 60;
                float seconds = time % 60;
                float fraction = (time * 100) % 100;

                trialTimerText.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
                break;
            case GameState.Finish:
                SaveTrialTime();
                break;
        }
    }

    private void SaveTrialTime()
    {
        //PlayerPrefs.SetInt("Minutes",)
    }

    /// <summary>
    /// Call this to start the countdown
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountdownStart()
    {
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
}
