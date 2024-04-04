using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialEnemy : MonoBehaviour
{
    private float _speed = 5.0f;

    [SerializeField] private GameObject _specialLaserPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private Transform _laserSpawnPos;
    [SerializeField] private AudioClip _laserSoundClip;

    private Player _player;
    private Transform _playerPos;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {     
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL.");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }
        else
        {
            _spawnManager.SpecialEnemySpawned();
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        StartCoroutine(FireSpecialLaser());
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > 0)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }

        else
        {
            Vector3 newPos = new Vector3(transform.position.x, 0, 0);
            transform.position = newPos;
        }

        Vector3 direction = _playerPos.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, direction);

        if(_player._playerLife == 0)
        {
            Destroy(this.gameObject);
        }
    }
    
    IEnumerator FireSpecialLaser()
    {   
        while (_player._playerLife != 0)
        {           
            yield return new WaitForSeconds(2);
            Instantiate(_specialLaserPrefab, _laserSpawnPos.position, Quaternion.identity);
            _audioSource.Play();
        }
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Special Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(20);
            }

            _player.AddEnemies(1);
            _spawnManager.SpecialEnemyDestroyed();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if(other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }

            _player.AddEnemies(1);
            _spawnManager.SpecialEnemyDestroyed();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
