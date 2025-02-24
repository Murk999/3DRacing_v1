using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private new Camera camera;
    [SerializeField] private CarCameraFollow follower;
    [SerializeField] private CarCameraShaker shaker;
    [SerializeField] private CarCameraFovCorrector fovCorrectorer;
    [SerializeField] private CameraPathFollower pathFollower;

    [SerializeField] private RaceStateTracker raceStateTracker;

    private void Awake()
    {
        follower.SetProperties(car, camera);
        shaker.SetProperties(car, camera);
        fovCorrectorer.SetProperties(car, camera);
    }

    private void Start()
    {
        raceStateTracker.PreparationStarted += OnPreparationStarted;
        raceStateTracker.Completed += OnCompleted;

        follower.enabled = false;
        pathFollower.enabled = true;
    }

    private void OnDestroy()
    {
        raceStateTracker.PreparationStarted -= OnPreparationStarted;
        raceStateTracker.Completed -= OnCompleted;

    }
    private void OnPreparationStarted()
    {
        follower.enabled = true;
        pathFollower.enabled = false;
    }
    private void OnCompleted()
    {
        pathFollower.enabled = true;
        pathFollower.StartMoveToNearestPoint();
        pathFollower.SetLookTarget(car.transform);

        follower.enabled = false;
    }
}
