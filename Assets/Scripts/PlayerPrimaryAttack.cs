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
        Vector3 direction = (mouseWorldPosition - player.transform.position).normalized;
        Vector3 bulletSpawnPosition = player.transform.position + direction * 3f;
        int attackDir = 0;

        if(mouseWorldPosition.x >= player.transform.position.x)
        {
            attackDir = 1;
        }
        else
        {
            attackDir = -1;
        }

        GameObject newBubble = Player.Instantiate(player.bubble[player.bubble_choice],
            bulletSpawnPosition, 
            new quaternion(0, 0, 0, 0)) as GameObject;
        Rigidbody2D bubbleRB = newBubble.GetComponent<Rigidbody2D>();
        Vector3 bulletDirection = (mouseWorldPosition - bulletSpawnPosition).normalized;
        bubbleRB.velocity = player.bubbleSpeed * bulletDirection;
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
