using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraFovCorrector : CarCameraComponent //динамический угол обзора
{

    [SerializeField] private float minFieldView; //значение минимального
    [SerializeField] private float maxFieldView; //значение максимального

    private float defaultFov;

    private void Start()
    {
        camera.fieldOfView = defaultFov;
    }

    private void Update()
    {
        camera.fieldOfView = Mathf.Lerp(minFieldView, maxFieldView, car.NormalizeLinearVelocity);
    }

}
