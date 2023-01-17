using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerHandler : MonoBehaviour
{
    public delegate void Finished(ActionTriggerHandler trigger);
    public event Finished OnFinished;

    public virtual void OnAction()
    {

    }

    public virtual void Deactivate()
    {
        OnFinished.Invoke(this);
    }
}
