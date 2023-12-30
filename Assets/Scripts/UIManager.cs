using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _enemiesKilledText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Sprite[] _livesSprite;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartGameText;
    [SerializeField] private GameObject _powerupPanel;

    private GameManager _gameManager;
    private Player _player;
    private bool _isPowerupPanelActive;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _enemiesKilledText.text = "Enemies Killed: " + 0;

        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL.");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
    }

    private void Update()
    {
        if (_isPowerupPanelActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _player.TripleShotActive();
                _isPowerupPanelActive = false;
                _powerupPanel.SetActive(false);

            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                _player.ShieldActive();
                _isPowerupPanelActive = false;
                _powerupPanel.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _player.SpeedBoostActive();
                _isPowerupPanelActive = false;
                _powerupPanel.SetActive(false);
            }
        }
    }

    public void EnemiesKilled(int enemy)
    {
        _enemiesKilledText.text = "Enemies Killed: " + enemy.ToString();
    }

    public void PowerupPanel()
    {
        _powerupPanel.SetActive(true);
        _isPowerupPanelActive = true;
        StartCoroutine(PowerupPanelDeactivateRountine());
    }

    IEnumerator PowerupPanelDeactivateRountine()
    {
        yield return new WaitForSeconds(5.0f);
        _isPowerupPanelActive = false;
        _powerupPanel.SetActive(false);
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprite[currentLives];   
        
        if(currentLives == 0)
        {
            GameOverSequence();
        }
    } 

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);       
        StartCoroutine(GameOverFlickerRoutine());
    }
    
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);

            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }        
    }

    public void ResumePlay()
    {
        _gameManager.ResumeGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
