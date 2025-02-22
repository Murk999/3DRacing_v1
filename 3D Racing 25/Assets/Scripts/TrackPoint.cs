using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackPoint : MonoBehaviour
{
    public event UnityAction<TrackPoint> Triggered;
    public TrackPoint Next;
    public bool IsFirst; // стартовая точка
    public bool IsLast; // последняя точка
    
    protected bool isTarget; // целевая
    public bool IsTarget => isTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<Car>() == null) return;
        
        Triggered?.Invoke(this);
    }

    public void Passed()
    {
        isTarget = false;
    }

    public void AssignAsTarget()
    {
        isTarget = true;
    }
    public void Reset()
    {
        Next = null;
        IsFirst = false;
        IsLast = false;
    }
}
