using UnityEngine;

public class ComponentLister : MonoBehaviour
{
    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Component[] components = obj.GetComponents<Component>();
            foreach (Component comp in components)
            {
                Debug.Log($"  - Component: {comp.GetType()}");
            }
        }
    }
}
