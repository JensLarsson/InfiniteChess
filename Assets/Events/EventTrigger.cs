using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [Header("Event Data")]
    public string eventCall;
    public EventParameter eventParameter;

    [Header("Trigger Options")]
    [SerializeField] actions postTriggerAction = actions.Nothing;
    enum actions { Nothing = 0, DeleteObject, DeactivateTrigger };
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (eventCall != "")
            {
                EventManager.TriggerEvent(eventCall, eventParameter);
            }
            switch (postTriggerAction)
            {
                case actions.DeleteObject:
                    Destroy(this.gameObject);
                    break;

                case actions.DeactivateTrigger:
                    Destroy(this);
                    break;
            }
        }
    }
}
