using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField]
    private State currentState;

    private void Awake()
    {
        currentState.StateStart();
    }

    private void Update()
    {
        State nextState = currentState?.StateUpdate();

        if (nextState != null)
            NextState(nextState);
    }

    private void NextState(State nextState)
    {
        currentState.StateEnd();
        currentState = nextState;
        currentState.StateStart();
    }
}
