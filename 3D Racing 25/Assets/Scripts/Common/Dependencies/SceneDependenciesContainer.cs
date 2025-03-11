using UnityEngine;

public class SceneDependencies : Dependency
{
    [SerializeField] private RaceStateTracker raceStateTracker;
    [SerializeField] private CarInputControl carInputControl;
    [SerializeField] private TrackPointCircuit trackPointCircuit; // —сылка на поинты
    [SerializeField] private Car car;
    [SerializeField] private CarCameraController carCameraController;
    [SerializeField] private RaceTimeTracker raceTimeTracker;
    [SerializeField] private RaceResultTime raceResultTime;

    protected override void BindAll(MonoBehaviour monoBehaviorInScene)
    {
        Bind<RaceStateTracker>(raceStateTracker, monoBehaviorInScene);
        Bind<CarInputControl>(carInputControl, monoBehaviorInScene);
        Bind<TrackPointCircuit>(trackPointCircuit, monoBehaviorInScene);
        Bind<Car>(car, monoBehaviorInScene);
        Bind<CarCameraController>(carCameraController, monoBehaviorInScene);
        Bind<RaceTimeTracker>(raceTimeTracker, monoBehaviorInScene);
        Bind<RaceResultTime>(raceResultTime, monoBehaviorInScene);
    }

    private void Awake()
    {
        FindAllObjectToBind();
    }
}
