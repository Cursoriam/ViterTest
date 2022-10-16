using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var gameState = (GameState) Enum.Parse(typeof(GameState),
            PlayerPrefs.GetString(Constants.GameStateParamName, GameState.BeforeStart.ToString()));

        if (gameState != GameState.BeforeStart)
        {
            LoadGameScene();
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(Constants.GameSceneName);
    }
}
