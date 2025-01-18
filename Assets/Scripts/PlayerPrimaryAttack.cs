using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time>=lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        int attackDir = 0;
        Debug.Log(mouseWorldPosition.x);
        Debug.Log(player.transform.position.x);
        if(mouseWorldPosition.x >= player.transform.position.x)
        {
            attackDir = 1;
        }
        else
        {
            attackDir = -1;
        }

        GameObject newBubble = Player.Instantiate(player.bubble, 
            new Vector3(player.transform.position.x + 1.2f * attackDir, player.transform.position.y, 0), 
            new quaternion(0, 0, 0, 0)) as GameObject;
        Rigidbody2D bubbleRB = newBubble.GetComponent<Rigidbody2D>();
        bubbleRB.velocity =new Vector2(attackDir * player.bubbleSpeed, 0);
        Debug.Log("Shot a Bubble!");
        //player.anim.SetInteger("ComboCounter",comboCounter);
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        //if (triggerCalled) 
        //{
        //    stateMachine.ChangeState(player.idleState);
        //}
        stateMachine.ChangeState(player.idleState);
    }
}
