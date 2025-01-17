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
        InBubble
    }
    public EnemyState currentState;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        currentState=EnemyState.Idle;
        _rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.InBubble:
            {
                _rb.gravityScale=0;
                break;
            }
            case EnemyState.Idle:
            {
                
                _rb.gravityScale=0;
                break;
            }
        }
    }
}
