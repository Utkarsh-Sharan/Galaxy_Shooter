using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Subject
{
    private float _speed = 10f;

    //Game object variables
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngineVisualizer;
    [SerializeField] private GameObject _leftEngineVisualizer;

    //Audio variables.
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private AudioClip _playerExplosionClip;
    private AudioSource _audioSource;

    //Setting fire rate.
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;

    //Spawn Manager variables.
    public int _playerLife = 3;
    private SpawnManager _spawnManager;

    //Boolean variables.
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    //UI variables
    [SerializeField] private int _enemies;
    private int _score;
    private UIManager _uiManager;  //handle to the component

    //for high score
    private string _currentPlayerName;
    private string _bestPlayerName;
    private int _bestPlayerScore;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);  //transform current position = new position(0,0,0)
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>(); //finds and gets the spawn manager component
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();      
        if(_audioSource == null)
        {
            Debug.LogError("The audio source is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        _currentPlayerName = PlayerDataHandler.instance.currentPlayerName;
        _uiManager.DisplayCurrentPlayerName(_currentPlayerName);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);       
        transform.Translate(direction * _speed * Time.deltaTime);
        
        if (transform.position.x >= 13.046f)
        {
            transform.position = new Vector3(-13.185f, transform.position.y, 0);
        }

        else if (transform.position.x <= -13.185f)
        {
            transform.position = new Vector3(13.046f, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.148f, 4.9698f), 0);
    }

    void FireLaser()
    {       
        _canFire = Time.time + _fireRate; 
        
        if(_isTripleShotActive == true)
        {
            //fire triple shot
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

        }

        else
        {
            //fire single laser
            GameObject laser = ObjectPooler.instance.GetPooledObjects();
            if(laser != null )
            {
                laser.transform.position = transform.position + new Vector3(0, 1.05f, 0);
                laser.SetActive(true);
            }
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if(_isShieldActive == true)    //checking if shield is collected
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _playerLife--;

        if(_playerLife == 2)
        {
            _rightEngineVisualizer.SetActive(true);
        }

        else if(_playerLife == 1)
        {
            _leftEngineVisualizer.SetActive(true);
        }

        _uiManager.UpdateLives(_playerLife);      //calling UI Manager to update player's life in case of damage.
        
        if(_playerLife == 0)
        {
            _spawnManager.OnPlayerDeath();  //will stop spawning enemies after player's death

            _audioSource.clip = _playerExplosionClip;
            _audioSource.Play();

            CheckForBestPlayer();

            this.gameObject.SetActive(false);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speed += 3.5f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed -= 3.5f;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        PlayerDataHandler.instance.currentPlayerScore = _score;
        _uiManager.UpdateScore(_score);
    }

    public void AddEnemies(int enemy)
    {
        _enemies += enemy;
        PlayerDataHandler.instance.currentPlayerEnemiesKilled = _enemies;
        _uiManager.EnemiesKilled(_enemies);

        if (_enemies > 4 && _enemies % 5 == 0 && _playerLife != 0)
        {          
            NotifyObservers(PowerupPanelActions.PanelActive);
            NotifyObservers(PowerupPanelActions.PanelSound);
        }
    }

    private void CheckForBestPlayer()
    {
        int score = PlayerDataHandler.instance.currentPlayerScore;
        int enemiesKilled = PlayerDataHandler.instance.currentPlayerEnemiesKilled;
        if(score > PlayerDataHandler.instance.bestPlayerScore)
        {
            PlayerDataHandler.instance.bestPlayerScore = score;
            PlayerDataHandler.instance.bestPlayerName = _currentPlayerName;
            PlayerDataHandler.instance.bestPlayerEnemiesKilled = enemiesKilled;

            PlayerDataHandler.instance.SaveBestPlayerData(_currentPlayerName, score, enemiesKilled);
        }
    }
}
