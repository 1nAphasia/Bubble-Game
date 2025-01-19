using UnityEngine;

public class VanishState : BubbleState
{
    public VanishState(Bubble _bubble, BubbleStateMachine _stateMachine, string _animBoolName) : base(_bubble, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.7f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        bubble.rb.gravityScale = 0f;
        bubble.rb.velocity = new Vector2(bubble.rb.velocity.x, 2.0f);
        if (stateTimer <= 0)
        {
            bubble.Vanish();
        }
    }
}
