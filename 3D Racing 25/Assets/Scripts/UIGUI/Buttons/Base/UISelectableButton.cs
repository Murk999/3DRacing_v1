using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISelectableButton : UIButton
{
    [SerializeField] private Image selectImage; // Ссылка на картинку выбора кнопки

    public UnityEvent OnSelect;
    public UnityEvent OnUnSelect;

    public override void SetFocus()
    {
        base.SetFocus(); // базовые методы тоже оставляем 

        selectImage.enabled = true; // Если навели на кнопку то включаем картинку , полупрозрачная которая обозначает выбор

        OnSelect?.Invoke();
    }
    public override void SetUnFocus()
    {
        base.SetUnFocus(); // базовые методы тоже оставляем 

        selectImage.enabled = false; // Если не навели на кнопку то выключаем картинку , полупрозрачная которая обозначает выбор

        OnUnSelect?.Invoke();
    }
}