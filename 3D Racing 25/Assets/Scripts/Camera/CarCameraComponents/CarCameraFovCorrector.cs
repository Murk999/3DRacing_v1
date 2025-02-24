using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraFovCorrector : CarCameraComponent //������������ ���� ������
{

    [SerializeField] private float minFieldView; //�������� ������������
    [SerializeField] private float maxFieldView; //�������� �������������

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
