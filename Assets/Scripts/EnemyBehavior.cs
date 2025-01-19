using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrolling,
        InBubble,
        Attacking
    }
    public List<Vector2> PatrolPoints;
    public float moveSpeed = 3f;
    public LayerMask groundLayer;
    public EnemyState currentState;
    public bool isAttackCoolDown = true;
    public bool isFalling = false;
    public GameObject Bullet;
    public float enemyHeight;
    public float oneHPTakenPerHeight = 2f;
    private int currentPointIndex = 0;
    private float step;
    private float startFallHeight;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;
    private GameObject _player;
    private int _enemyHP = 5;
    private Coroutine damageCoroutine;
    private SpriteRenderer _sr;

    public Bubble inbubble;

    // Start is called before the first frame update
    void Start()
    {
        PatrolPoints.Add(new Vector2(2f, -4.28f));
        PatrolPoints.Add(new Vector2(8f, -4.28f));
        currentState = EnemyState.Idle;
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");
        _col = GetComponent<CapsuleCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        enemyHeight = _col.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.InBubble:
                {
                    _rb.gravityScale = 0;
                    if (damageCoroutine == null)
                    {
                        damageCoroutine = StartCoroutine(ApplyDamageOverTime());
                    }
                    break;
                }
            case EnemyState.Idle:
                {
                    StopTakingDamage();
                    _rb.gravityScale = 1;
                    if (Vector3.Distance(transform.position, _player.transform.position) <= 15 && IsGrounded())
                    {
                        currentState = EnemyState.Patrolling;
                    }
                    if (!isFalling && !IsGrounded() && _rb.velocity.y < 0)
                    {
                        isFalling = true;
                        startFallHeight = transform.position.y;
                    }

                    break;
                }
            case EnemyState.Patrolling:
                {
                    StopTakingDamage();
                    Patrol();
                    break;
                }
            case EnemyState.Attacking:
                {
                    break;
                }
        }
    }
    void Patrol()
    {
        if (PatrolPoints.Count == 0)
            return;
        Vector2 moveDir = (PatrolPoints[currentPointIndex] - new Vector2(transform.position.x, transform.position.y)).normalized;
        _rb.velocity = moveDir * moveSpeed;
        if (Vector2.Distance(PatrolPoints[currentPointIndex], new Vector2(transform.position.x, transform.position.y)) < 0.2f)
        {
            currentPointIndex = (currentPointIndex + 1) % PatrolPoints.Count;
        }
        TrytoHitPlayer();
    }

    void TrytoHitPlayer()
    {
        Vector2 playerPos = _player.transform.position;
        Vector2 selfPos = transform.position;
        float distance = Vector2.Distance(playerPos, selfPos);
        Vector2 playerDir = (playerPos - selfPos).normalized;
        RaycastHit2D hit = Physics2D.Raycast(selfPos, playerDir);
        Debug.DrawRay(selfPos, playerDir * 10f, Color.red, 0.1f);
        if (isAttackCoolDown && hit.collider != null)
        {
            Debug.LogFormat("Attack!Target = {0}", hit.collider);
            GameObject newBullet = Instantiate(Bullet, selfPos + playerDir * 2, new quaternion(0, 0, 0, 0));
            newBullet.GetComponent<Rigidbody2D>().velocity = playerDir * 6;
            isAttackCoolDown = false;
            StartCoroutine(AttackCoolDown(2f));
        }
    }


    void Die()
    {

        inbubble.stateMachine.ChangeState(inbubble.vanishState);

        Destroy(gameObject);
    }

    void StopTakingDamage()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFalling && IsGroundLayer(collision))
        {
            isFalling = false;

            float fallHeight = startFallHeight - transform.position.y;
            if (fallHeight >= oneHPTakenPerHeight)
            {
                int damage = (int)(fallHeight / oneHPTakenPerHeight);
                ApplyDamage(damage);
                Debug.Log($"Enemy took {damage} fall damage from a height of {fallHeight}.");
            }
        }
    }
    void ApplyDamage(int damage)
    {
        _enemyHP -= damage;
        Debug.Log($"Enemy took {damage} damage,and {_enemyHP} left!");
        if (_enemyHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, enemyHeight / 2 + 0.01f, groundLayer);
        return hit.collider != null;
    }

    bool IsGroundLayer(Collision2D collision)
    {
        return (groundLayer.value & (1 << collision.gameObject.layer)) > 0;//LayerMusk是一个按照1的位来标记layer的二进制数,用与运算可以快速得到是否包含层。
    }

    private IEnumerator AttackCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        isAttackCoolDown = true;
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (true)
        {
            _enemyHP -= 1;
            if (_enemyHP <= 0)
            {
                Debug.Log("Enemy Is Dead.");
                Die();
                yield break;
            }
            else
            {
                StartCoroutine(FlashRed());
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator FlashRed()
    {
        _sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.white;
    }
}
