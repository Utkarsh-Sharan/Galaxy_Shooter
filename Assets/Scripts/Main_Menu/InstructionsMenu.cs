using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _palette1, _palette2, _introVideo, _nextButton;
    [SerializeField] private GameObject _palette3, _palette4, _splEnemyVideo, _playButton;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextButtonClicked()
    {
        _introVideo.SetActive(false);
        _palette1.SetActive(false);
        _palette2.SetActive(false);
        _nextButton.SetActive(false);
        _splEnemyVideo.SetActive(true);
        _palette3.SetActive(true);
        _palette4.SetActive(true);
        _playButton.SetActive(true);
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene(2);
    }
}
