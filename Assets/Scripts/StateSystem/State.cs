using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public Target targetType;
    public GameObject targetIfOther;

    private GameObject user;
    private GameObject target;
    private GameObject moveTarget;

    private List<StateEffect> stateEffects;

    public State StateUpdate()
    {
        State returnState = null;

        user.GetComponent<Character>().moveInput = Vector2.zero;

        foreach (StateEffect effect in stateEffects)
        {
            returnState = effect.OnUpdate(user, target, moveTarget);
            if (returnState != null)
                break;
        }

        return returnState;
    }

    public void StateStart()
    {
        stateEffects = new List<StateEffect>(GetComponents<StateEffect>());

        user = transform.parent.parent.gameObject;
        target = targetType == Target.Player ? GameObject.FindGameObjectWithTag("Player") : targetIfOther;
        moveTarget = target;

        foreach (StateEffect effect in stateEffects)
            effect.OnStart(user, target, moveTarget);
    }

    public void StateEnd()
    {
        foreach (StateEffect effect in stateEffects)
            effect.OnEnd(user, target, moveTarget);
    }

    public void ResetTarget()
    {
        moveTarget = target;
    }

    public void SetTarget(GameObject newTarget)
    {
        moveTarget = newTarget;
    }

    public enum Target
    {
        None,
        Player,
        Other
    }
}
