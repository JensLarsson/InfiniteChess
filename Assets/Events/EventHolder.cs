using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventHolder : MonoBehaviour
{
    public string subscriptionName;
    public UnityEvent actions;

    private void OnEnable()
    {
        EventManager.Subscribe(subscriptionName, TriggerEvent);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribe(subscriptionName, TriggerEvent);
    }
    void TriggerEvent(EventParameter eventParam)
    {
        actions.Invoke();
    }
}
