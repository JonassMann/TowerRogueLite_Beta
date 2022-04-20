using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public Target targetType;
    private GameObject targetIfOther;

    private GameObject user;
    private GameObject target;

    private List<StateEffect> stateEffects;

    public State StateUpdate()
    {
        State returnState = null;

        foreach (StateEffect effect in stateEffects)
        {
            returnState = effect.OnUpdate(user, target);
            if (returnState != null)
                break;
        }

        return returnState;
    }

    public void StateStart()
    {
        stateEffects = new List<StateEffect>(GetComponents<StateEffect>());

        user = transform.parent.parent.gameObject;
        if (targetType == Target.Player) target = GameObject.FindGameObjectWithTag("Player");
        if (targetType == Target.Other) target = targetIfOther;

        foreach (StateEffect effect in stateEffects)
            effect.OnStart(user, target);
    }

    public void StateEnd()
    {
        foreach (StateEffect effect in stateEffects)
            effect.OnEnd(user, target);
    }

    public enum Target
    {
        None,
        Player,
        Other
    }
}
