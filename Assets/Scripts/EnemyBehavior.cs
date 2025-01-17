using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState{
        Idle,
        Patrolling,
        Jumping,
        InBubble,
        Attacking
    }
    public EnemyState currentState;

    public float moveSpeed=3f;
    public float attackRange=1.5f;
    
    private Animator _animator;
    private Transform _player;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        currentState=EnemyState.Idle;
        _rb=GetComponent<Rigidbody2D>();
        _player=GameObject.Find("Capsule").transform;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBehaviour();
        //UpdateAnimator();
    }

    void HandleBehaviour(){
        switch(currentState)
        {
            case EnemyState.InBubble:
            {
                _rb.gravityScale=0;
                break;
            }
            case EnemyState.Idle:
            {
                _rb.gravityScale=1;
                if (Vector2.Distance(transform.position, _player.position) < 5f)
                {
                    currentState = EnemyState.Patrolling;
                }
                break;
            }
            case EnemyState.Patrolling:
            {
                Patrol();
                if (Vector2.Distance(transform.position, _player.position) < attackRange)
                {
                    currentState = EnemyState.Attacking;
                }
                break;
            }
            case EnemyState.Attacking:
            {
                //Attack();
                break;
            }

        }
    }
    void Patrol(){
        
    }
}
