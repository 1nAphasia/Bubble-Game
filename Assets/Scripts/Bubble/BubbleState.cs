using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleState
{
    protected BubbleStateMachine stateMachine;
    protected Bubble bubble;

    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public BubbleState(Bubble _bubble, BubbleStateMachine _stateMachine, string _animBoolName)
    {
        this.bubble = _bubble;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        bubble.anim.SetBool(animBoolName, true);
        rb = bubble.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

    }

    public virtual void Exit()
    {
        if(bubble.anim)
            bubble.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}