using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Game Event", order = 52)]
public class Event : ScriptableObject
{
    private List<EventListener> eventListeners = new List<EventListener>();
    public void Register(EventListener eventListener)
    {
        eventListeners.Add(eventListener);
    }
    public void Unregister(EventListener eventListener)
    {
        eventListeners.Remove(eventListener);
    }

    public void Occurred(GameObject gameObject)
    {
        for (int i = 0; i < eventListeners.Count; i++)
        {
            eventListeners[i].OnEventOccurs(gameObject);
        }
    }
}
