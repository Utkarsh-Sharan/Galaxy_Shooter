using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _palette1, _palette2, _introVideo, _nextButton;
    [SerializeField] private GameObject _palette3, _palette4, _splEnemyVideo, _playButton, _backButton;

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
        _backButton.SetActive(true);
    }

    public void BackButtonClicked()
    {
        _introVideo.SetActive(true);
        _palette1.SetActive(true);
        _palette2.SetActive(true);
        _nextButton.SetActive(true);
        _splEnemyVideo.SetActive(false);
        _palette3.SetActive(false);
        _palette4.SetActive(false);
        _playButton.SetActive(false);
        _backButton.SetActive(false);
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene(2);
    }
}
