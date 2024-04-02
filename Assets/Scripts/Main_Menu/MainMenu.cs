using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjects;
    [SerializeField] private Text _bestPlayerName;
    [SerializeField] private Text _bestPlayerScore;
    [SerializeField] private Text _bestPlayerEnemiesKilled;

    public void LoadInstructions()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadHighScore()
    {
        PlayerDataHandler.Instance.LoadBestPlayerData();
        _bestPlayerName.text = $"Player : {PlayerDataHandler.Instance.bestPlayerName}";
        _bestPlayerScore.text = $"Score : {PlayerDataHandler.Instance.bestPlayerScore}";
        _bestPlayerEnemiesKilled.text = $"Enemies Killed : {PlayerDataHandler.Instance.bestPlayerEnemiesKilled}";

        for (int i = 0; i < _gameObjects.Length; i++)
        {
            if(i < 5)
                _gameObjects[i].SetActive(false);
            else
                _gameObjects[i].SetActive(true);
        }
    }

    public void BackToMenu()
    {
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            if (i < 5)
                _gameObjects[i].SetActive(true);
            else
                _gameObjects[i].SetActive(false);
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
