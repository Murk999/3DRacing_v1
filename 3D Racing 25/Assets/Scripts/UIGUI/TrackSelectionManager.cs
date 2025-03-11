using UnityEngine;
using UnityEngine.UI;

public class TrackSelectionManager : MonoBehaviour
{
    [SerializeField] private string[] m_TrackNames;
       
    [SerializeField] private GameObject[] m_Objects;

    private void Start()
    {
        RaceResultTime.UnlockedTrackIndex = PlayerPrefs.GetInt("UnlockedTrackIndex", 0);

        for (int i = 0; i < m_TrackNames.Length; i++)
        {
            if (i <= RaceResultTime.UnlockedTrackIndex)
            {
                m_Objects[i].SetActive(true);
            }
            else
            {
                m_Objects[i].SetActive(false);
            }            
        }         
    }
}
