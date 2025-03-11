using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RaceResultTime : MonoBehaviour, IDependency<RaceTimeTracker>, IDependency<RaceStateTracker>
{
    public const string SaveMark = "_player_best_time";

    public event UnityAction ResultUpdate;
    
    public static int UnlockedTrackIndex;
    
    [SerializeField] private float goldTime;

    private float playerRecordTime;
    private float currentTime;

    public float GoldTime => goldTime;
    public float PlayerRecordTime => playerRecordTime;
    public float CurrentTime => currentTime;
    public bool RecordWasSet => playerRecordTime != 0;
    
    private RaceTimeTracker raceTimeTracker;
    public void Construct(RaceTimeTracker obj) => raceTimeTracker = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        raceStateTracker.Completed += OnRaceCompleted;
        if (UnlockedTrackIndex == 0) goldTime = 10;
        if (UnlockedTrackIndex == 1) goldTime = 55;
        if (UnlockedTrackIndex == 2) goldTime = 60;
        if (UnlockedTrackIndex == 3) goldTime = 100;
    }

    private void OnDestroy()
    {
        raceStateTracker.Completed -= OnRaceCompleted;
    }


    private void OnRaceCompleted()
    {
        float absoluteRecord = GetAbsoluteRecord();

        if(raceTimeTracker.CurrentTime < absoluteRecord || playerRecordTime == 0)
        {
            playerRecordTime = raceTimeTracker.CurrentTime;

            if (raceTimeTracker.CurrentTime < goldTime)
            {
                UnlockedTrackIndex++;
            }

            Save();
        }

        currentTime = raceTimeTracker.CurrentTime;

        ResultUpdate?.Invoke();
    }

    public float GetAbsoluteRecord()
    {
        if(playerRecordTime < goldTime && playerRecordTime != 0)
        {
            return playerRecordTime;
        }
        else
        {
            return goldTime;
        }
    }

    private void Load()
    {
        playerRecordTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + SaveMark, 0);

        if (PlayerPrefs.HasKey("UnlockedTrackIndex"))
        {
            UnlockedTrackIndex = PlayerPrefs.GetInt("UnlockedTrackIndex", 0);
        }
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + SaveMark, playerRecordTime);

        PlayerPrefs.SetInt("UnlockedTrackIndex", UnlockedTrackIndex);
    }

    public int GetUnlockedTrackIndex()
    {
        return UnlockedTrackIndex;
    }
}
