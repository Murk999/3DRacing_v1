using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour //физика всего автомобиля
{
    [SerializeField] private WheelAxel[] wheelAxles; //ссылка на ось автомобиля
    [SerializeField] private float wheelBaseLength;

    [SerializeField] private Transform centerOfMass; //центр массы

    [Header("Down Force")]
    [SerializeField] private float downForceMin; //Минимальная сила
    [SerializeField] private float downForceMax; //максимальная сила
    [SerializeField] private float downForceFactor; //на сколько увеличивается в зависимости от скорости

    [Header("AngularDrag")]
    [SerializeField] private float angularDragMin; //минимальное сопротивление
    [SerializeField] private float angularDragMax; //максимальное сопротивление
    [SerializeField] private float angularDragFactor; //на сколько увеличивается в зависимости от скорости

    //DEBUG
    public float MotorTorque; // ссылка на максимальный крутящий момент
    public float BrakeTorque; //ссылка на торможение
    public float SteerAngle; //ссылка на угол поворотв

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

    private void FixedUpdate() //обновление колесных осей
    {
        UpdateAngularDrag();  //сопротивления к вращению

        UpdateDownForce(); //прижимная сила

        UpdateWheelAxles();
    }

    public float GetAverageRpm() //среднее вращение всех калес
    {
        float sum = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            sum += wheelAxles[i].GetAvarageRpm();
        }

        return sum / wheelAxles.Length;
    }

    public float GetWheelSpeed() //скорость оборота колеса
    {
        return GetAverageRpm() * wheelAxles[0].GetRadius() * 2 * 0.1885f;
    }

    private void UpdateAngularDrag() //сопротивления к вращению
    {
        rigidbody.angularDrag = Mathf.Clamp(angularDragFactor * LinearVelocity, angularDragMin, angularDragMax);
    }

    private void UpdateDownForce() //прижимная сила
    {
        float downForce = Mathf.Clamp(downForceFactor * LinearVelocity, downForceMin, downForceMax);
        rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdateWheelAxles() //обновление колесных осей
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < wheelAxles.Length; i++)  //если колесная ось является мотором 
        {
            if (wheelAxles[i].IsMotor == true)         //,то прибавляется количество маторных калес
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
}
