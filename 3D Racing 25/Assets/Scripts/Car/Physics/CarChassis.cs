using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour //������ ����� ����������
{
    [SerializeField] private WheelAxel[] wheelAxles; //������ �� ��� ����������
    [SerializeField] private float wheelBaseLength;

    [SerializeField] private Transform centerOfMass; //����� �����

    [Header("Down Force")]
    [SerializeField] private float downForceMin; //����������� ����
    [SerializeField] private float downForceMax; //������������ ����
    [SerializeField] private float downForceFactor; //�� ������� ������������� � ����������� �� ��������

    [Header("AngularDrag")]
    [SerializeField] private float angularDragMin; //����������� �������������
    [SerializeField] private float angularDragMax; //������������ �������������
    [SerializeField] private float angularDragFactor; //�� ������� ������������� � ����������� �� ��������

    //DEBUG
    public float MotorTorque; // ������ �� ������������ �������� ������
    public float BrakeTorque; //������ �� ����������
    public float SteerAngle; //������ �� ���� ��������

    public float LinearVelocity => rigidbody.velocity.magnitude * 3.6f;

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody == null? GetComponent<Rigidbody>(): rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (centerOfMass != null)
            rigidbody.centerOfMass = centerOfMass.localPosition;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].ConfigureVehiclesubsteps(50, 50, 50);
        }
    }

    private void FixedUpdate() //���������� �������� ����
    {
        UpdateAngularDrag();  //������������� � ��������

        UpdateDownForce(); //��������� ����

        UpdateWheelAxles();
    }

    public float GetAverageRpm() //������� �������� ���� �����
    {
        float sum = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            sum += wheelAxles[i].GetAvarageRpm();
        }

        return sum / wheelAxles.Length;
    }

    public float GetWheelSpeed() //�������� ������� ������
    {
        return GetAverageRpm() * wheelAxles[0].GetRadius() * 2 * 0.1885f;
    }

    private void UpdateAngularDrag() //������������� � ��������
    {
        rigidbody.angularDrag = Mathf.Clamp(angularDragFactor * LinearVelocity, angularDragMin, angularDragMax);
    }

    private void UpdateDownForce() //��������� ����
    {
        float downForce = Mathf.Clamp(downForceFactor * LinearVelocity, downForceMin, downForceMax);
        rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdateWheelAxles() //���������� �������� ����
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < wheelAxles.Length; i++)  //���� �������� ��� �������� ������� 
        {
            if (wheelAxles[i].IsMotor == true)         //,�� ������������ ���������� �������� �����
                amountMotorWheel += 2;
        }

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].Update();

            wheelAxles[i].ApplyMotorTorque(MotorTorque / amountMotorWheel);
            wheelAxles[i].ApplySteerAngle(SteerAngle, wheelBaseLength);
            wheelAxles[i].ApplyBreakTorque(BrakeTorque);
        }
    }

    public void Reset()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
