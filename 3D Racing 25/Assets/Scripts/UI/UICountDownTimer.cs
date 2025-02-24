using System;
using UnityEngine;
using UnityEngine.UI;

public class UICountDownTimer : MonoBehaviour
{
    [SerializeField] private RaceStateTracker raceStateTracker;

    [SerializeField] Text text;
    [SerializeField] private Timer countDownTimer;

    private void Start()
    {
        raceStateTracker.PreparationStarted += OnPreparationStarted;
        raceStateTracker.Started += OnRaceStarted;

        text.enabled = false;
    }

    private void OnDestroy()
    {
        raceStateTracker.PreparationStarted -= OnPreparationStarted;
        raceStateTracker.Started -= OnRaceStarted;
    }
    private void OnPreparationStarted()
    {
        text.enabled = true;
        enabled = true;
    }

    private void OnRaceStarted()
    {
        text.enabled = false;
        enabled = false;
    }

    private void Update()
    {
        text.text = countDownTimer.Value.ToString("F0");

        if (text.text == "0")
        {
            text.text = "GO!";
        }
    }
}
