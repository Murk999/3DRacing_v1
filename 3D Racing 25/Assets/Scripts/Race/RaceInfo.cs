using UnityEngine;

[CreateAssetMenu]
public class RaceInfo : ScriptableObject
{
    [SerializeField] private string sceneName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string title;

    public string SceneName => sceneName;
    public Sprite Sprite => sprite;
    public string Title => title;
}
