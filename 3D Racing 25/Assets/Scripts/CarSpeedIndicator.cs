using UnityEngine.UI;
using UnityEngine;

public class CarSpeedIndicator : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private Text text;

    private void Update()
    {
        text.text = car.LinearVelecity.ToString("F0");
    }
}
