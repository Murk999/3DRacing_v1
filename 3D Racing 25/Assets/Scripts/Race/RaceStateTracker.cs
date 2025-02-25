using System;
using UnityEngine;
using UnityEngine.Events;

public enum RaceState
{
    Preparation,
    CountDown,
    Race,
    Passed
}
public class RaceStateTracker : MonoBehaviour, IDependency<TrackPointCircuit>
{
    public event UnityAction PreparationStarted; // подготовка старта
    public event UnityAction Started; // старт 
    public event UnityAction Completed; // завершение 
    public event UnityAction<TrackPoint> TrackPointPassed; // пройденные точки трека
    public event UnityAction<int> LapCompleted; // завершенный круг

    private TrackPointCircuit trackPointCircuit;
    public void Construct(TrackPointCircuit trackPointCircuit) => this.trackPointCircuit = trackPointCircuit;
    [SerializeField] private Timer countdownTimer;
    [SerializeField] private int lapsToComplete;

    private RaceState state;
    public RaceState State => state;

    public Timer CountDownTimer => countdownTimer;
    private void StartState(RaceState state)
    {
        this.state = state;
    }
   
    private void Start()
    {
        StartState(RaceState.Preparation);

        countdownTimer.enabled = false;

        countdownTimer.Finished += OnCountdownTimerFinished;

        trackPointCircuit.TrackPointTriggered += OnTrackPointTrigger;
        trackPointCircuit.LapCompleted += OnLapCompleted;
    }

    private void OnDestroy()
    {
        trackPointCircuit.TrackPointTriggered -= OnTrackPointTrigger;

        trackPointCircuit.TrackPointTriggered -= OnTrackPointTrigger;
        trackPointCircuit.LapCompleted -= OnLapCompleted;
    }

    private void OnTrackPointTrigger(TrackPoint trackPoint)
    {
        TrackPointPassed?.Invoke(trackPoint);
    }

    private void OnCountdownTimerFinished()
    {
        StartRace();
    }

    private void OnLapCompleted(int lapAmount)
    {
        if (trackPointCircuit.Type == TrackType.Sprint)
        {
            CompleteRace();
        }
        if(trackPointCircuit.Type == TrackType.Circular)
        {
            if(lapAmount == lapsToComplete)
            {
                CompleteRace();
            }
            else
            {
                CompleteLap(lapAmount);
            }
        }
    }

    public void LaunchPreparationStarted()
    {
        if (state != RaceState.Preparation) return;
        StartState(RaceState.CountDown);

        countdownTimer.enabled = true;
        PreparationStarted?.Invoke();
    }

    private void StartRace()
    {
        if(state != RaceState.CountDown) return;
        StartState(RaceState.Race);

        Started?.Invoke();
    }

    private void CompleteRace()
    {
        if (state != RaceState.Race) return;
        StartState(RaceState.Passed);
        Completed?.Invoke();
    }

    private void CompleteLap(int lapAmount)
    {
        LapCompleted?.Invoke(lapAmount);
    }
}
