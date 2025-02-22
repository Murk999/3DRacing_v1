using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFovCorrector : MonoBehaviour //������������ ���� ������
{
    [SerializeField] private Car car; //������ �� ����������
    [SerializeField] private new Camera camera; //������ �� ������ 

    [SerializeField] private float minFieldofView; //�������� ������������
    [SerializeField] private float maxFieldofView; //�������� �������������

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
