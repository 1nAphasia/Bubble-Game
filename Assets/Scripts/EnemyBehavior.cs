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
    public List<Vector2> PatrolPoints;
    public float moveSpeed=3f;
    private int currentPointIndex=0;
    private float step;
    public LayerMask groundLayer;
    public EnemyState currentState;
    private Rigidbody2D _rb;
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        PatrolPoints.Add(new Vector2(2f,-4.28f));
        PatrolPoints.Add(new Vector2(8f,-4.28f));
        currentState=EnemyState.Idle;
        _rb=GetComponent<Rigidbody2D>();
        _player=GameObject.Find("Player");
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
                _rb.gravityScale=1;
                if(Vector3.Distance(transform.position,_player.transform.position)<=10&&IsGrounded()){
                    currentState=EnemyState.Patrolling;
                }
                break;
            }
            case EnemyState.Patrolling:
            {
                Patrol();
                break;
            }
        }
    }
    void Patrol(){
        if(PatrolPoints.Count==0)
        return;
        Vector2 moveDir=(PatrolPoints[currentPointIndex]-new Vector2(transform.position.x,transform.position.y)).normalized;
        _rb.velocity=moveDir*moveSpeed;
        if(Vector2.Distance(PatrolPoints[currentPointIndex],new Vector2(transform.position.x,transform.position.y))<0.2f){
            currentPointIndex = (currentPointIndex + 1) % PatrolPoints.Count;
        }
    }

    bool IsGrounded(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.3f, groundLayer);
        return hit.collider != null;
    }
}
