using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BossBehaviour : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Attacking
    }
    public float moveSpeed = 3f;
    public EnemyState currentState;
    public bool isAttackCoolDown = true;
    public GameObject Bullet;
    private GameObject _player;
    private int _enemyHP = 50;
    private SpriteRenderer _sr;
    private VisualElement _gameEndingMenu;
    private Button _endingButton;

    // Start is called before the first frame update
    void Start()
    {
        var root = GameObject.Find("PlayerHUD").GetComponent<UIDocument>().rootVisualElement;
        _gameEndingMenu = root.Q<VisualElement>("WinningMenu");
        _endingButton = root.Q<Button>("BackButton");
        currentState = EnemyState.Idle;
        _player = GameObject.Find("Player");
        _sr = GetComponent<SpriteRenderer>();
        _endingButton.clicked += BacktoStartMenu;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                {
                    if (Vector3.Distance(transform.position, _player.transform.position) <= 15)
                    {
                        currentState = EnemyState.Attacking;
                    }
                    break;
                }
            case EnemyState.Attacking:
                {
                    TrytoHitPlayer();
                    break;
                }
        }
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
            GameObject newBullet = Instantiate(Bullet, selfPos + playerDir * 4, new quaternion(0, 0, 0, 0));
            newBullet.GetComponent<Rigidbody2D>().velocity = playerDir * 6;
            isAttackCoolDown = false;
            StartCoroutine(AttackCoolDown(2f));
        }
    }
    void Die()
    {
        Destroy(gameObject);
        Time.timeScale = 0;
        _gameEndingMenu.style.display = DisplayStyle.Flex;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Blue_Bubble(Clone)")
        {
            ApplyDamage(3);
        }
        else if (collider.gameObject.name == "Red_Bubble(Clone")
        {
            ApplyDamage(6);
        }
    }

    void ApplyDamage(int damage)
    {
        _enemyHP -= damage;
        Debug.Log($"Boss took {damage} damage,and {_enemyHP} left!");
        if (_enemyHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }
    private IEnumerator AttackCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        isAttackCoolDown = true;
    }
    private IEnumerator FlashRed()
    {
        _sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.white;
    }
    void BacktoStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
