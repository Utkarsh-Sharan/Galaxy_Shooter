using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _specialEnemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    private bool _stopSpawning = false;
    private bool _isSpecialEnemySpawned = false;
         
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnSpecialEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.71f, 9.6f), 5.11f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(3f);
            if(_isSpecialEnemySpawned == true)
            {
                yield return new WaitForSeconds(5);
            }
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3);
        while(_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));

            Vector3 posToSpawn = new Vector3(Random.Range(-9.71f, 9.6f), 5.11f, 0);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
        }
    }

    IEnumerator SpawnSpecialEnemyRoutine()
    {
        yield return new WaitForSeconds(6);

        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-7, 7), 5.11f, 0);
            GameObject newEnemy = Instantiate(_specialEnemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(30);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void SpecialEnemySpawned()
    {
        _isSpecialEnemySpawned = true;
    }

    public void SpecialEnemyDestroyed()
    {
        _isSpecialEnemySpawned = false;
    }
}
