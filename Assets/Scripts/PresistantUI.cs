using UnityEngine;

// just making sure its not destroyed when loading the game scene
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}