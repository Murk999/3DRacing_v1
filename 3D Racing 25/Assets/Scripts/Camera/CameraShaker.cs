using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour //Тряска камеры
{
    [SerializeField] private Car car;
    [SerializeField][Range(0.0f, 1.0f)] private float normalizeSpeedShake;
    [SerializeField] private float shakeAmount; //тряска

    private void Update()
    {
        if (car.NormalizeLinearVelecity >= normalizeSpeedShake)
        transform.localPosition += Random.insideUnitSphere * shakeAmount * Time.deltaTime;
    }
}
