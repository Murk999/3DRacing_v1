using UnityEngine;

public class UIPausePanel : MonoBehaviour, IDependency<Pauser>
{
    [SerializeField] private GameObject panel;
    
    private Pauser pauser;
    public void Construct(Pauser obj) => pauser = obj;

    void Start()
    {
        panel.SetActive(false);
        pauser.PauseStateChange += OnPauseStatChanged;
    }

    private void OnDestroy()
    {
        pauser.PauseStateChange -= OnPauseStatChanged;
    }

    private void OnPauseStatChanged(bool isPause)
    {
        panel.SetActive(isPause);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            pauser.ChangePauseState();
        }
    }

    public void Unpause()
    {
        pauser.UnPause();
    }
}
