using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour //информационная модель автомобиля
{
    public event UnityAction<string> GearChanged;
    [SerializeField] private new AudioSource audio;

    [SerializeField] private float maxSteerAngle; //ссыока на макс поворот
    [SerializeField] private float maxBrakeTorque; //ссылка на макс тормоз

    [Header("Engine")] 
    [SerializeField] private AnimationCurve engineTorqueCurve; //кривая которая показывает крутящий момент
    [SerializeField] private float engineMaxTorque; //максимально крутящий момент
    [SerializeField] private float engineTorque; // текущий крутящий момент
    [SerializeField] private float engineRpm; //количество тякущих оборотов двигателя
    [SerializeField] private float engineMinRpm; //минимальный оборот двигателя 
    [SerializeField] private float engineMaxRpm; //максимальный оборот двигателя

    [Header("Gearbox")]
    [SerializeField] private float[] gears;
    [SerializeField] private float finalDriveRatio;

    [SerializeField] private int selectedGearIndex; //какая прередача включена

    [SerializeField] private float selectedGear;
    [SerializeField] private float rearGear;
    [SerializeField] private float upShiftEngineRpm;
    [SerializeField] private float downShiftEngineRpm;

    [SerializeField] private int maxSpeed; //значение для максиальной скорости
    
    public float LinearVelecity => chassis.LinearVelocity; //ссылается на шасси
    public float NormalizeLinearVelecity => chassis.LinearVelocity / maxSpeed;
    public float WheelSpeed => chassis.GetWheelSpeed();
    public float MaxSpeed => maxSpeed;

    public float EngineRmp => engineRpm;
    public float EngineMaxRpm => engineMaxRpm;


    private CarChassis chassis; //ссылка на калеса

    [SerializeField] private float linearVelecity;
    public float ThrottleControl; //педаль газа
    public float SteerControl; //поворот
    public float BrakeControl; //тормоз

    private void Start()
    {
        chassis = GetComponent<CarChassis>(); //берем компонент
    }

    private void Update() //упарвление физикой автомобиля
    {
        linearVelecity = LinearVelecity;

        UpdateEngineTorque();

        AutoGearShift();

        if (LinearVelecity >= maxSpeed)
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

    private void AutoGearShift() //автоматическое перечключение
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

    public void UpGear() //верхняя передача
    {
        ShiftGear(selectedGearIndex + 1);
    }

    public void DownGear() //опустить передачу вниз
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

    private void ShiftGear(int gearIndex)  //включает передачу 
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

        engineTorque = engineTorqueCurve.Evaluate(engineRpm / engineMaxRpm) * engineMaxTorque * finalDriveRatio * Mathf.Sign(selectedGear) * gears[0]; //вычисление крутящего момета
    }
}
