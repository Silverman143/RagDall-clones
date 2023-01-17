using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TriggerType
{
    Tree
}

public class TriggerTypeHandler : MonoBehaviour
{
    [SerializeField] private TriggerType _type;
    public TriggerType Type
    {
        get { return _type; }
        private set { _type = value; }
    }

}
