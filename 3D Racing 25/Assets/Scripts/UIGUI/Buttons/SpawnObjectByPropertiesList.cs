using UnityEngine;

public class SpawnObjectByPropertiesList : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ScriptableObject[] properties;

    [ContextMenu(nameof(SpawnInEditMode))]
    public void SpawnInEditMode()
    {
        if (Application.isPlaying == true) return;

        GameObject[] allObject = new GameObject[parent.childCount]; // создаем новый массив по колличеству  детей

        for(int i = 0; i < parent.childCount; i++)
        {
            allObject[i] = parent.GetChild(i).gameObject; // перекинули и пересчитали 
        }

        for(int i = 0; i < allObject.Length; i++)
        {
            DestroyImmediate(allObject[i]); // теперь проходимся по массиву и удаляем 
        }

        for(int i = 0; i < properties.Length; i++)
        {
            GameObject go = Instantiate(prefab, parent);
            IScriptableObjectProperty scriptableObjectProperty = go.GetComponent<IScriptableObjectProperty>();
            scriptableObjectProperty.ApplyProperty(properties[i]);
        }
    }
}
