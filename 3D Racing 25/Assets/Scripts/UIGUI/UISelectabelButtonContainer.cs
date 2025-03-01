using System;
using UnityEngine;

public class UISelectableButtonContainer : MonoBehaviour
{
    [SerializeField] private Transform buttonsContainer; // ���������� ( ����� ) ������ ����� �������� ������

    public bool Interactable = true; // ����� �� ��������� ������� ������ � ������� ����� ����������������� 

    public void SetInteractable(bool interactable) => Interactable = interactable; // ��� �� �� ������� ������ ��������������� ������ ��� ���  

    private UISelectableButton[] buttons; // ������ ������ ������

    private int selectButtonIndex = 0; // ������� ��������� ������ 

    private void Start()
    {
        buttons = buttonsContainer.GetComponentsInChildren<UISelectableButton>(); // ���� UISelectableButton �� ���� �������� �������� ������ �������
        if (buttons == null)
            Debug.LogError("Button list is empty!");

        for (int i = 0; i < buttons.Length; i++) // �������� �� ������� ���� ������
        {   // ������������� �� ������� ������� �����
            buttons[i].PointerEnter += OnPointerEnter; 
        } 
        if (Interactable ==  false) return;

        buttons[selectButtonIndex].SetFocus(); // �������� ������ ������ ���� ��� Interactable �� ���� ���� ����� �����������������
    }
    private void OnDestroy()
    {
        for (int i = 0; i < buttons.Length; i++) // �������� �� ������� ���� ������
        {   // ������������ �� ������� ������� �����
            buttons[i].PointerEnter -= OnPointerEnter;
        }
    }

    private void OnPointerEnter(UIButton button)
    {
        SelectButton(button); // �������� ����� �� ������ ������
    }

    private void SelectButton(UIButton button)
    {
        if (Interactable == false) return ;

        buttons[selectButtonIndex].SetUnFocus(); // ������� ��������� � ���������� ������ 

        for (int i = 0; i < buttons.Length; i++)
        {
            if(button == buttons[i]) // ���� ������� ������ ����� ������ �� ������� i
            {
                selectButtonIndex = i; // �� ����������� ������ ��������� ������ ������ �� ������� ���������
                button.SetFocus(); // ������� ������
                break; // ������ �� ������
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
