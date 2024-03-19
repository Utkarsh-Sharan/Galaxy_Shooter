using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    //collection of all observers of this subject
    private List<IObservable> _observers = new List<IObservable>();

    //add observer to the subject's collection
    public void AddObserver(IObservable observer)
    {
        _observers.Add(observer);
    }

    //remove observer from the subject's collection
    public void RemoveObserver(IObservable observer)
    {
        _observers.Remove(observer);
    }

    //notify all observers when an event has occured
    protected void NotifyObservers(PowerupPanelActions action)
    {
        _observers.ForEach((observer) =>
        {
            observer.OnNotify(action);
        });
    }
}
