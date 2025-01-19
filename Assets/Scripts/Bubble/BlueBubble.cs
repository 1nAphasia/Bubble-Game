using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BlueBubble : Bubble
{
    Vector3 CapturedScale = new Vector3(3f, 3f, 3f);
    float floatingTime = 3f;
    public float capturingTime = 0.3f;
    public Vector3 bubbleInflatingScale = new Vector3(3f, 3f, 3f);
    public CaptureState captureState { get; set; }
    public override void Awake()
    {
        base.Awake();
        captureState = new CaptureState(this, stateMachine, "Floating");
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Enemy") && stateMachine.currentState!=captureState)
        {
            targetObject = other.gameObject;
            EnemyBehavior enemy = targetObject.GetComponent<EnemyBehavior>();
            enemy.inbubble = this;
            stateMachine.ChangeState(captureState);
            CapturingEnemy();
        }

        else if (other.gameObject.name == "Player" && stateMachine.currentState != captureState)
        {
            targetObject = other.gameObject;
            //这一行设置Player状态机 也就是中断玩家的输入的Captured状态
            stateMachine.ChangeState(captureState);
            Player player = targetObject.GetComponent<Player>();
            if (player != null) {
                player.stateMachine.ChangeState(player.inBubbleState);
                player.inBubble = this;
            }
            CapturingPlayer();
        }
    }
    private void CapturingEnemy()
    {
        //Vector3 predictedEnemyPosition=targetEnemy.transform.position;
        //Vector3 currentPosition=transform.position;
        //Vector3 v=(predictedEnemyPosition-currentPosition)/0.3f;
        //_rb.velocity=v;
        StartCoroutine(SlowDownEnemy(0.3f));
        StartCoroutine(MorphingBubble(CapturedScale, 0.3f));
        StartCoroutine(StartingBubble(0.3f));
        StartCoroutine(SetEnemyState(floatingTime, EnemyBehavior.EnemyState.Idle));
    }

    private IEnumerator SlowDownEnemy(float time)
    {
        float elapsedTime = 0f;
        float initialDrag = 2f;
        var rb = targetObject.GetComponent<Rigidbody2D>();
        while (elapsedTime < time && targetObject)
        {
            rb.drag = Mathf.Lerp(0, initialDrag, elapsedTime / time);
            yield return null;
        }
        if(targetObject)
            rb.velocity = new Vector2(0f, 0f);

    }
    private IEnumerator SetEnemyState(float delay, EnemyBehavior.EnemyState EState)
    {
        yield return new WaitForSeconds(delay);
        var enemyScript = targetObject.GetComponent<EnemyBehavior>();
        enemyScript.currentState = EState;
    }

    private IEnumerator StartingBubble(float delay)
    {
        var enemyScript = targetObject.GetComponent<EnemyBehavior>();
        yield return new WaitForSeconds(delay);
        enemyScript.currentState = EnemyBehavior.EnemyState.InBubble;

    }

    private void CapturingPlayer()
    {
        StartCoroutine(SettingPlayers(capturingTime));
        StartCoroutine(MorphingBubble(bubbleInflatingScale, capturingTime));
        StartCoroutine(SettingPlayersStatusMachine(floatingTime));
    }

    private IEnumerator SettingPlayers(float time)
    {
        Rigidbody2D playerRB = targetObject.GetComponent<Rigidbody2D>();
        Rigidbody2D bubbleRB = GetComponent<Rigidbody2D>();
        //如果是已经得到了bubble的RigidBody2d对象可以替换成
        //Rigidbody2D bubbleRB=_rb;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            //通过插值尝试将Player的Pos和Velo平滑过渡到与泡泡相同
            targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, transform.position, elapsedTime / time);
            playerRB.velocity = Vector3.Lerp(playerRB.velocity, bubbleRB.velocity, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = transform.position;
        playerRB.velocity = bubbleRB.velocity;
    }
    private IEnumerator MorphingBubble(Vector3 targetScale, float time)
    {
        Vector3 InitialScale = transform.localScale;
        float elapsedTime = 0f;
        while (elapsedTime < time && targetObject)
        {
            //同样的根据时间来将Scale和transform.position动态插值到目标值。
            Vector3 EnemyPos = targetObject.transform.position;
            transform.position = Vector3.Lerp(transform.position, EnemyPos, elapsedTime / time);
            transform.localScale = Vector3.Lerp(InitialScale, targetScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
    private IEnumerator SettingPlayersStatusMachine(float time)
    {
        yield return new WaitForSeconds(time);
        //首先获取player状态机状态,如果还在inBubble就要改成air,如果不在inbubble就啥也不做
        //思路是在泡泡结束之后如果角色没有操作就直接修改角色的状态使其下坠。 
    }
}
