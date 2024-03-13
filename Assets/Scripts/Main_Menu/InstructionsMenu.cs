using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsMenu : MonoBehaviour
{   
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

    public void NextButtonClicked()
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            if(i < 4)
                gameObjects[i].SetActive(false);
            else
                gameObjects[i].SetActive(true);
        }
    }

    public void BackButtonClicked()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (i < 4)
                gameObjects[i].SetActive(true);
            else
                gameObjects[i].SetActive(false);
        }
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene(2);
    }
}
