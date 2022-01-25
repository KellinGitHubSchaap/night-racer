using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Tooltip("How many seconds it takes before the race starts")]
    [SerializeField] private int countdownTime = 3;

    private bool IsCountingDown;
    private float countdownTimer;
    private float time;
    private float minutes;
    private float seconds;
    private float fraction;

    private void Start()
    {
        countdownTimer = countdownTime;

        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetInt("AmountOfSaves") > 0)
            LoadTrialTimes();
    }

    private void Update()
    {
        time += Time.deltaTime;

        minutes = Mathf.FloorToInt(time / 60);
        seconds = Mathf.FloorToInt(time % 60);
        fraction = Mathf.FloorToInt((time * 100) % 100);
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
            //countdownTimerText.text = countdownTimer.ToString();
            //countdownTimerText.text = string.Format("{0}", countdownTimer);

            yield return new WaitForSeconds(1);

            countdownTimer--;
        }

        //countdownTimerText.text = string.Format("RACE");
        //State = GameState.Race;

        yield return new WaitForSeconds(1);

        //countdownTimerText.gameObject.SetActive(false);
    }
}
