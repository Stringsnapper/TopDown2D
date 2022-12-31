using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnObjectTriggerEvent();
    public static event OnObjectTriggerEvent onObjectTriggerEvent;
    public static void RaiseObjectTriggerEvent()
    {
        if(onObjectTriggerEvent != null)
        {
            onObjectTriggerEvent();
        }
    }
}
