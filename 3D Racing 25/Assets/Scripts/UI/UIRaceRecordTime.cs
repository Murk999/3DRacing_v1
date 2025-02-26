using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRaceRecordTime : MonoBehaviour, IDependency<RaceResultTime>, IDependency<RaceStateTracker>
{
    [SerializeField] private GameObject goldRecordObject;
    [SerializeField] private GameObject playerRecordObject;
    [SerializeField] private Text goldRecordTime;
    [SerializeField] private Text playerRecordTime;
    [SerializeField] private GameObject playerRaceRecord;
    [SerializeField] private Text playerRaceTime;
    [SerializeField] private RaceTimeTracker raceTimeTracker;
    [SerializeField] private GameObject Record;
    [SerializeField] private Text RecordTime;

    // [SerializeField] private Text recordLable;

    private RaceResultTime raceResultTime;
    public void Construct(RaceResultTime obj) => raceResultTime = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Start()
    {
        raceStateTracker.Started += OnRaceStart;
        raceStateTracker.Completed += OnRaceCompleted;

        goldRecordObject.SetActive(false);  // ++
        playerRecordObject.SetActive(false);//++

        playerRaceRecord.SetActive(false);
        Record.SetActive(false);
    }

    private void OnDestroy()
    {
        raceStateTracker.Started -= OnRaceStart;
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRaceStart()
    {
        // Если текущий рекорд больше чем золотой или             рекорд не установлен то есть первый раз едем
        if (raceResultTime.PlayerRecordTime > raceResultTime.GoldTime || raceResultTime.RecordWasSet == false)
        {
            goldRecordObject.SetActive(true);
            goldRecordTime.text = StringTime.SecondToTimeString(raceResultTime.GoldTime); // То показываем голд время           
        }
        // Если рекорд установлен 
        if (raceResultTime.RecordWasSet == true)
        {
            playerRecordObject.SetActive(true);
            playerRecordTime.text = StringTime.SecondToTimeString(raceResultTime.PlayerRecordTime); // То показываем результат игрока

        }
    }

    private void OnRaceCompleted()
    {
        goldRecordObject.SetActive(false);
        playerRecordObject.SetActive(false);
        playerRaceRecord.SetActive(true);
        Record.SetActive(true);
        playerRaceTime.text = StringTime.SecondToTimeString(raceTimeTracker.CurrentTime);
        if (raceResultTime.RecordWasSet == false && raceTimeTracker.CurrentTime < raceResultTime.GoldTime)
        {
            RecordTime.text = StringTime.SecondToTimeString(raceTimeTracker.CurrentTime);
        }
        else if (raceResultTime.RecordWasSet == true && raceTimeTracker.CurrentTime < raceResultTime.PlayerRecordTime)
        {
            RecordTime.text = StringTime.SecondToTimeString(raceTimeTracker.CurrentTime);
        }
        else
        {
            RecordTime.text = StringTime.SecondToTimeString(raceResultTime.PlayerRecordTime);
        }
    }
}