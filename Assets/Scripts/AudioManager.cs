using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IObservable
{
    [SerializeField] private Subject _playerSubject;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        _playerSubject.AddObserver(this);
    }

    private void Start()
    {
        _audioSource = transform.GetChild(1).GetComponent<AudioSource>();
    }

    public void OnNotify(PowerupPanelActions action)
    {
        if(action == PowerupPanelActions.PanelSound)
        { 
            _audioSource.Play();
        }
    }

    private void OnDisable()
    {
        _playerSubject.RemoveObserver(this);
    }
}
