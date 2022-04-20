using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateEffect : MonoBehaviour
{
    public StatBlock statBlock;
    public abstract void OnStart(GameObject user, GameObject target);
    public abstract State OnUpdate(GameObject user, GameObject target);
    public abstract void OnEnd(GameObject user, GameObject target);
}