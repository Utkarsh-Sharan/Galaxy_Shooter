using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
    private int _score;
    [SerializeField] private int _enemies;
    private UIManager _uiManager;  //handle to the component

    //Special Enemy variables
    private SpecialEnemy _specialEnemy;

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

        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);      //(new Vector3(1, 0, 0) * 5 * Time.deltaTime; //(new Vector3(-1, 0, 0) will move the object towards left
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);      //(new Vector3(1, 0, 0) * 5 * Time.deltaTime;

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
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
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

            Destroy(this.gameObject);
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
        _uiManager.UpdateScore(_score);
    }

    public void AddEnemies(int enemy)
    {
        _enemies += enemy;
        _uiManager.EnemiesKilled(_enemies);

        if (_enemies > 4 && _enemies % 5 == 0 && _playerLife != 0)
        {
            _uiManager.PowerupPanel();
        }
    }
}
