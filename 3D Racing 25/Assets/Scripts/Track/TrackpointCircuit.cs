using System;
using UnityEngine;
using UnityEngine.Events;

public enum TrackType
{
    Circular,
    Sprint
}

public class TrackPointCircuit : MonoBehaviour
{
    public event UnityAction<TrackPoint> TrackPointTriggered;
    public event UnityAction<int> LapCompleted;

    [SerializeField] private TrackType type;
    public TrackType Type => type;

    private TrackPoint[] points;

    private int lapsCompleted = -1;

    private void Awake()
    {
        BuildCircuit();
    }

    private void Start()
    {
        for(int i = 0; i < points.Length; i++)
        {
            points[i].Triggered += OnTrackPointTriggered;
        }
        points[0].AssignAsTarget();
    }
    [ContextMenu(nameof(BuildCircuit))]
    private void BuildCircuit()
    {
        points = TrackCircuitBuilder.Build(transform, type);
    }

    private void OnTrackPointTriggered(TrackPoint trackPoint)
    {
        if (trackPoint.IsTarget == false) return;
        trackPoint.Passed();
        trackPoint.Next?.AssignAsTarget();

        TrackPointTriggered?.Invoke(trackPoint);

        if (trackPoint.IsLast == true)
        {
            if(type == TrackType.Sprint)
            {
                lapsCompleted++;
                if (type == TrackType.Sprint)
                {
                    LapCompleted?.Invoke(lapsCompleted);
                }
                
                if(type== TrackType.Circular)
                {
                    if(lapsCompleted > 0)
                    {
                        LapCompleted?.Invoke(lapsCompleted);
                    }
                }
            }
        }
    }
}
