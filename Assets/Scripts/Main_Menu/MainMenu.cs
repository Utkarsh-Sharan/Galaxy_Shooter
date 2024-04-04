using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjects;

    [SerializeField] private Text _currentPlayerName;

    [SerializeField] private Text _bestPlayerName;
    [SerializeField] private Text _bestPlayerScore;
    [SerializeField] private Text _bestPlayerEnemiesKilled;

    public void LoadInstructions()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadHighScore()
    {
        PlayerDataHandler.instance.LoadBestPlayerData();
        _bestPlayerName.text = $"Player : {PlayerDataHandler.instance.bestPlayerName}";
        _bestPlayerScore.text = $"Score : {PlayerDataHandler.instance.bestPlayerScore}";
        _bestPlayerEnemiesKilled.text = $"Enemies Killed : {PlayerDataHandler.instance.bestPlayerEnemiesKilled}";

        for (int i = 0; i < _gameObjects.Length - 2; i++)
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

    public void NewGameNameInput()
    {
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            if (i < 5)
                _gameObjects[i].SetActive(false);
            else if(i == 5 || i < 9)
                continue;
            else
                _gameObjects[i].SetActive(true);
        }
    }

    public void StartGame()
    {
        PlayerDataHandler.instance.currentPlayerName = _currentPlayerName.text;
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
