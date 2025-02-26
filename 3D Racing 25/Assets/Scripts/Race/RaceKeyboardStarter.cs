using UnityEngine;

public class RaceKeyboardStarter : MonoBehaviour, IDependency<RaceStateTracker>
{
    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;
    [SerializeField] private GameObject PressEnter;

    private void Start()
    {
        PressEnter.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) == true)
        {
            raceStateTracker.LaunchPreparationStarted();
            PressEnter.SetActive(false);
        } 
    }
}
