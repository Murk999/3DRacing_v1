using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected bool Interactable; // Можно ли взаимодействовать с кнопкой

    private bool focus = false; // В фокусе кнопка или нет 
    public bool Focus => focus;

    public UnityEvent OnClick; // Событие когда кликаем 

    public event UnityAction<UIButton> PointerEnter;
    public event UnityAction<UIButton> PointerExit;
    public event UnityAction<UIButton> PointerClick;

    // Два виртуальных метода которые управляют кнопкой

    public virtual void SetFocus() // Если в фокусе 
    {
        if (Interactable == false)  return; // Если не можем взаимодействовать с кнопкой то выходим из метода 

        focus = true;
    }

    public virtual void SetUnFocus() // Если не в фокусе
    {
        if(Interactable == false) return; // Если не можем взаимодействовать с кнопкой то выходим из метода 

        focus = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Interactable == false) return; // Если не можем взаимодействовать с кнопкой то выходим из метода 
        PointerEnter?.Invoke(this); // Включаем событие 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Interactable == false) return; // Если не можем взаимодействовать с кнопкой то выходим из метода 
        PointerExit?.Invoke(this);  // Включаем событие 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Interactable == false) return; // Если не можем взаимодействовать с кнопкой то выходим из метода 
        PointerClick?.Invoke(this);  // Включаем событие 
        OnClick?.Invoke();
    } 
}