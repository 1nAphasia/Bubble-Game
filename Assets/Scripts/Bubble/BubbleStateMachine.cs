using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleStateMachine
{

    public BubbleState currentState { get; private set; }
    // Start is called before the first frame update

    public void Initialize(BubbleState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(BubbleState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

}
