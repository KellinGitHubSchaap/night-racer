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

    private float countdownTimer;

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
                break;
            case GameState.Finish:
                break;
        }
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

            yield return new WaitForSeconds(1);

            countdownTimer--;
        }

        countdownTimerText.text = "RACE";
        State = GameState.Race;

        yield return new WaitForSeconds(1);

        countdownTimerText.gameObject.SetActive(false);
    }
}
