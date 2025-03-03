using System;
using UnityEngine;

public class UISelectableButtonContainer : MonoBehaviour
{
    [SerializeField] private Transform buttonsContainer; // трансформа ( место ) откуда берем картинки селект

    public bool Interactable = true; // Можем из редактора выбрать кнопка с которой можем взаимодействовать 

    public void SetInteractable(bool interactable) => Interactable = interactable; // Что бы из скрипта менять взаимодействует кнопка или нет  

    private UISelectableButton[] buttons; // Массив кнопок выбора

    private int selectButtonIndex = 0; // Текущая выбранная кнопка 

    private void Start()
    {
        buttons = buttonsContainer.GetComponentsInChildren<UISelectableButton>(); // Ищем UISelectableButton во всех дочерних объектов нашего объекта
        if (buttons == null)
            Debug.LogError("Button list is empty!");

        for (int i = 0; i < buttons.Length; i++) // Проходим по массиву всех кнопок
        {   // Подписываемся на событие поинтер ентер
            buttons[i].PointerEnter += OnPointerEnter; 
        } 
        if (Interactable ==  false) return;

        buttons[selectButtonIndex].SetFocus(); // Выделяем кнопку только если она Interactable то есть если можем взаимодействовать
    }
    private void OnDestroy()
    {
        for (int i = 0; i < buttons.Length; i++) // Проходим по массиву всех кнопок
        {   // Отписываемся от события поинтер ентер
            buttons[i].PointerEnter -= OnPointerEnter;
        }
    }

    private void OnPointerEnter(UIButton button)
    {
        SelectButton(button); // Выделяем какой то баттон кнопку
    }

    private void SelectButton(UIButton button)
    {
        if (Interactable == false) return ;

        buttons[selectButtonIndex].SetUnFocus(); // Снимаем выделение с предыдущей кнопки 

        for (int i = 0; i < buttons.Length; i++)
        {
            if(button == buttons[i]) // Если текущая кнопка равна кнопке по индексу i
            {
                selectButtonIndex = i; // То присваиваем кнопке выделения индекс кнопки на которую направили
                button.SetFocus(); // Выделяю кнопку
                break; // Выхожу из метода
            }
        }
    }

    public void SelectNext()
    {

    }

    public void SelectPrevious()
    {

    }
}
