﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInBubbleState : PlayerState
{
    public PlayerInBubbleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter InBubble");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.inBubble == null)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.inBubble.Vanish();
            stateMachine.ChangeState(player.jumpState);
        }

        if (xInput != 0)
        {
            player.inBubble.Vanish();
            stateMachine.ChangeState(player.moveState);
        }

    }
}
