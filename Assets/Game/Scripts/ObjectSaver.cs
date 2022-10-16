using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSaver : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += AddCamera;
        DontDestroyOnLoad(gameObject);
    }

    private void AddCamera(Scene scene, LoadSceneMode sceneMode)
    {
        var canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
}
