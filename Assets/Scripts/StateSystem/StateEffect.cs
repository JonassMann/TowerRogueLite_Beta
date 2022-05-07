using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateEffect : MonoBehaviour
{
    public abstract void OnStart(GameObject user, GameObject target, GameObject moveTarget);
    public abstract State OnUpdate(GameObject user, GameObject target, GameObject moveTarget);
    public abstract void OnEnd(GameObject user, GameObject target, GameObject moveTarget);
}