using System;
using UnityEngine;

[System.Serializable] //чтобы увидеть параметры
public class WheelAxel // физика колесной оси
{
    [SerializeField] private WheelCollider leftWheelCollider; //ссылка на левое колесо
    [SerializeField] private WheelCollider rightWheelCollider; //ссылка на правое колесо

    //ссылка на трансформы калес
    [SerializeField] private Transform leftWheelMech;
    [SerializeField] private Transform rightWheelMech;

    [SerializeField] private bool isMotor; //ссылка на крутящий момент
    [SerializeField] private bool isSteer; //ссылка на поворачивающий момент

    [SerializeField] private float wheelWidth; //ширина калес

    [SerializeField] private float antiRollForce;

    [SerializeField] private float additionalWheelDownForce;

    [SerializeField] private float baseForwardStiffnes = 1.5f;
    [SerializeField] private float stabilityForwardFactor = 1.0f;

    [SerializeField] private float baseSidewaysStiffnes = 2.0f;
    [SerializeField] private float stabilitySidewaysFactor = 1.0f;

    private WheelHit leftWheelHit;
    private WheelHit rightWheelHit;

    public bool IsMotor => isMotor;
    public bool IsSteer => isSteer;

    // Public API
    //обновление различных параметров
    public void Update()
    {
        UpdateWheelHits();

        ApplyAntiRoll(); //стабилизатор поперечной устойчивости
        ApplyDownForce(); //рижимная сила калес
        CorrectStiffness(); //общая сила трения калес

        SyncMechTransform(); //синхронизирует колайдеры и трансформеры
    }

    public float GetAvarageRpm()
    {
        return (leftWheelCollider.rpm + rightWheelCollider.rpm) * 0.5f;
    }

    public float GetRadius()
    {
        return leftWheelCollider.radius;
    }

    private void UpdateWheelHits()
    {
        leftWheelCollider.GetGroundHit(out leftWheelHit);
        rightWheelCollider.GetGroundHit(out rightWheelHit);
    }

    public void ConfigureVehiclesubsteps(float speedThreshold, int speedBelowThreshold, int stepsAboveThreshold)
    {
        leftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
        rightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
    }

    private void CorrectStiffness() //общая сила трения калес
    {
        WheelFrictionCurve leftForward = leftWheelCollider.forwardFriction;
        WheelFrictionCurve rightForward = rightWheelCollider.forwardFriction;

        WheelFrictionCurve leftSideways = leftWheelCollider.sidewaysFriction;
        WheelFrictionCurve rightSideways = rightWheelCollider.sidewaysFriction;

        leftForward.stiffness = baseForwardStiffnes + Mathf.Abs(leftWheelHit.forwardSlip) * stabilityForwardFactor;
        rightForward.stiffness = baseForwardStiffnes + Mathf.Abs(rightWheelHit.forwardSlip) * stabilityForwardFactor;

        leftSideways.stiffness = baseSidewaysStiffnes + Mathf.Abs(leftWheelHit.sidewaysSlip) * stabilitySidewaysFactor;
        rightSideways.stiffness = baseSidewaysStiffnes + Mathf.Abs(rightWheelHit.sidewaysSlip) * stabilitySidewaysFactor;

        leftWheelCollider.forwardFriction = leftForward;
        rightWheelCollider.forwardFriction = rightForward;

        leftWheelCollider.sidewaysFriction = leftSideways;
        rightWheelCollider.sidewaysFriction = rightSideways;
    }

    private void ApplyDownForce() //рижимная сила калес
    {
        if (leftWheelCollider.isGrounded == true)
        {
            leftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelHit.normal *
                -additionalWheelDownForce * leftWheelCollider.attachedRigidbody.velocity.magnitude, leftWheelCollider.transform.position);
        }

        if (rightWheelCollider.isGrounded == true)
        {
            rightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelHit.normal *
                -additionalWheelDownForce * rightWheelCollider.attachedRigidbody.velocity.magnitude, rightWheelCollider.transform.position);
        }
    }

    private void ApplyAntiRoll() //стабилизатор поперечной устойчивости
    {
        float travelL = 1.0f;
        float travelR = 1.0f;

        if (leftWheelCollider.isGrounded == true)
            travelL = (-leftWheelCollider.transform.InverseTransformPoint(leftWheelHit.point).y - leftWheelCollider.radius) / leftWheelCollider.suspensionDistance;

        if (rightWheelCollider.isGrounded == true)
            travelR = (-rightWheelCollider.transform.InverseTransformPoint(rightWheelHit.point).y - rightWheelCollider.radius) / rightWheelCollider.suspensionDistance;

        float forceDir = (travelL - travelR); //направление в лево и право

        if (leftWheelCollider.isGrounded == true) //если левое колесо на земле
        {
            leftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelCollider.transform.up * -forceDir * antiRollForce, leftWheelCollider.transform.position);
        }

        if (rightWheelCollider.isGrounded == true) //если правое  колесо на земле
        {
            rightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelCollider.transform.up * forceDir * antiRollForce, rightWheelCollider.transform.position);
        }
    }

    public void ApplySteerAngle(float steerAngle, float wheelBaseLength)
    {
        if (isSteer == false) return;  //если ось не поворачиваемая,то выходим из метода

        //Угол Аккермана
        float raidus = Mathf.Abs(wheelBaseLength * Mathf.Tan(Mathf.Deg2Rad * ( 90 - Mathf.Abs(steerAngle)) )); //вычисляем радиус
        float angleSing = Mathf.Sign(steerAngle);

        if (steerAngle > 0)
        {
            leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (raidus + (wheelWidth * 0.5f))) * angleSing;
            rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (raidus - (wheelWidth * 0.5f))) * angleSing;
        }
        else if (steerAngle < 0)
        {
            leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (raidus - (wheelWidth * 0.5f))) * angleSing;
            rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (raidus + (wheelWidth * 0.5f))) * angleSing;
        }
        else
        {
            leftWheelCollider.steerAngle = 0;
            rightWheelCollider.steerAngle = 0;
        }
    }

    public void ApplyMotorTorque(float motorTorque) //мотор 
    {
        if (isMotor == false) return;

        leftWheelCollider.motorTorque = motorTorque;
        rightWheelCollider.motorTorque = motorTorque;
    }

    public void ApplyBreakTorque(float brakeTorque) //тормоз
    {
        leftWheelCollider.brakeTorque = brakeTorque;
        rightWheelCollider.brakeTorque = brakeTorque;
    }


    // Private
    private void SyncMechTransform() //синхронизирует колайдеры и трансформеры
    {
        UpdateWheelTransform(leftWheelCollider, leftWheelMech);
        UpdateWheelTransform(rightWheelCollider, rightWheelMech);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position; //позиция
        Quaternion rotation; //ротация

        wheelCollider.GetWorldPose(out position, out rotation); //задаем параметр
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
