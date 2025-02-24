using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class EnigneIndicatorColor
{
    public float MaxRpm;
    public Color color;
}

public class CarEngineIndicator : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private Image image;
    [SerializeField] private EnigneIndicatorColor[] colors;

    private void Update()
    {
        image.fillAmount = car.EngineRmp / car.EngineMaxRpm;
        for (int i = 0; i < colors.Length; i++)
        {
            if (car.EngineRmp <= colors[i].MaxRpm)
            {
                image.color = colors[i].color;
                break;
            }
        }
    }
}
