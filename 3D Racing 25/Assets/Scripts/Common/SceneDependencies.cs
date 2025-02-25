using UnityEngine;

public interface IDependency<T>
{
    void Construct(T obj);
}

public class SceneDependencies : MonoBehaviour
{
    [SerializeField] private RaceStateTracker raceStateTracker;
    [SerializeField] private CarInputControl carInputControl;
    [SerializeField] private TrackPointCircuit trackPointCircuit; // Ссылка на поинты
    [SerializeField] private Car car;
    [SerializeField] private CarCameraController carCameraController;
    //[SerializeField] private RaceTimeTracker raceTimeTracker;
    //[SerializeField] private RaceResultTime raceResultTime;

    private void Bind(MonoBehaviour mono)
    {
        if (mono is IDependency<RaceStateTracker>) (mono as IDependency<RaceStateTracker>).Construct(raceStateTracker);
        if (mono is IDependency<CarInputControl>) (mono as IDependency<CarInputControl>).Construct(carInputControl);
        if (mono is IDependency<TrackPointCircuit>) (mono as IDependency<TrackPointCircuit>).Construct(trackPointCircuit);
        if (mono is IDependency<Car>) (mono as IDependency<Car>).Construct(car);
        if (mono is IDependency<CarCameraController>) (mono as IDependency<CarCameraController>).Construct(carCameraController);
       // if (mono is IDependency<RaceTimeTracker>) (mono as IDependency<RaceTimeTracker>).Construct(raceTimeTracker);
        //if (mono is IDependency<RaceResultTime>) (mono as IDependency<RaceResultTime>).Construct(raceResultTime);
    }

    [System.Obsolete]
    private void Awake()
    {
        MonoBehaviour[] monoInScene = FindObjectsOfType<MonoBehaviour>(); // Находим все монобехи на сцене

        for (int i = 0; i < monoInScene.Length; i++)
        {
            Bind(monoInScene[i]);

        }
    }
}
