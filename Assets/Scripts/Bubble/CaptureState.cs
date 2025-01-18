using UnityEngine;

public class CaptureState : BubbleState
{
    public CaptureState(Bubble _bubble, BubbleStateMachine _stateMachine, string _animBoolName) : base(_bubble, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 3f;
        rb.gravityScale = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, 1);
        rb.gravityScale = 0f;
        bubble.targetObject.GetComponent<Rigidbody2D>().velocity = rb.velocity;
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(bubble.vanishState);
        }
    }
}
