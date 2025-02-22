using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFovCorrector : MonoBehaviour //динамический угол обзора
{
    [SerializeField] private Car car; //ссылка на автомобиль
    [SerializeField] private new Camera camera; //ссылка на камеру 

    [SerializeField] private float minFieldofView; //значение минимального
    [SerializeField] private float maxFieldofView; //значение максимального

    private float defaultFov;

    private void Start()
    {
        camera.fieldOfView = defaultFov;
    }

    private void Update()
    {
        camera.fieldOfView = Mathf.Lerp(minFieldofView, maxFieldofView, car.NormalizeLinearVelecity);
    }

}
