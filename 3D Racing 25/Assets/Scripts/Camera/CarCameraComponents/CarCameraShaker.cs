using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraShaker : CarCameraComponent //Тряска камеры
{
    [SerializeField][Range(0.0f, 1.0f)] private float normalizeSpeedShake;
    [SerializeField] private float shakeAmount; //тряска

    private void Update()
    {
        if (car.NormalizeLinearVelocity >= normalizeSpeedShake)
        transform.localPosition += Random.insideUnitSphere * shakeAmount * Time.deltaTime;
    }
}
