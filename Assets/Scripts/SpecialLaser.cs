using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialLaser : MonoBehaviour
{
    private float _speed = 3.5f;
    private float _rotationSpeed = 100.0f;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is NULL.");
        }        
    }

    // Update is called once per frame
    void Update()
    {   
        if(_player._playerLife != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
            //transform.up = _player.transform.position - transform.position;
            transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(this.gameObject);
            _player.Damage();
        }

        if(other.tag == "Special Enemy")
        {
            Destroy(this.gameObject);
        }
    }   
}
