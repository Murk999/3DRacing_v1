using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour //�������������� ������ ����������
{
    public event UnityAction<string> GearChanged;
    [SerializeField] private new AudioSource audio;

    [SerializeField] private float maxSteerAngle; //������ �� ���� �������
    [SerializeField] private float maxBrakeTorque; //������ �� ���� ������

    [Header("Engine")] 
    [SerializeField] private AnimationCurve engineTorqueCurve; //������ ������� ���������� �������� ������
    [SerializeField] private float engineMaxTorque; //����������� �������� ������
    [SerializeField] private float engineTorque; // ������� �������� ������
    [SerializeField] private float engineRpm; //���������� ������� �������� ���������
    [SerializeField] private float engineMinRpm; //����������� ������ ��������� 
    [SerializeField] private float engineMaxRpm; //������������ ������ ���������

    [Header("Gearbox")]
    [SerializeField] private float[] gears;
    [SerializeField] private float finalDriveRatio;

    [SerializeField] private int selectedGearIndex; //����� ��������� ��������

    [SerializeField] private float selectedGear;
    [SerializeField] private float rearGear;
    [SerializeField] private float upShiftEngineRpm;
    [SerializeField] private float downShiftEngineRpm;

    [SerializeField] private int maxSpeed; //�������� ��� ����������� ��������
    
    public float LinearVelocity => chassis.LinearVelocity; //��������� �� �����
    public float NormalizeLinearVelocity => chassis.LinearVelocity / maxSpeed;
    public float WheelSpeed => chassis.GetWheelSpeed();
    public float MaxSpeed => maxSpeed;

    public float EngineRmp => engineRpm;
    public float EngineMaxRpm => engineMaxRpm;

    private CarChassis chassis; //������ �� ������
    public Rigidbody Rigidbody => chassis == null? GetComponent<CarChassis>().Rigidbody: chassis.Rigidbody;

    [SerializeField] private float linearVelocity;
    public float ThrottleControl; //������ ����
    public float SteerControl; //�������
    public float BrakeControl; //������

    private void Start()
    {
        chassis = GetComponent<CarChassis>(); //����� ���������
    }

    private void Update() //���������� ������� ����������
    {
        linearVelocity = LinearVelocity;

        UpdateEngineTorque();

        AutoGearShift();

        if (LinearVelocity >= maxSpeed)
            engineTorque = 0;

        chassis.MotorTorque = engineTorque * ThrottleControl;
        chassis.SteerAngle = maxSteerAngle * SteerControl;
        chassis.BrakeTorque = maxBrakeTorque * BrakeControl;
    } 

    //Gearbox

    public string GetSelectedGearName()
    {
        if (selectedGear == rearGear) return "R";
        if (selectedGear == 0) return "N";
        return (selectedGearIndex + 1).ToString();
    }

    private void AutoGearShift() //�������������� �������������
    {
        if (selectedGear < 0) return;

        if (engineRpm >= upShiftEngineRpm)
        {
            UpGear();
            audio.Play();
        }

        if (engineRpm < downShiftEngineRpm)
        {
            DownGear();
            audio.Play();
        }
    }

    public void UpGear() //������� ��������
    {
        ShiftGear(selectedGearIndex + 1);
    }

    public void DownGear() //�������� �������� ����
    {
        ShiftGear(selectedGearIndex - 1);
    }

    public void ShiftToReverseGear()
    {
        selectedGear = rearGear;
        GearChanged?.Invoke(GetSelectedGearName());
    }

    public void ShiftToFirstGear()
    {
        ShiftGear(0);
    }

    public void ShiftToNetral() 
    {
        selectedGear = 0;
        GearChanged?.Invoke(GetSelectedGearName());
    }

    private void ShiftGear(int gearIndex)  //�������� �������� 
    {
        gearIndex = Mathf.Clamp(gearIndex, 0, gears.Length - 1);
        selectedGear = gears[gearIndex];
        selectedGearIndex = gearIndex;

        GearChanged?.Invoke(GetSelectedGearName());
    }

    private void UpdateEngineTorque()
    {
        engineRpm = engineMinRpm + Mathf.Abs(chassis.GetAverageRpm() * selectedGear * finalDriveRatio);
        engineRpm = Mathf.Clamp(engineRpm,engineMinRpm, engineMaxRpm);

        engineTorque = engineTorqueCurve.Evaluate(engineRpm / engineMaxRpm) * engineMaxTorque * finalDriveRatio * Mathf.Sign(selectedGear) * gears[0]; //���������� ��������� ������
    }

    public void Reset()
    {
        chassis.Reset();

        chassis.MotorTorque = 0;
        chassis.BrakeTorque = 0;
        chassis.SteerAngle = 0;

        ThrottleControl = 0;
        BrakeControl = 0;
        SteerControl = 0;
    }
    public void Respawn(Vector3 position, Quaternion rotation)
    {
        Reset();

        transform.position = position;
        transform.rotation = rotation;
    }
}
