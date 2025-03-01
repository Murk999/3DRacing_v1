using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected bool Interactable; // ����� �� ����������������� � �������

    private bool focus = false; // � ������ ������ ��� ��� 
    public bool Focus => focus;

    public UnityEvent OnClick; // ������� ����� ������� 

    public event UnityAction<UIButton> PointerEnter;
    public event UnityAction<UIButton> PointerExit;
    public event UnityAction<UIButton> PointerClick;

    // ��� ����������� ������ ������� ��������� �������

    public virtual void SetFocus() // ���� � ������ 
    {
        if (Interactable == false)  return; // ���� �� ����� ����������������� � ������� �� ������� �� ������ 

        focus = true;
    }

    public virtual void SetUnFocus() // ���� �� � ������
    {
        if(Interactable == false) return; // ���� �� ����� ����������������� � ������� �� ������� �� ������ 

        focus = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Interactable == false) return; // ���� �� ����� ����������������� � ������� �� ������� �� ������ 
        PointerEnter?.Invoke(this); // �������� ������� 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Interactable == false) return; // ���� �� ����� ����������������� � ������� �� ������� �� ������ 
        PointerExit?.Invoke(this);  // �������� ������� 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Interactable == false) return; // ���� �� ����� ����������������� � ������� �� ������� �� ������ 
        PointerClick?.Invoke(this);  // �������� ������� 
        OnClick?.Invoke();
    } 
}