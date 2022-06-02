using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Used for animation clips events when the method is not in the same object.
/// </summary>
public class animationEventHandler : MonoBehaviour
{
    /// <summary>
    /// Action to invoke when animation triggers the event.
    /// </summary>
    public UnityEvent Event;
    public void InvokeAnimationEvent()
    {
        Event?.Invoke();
    }
}
