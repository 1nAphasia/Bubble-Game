using System.Collections;
using UnityEngine;

public class BlueBubble : Bubble
{
    Vector3 CapturedScale = new Vector3(3f, 3f, 3f);
    float floatingTime = 3f;
    public CaptureState captureState { get; set; }
    public override void Awake()
    {
        base.Awake();
        captureState = new CaptureState(this, stateMachine, "Floating");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Enemy")
        {
            targetObject = other.gameObject;
            stateMachine.ChangeState(captureState);
            CapturingEnemy();
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

    private IEnumerator MorphingBubble(Vector3 targetScale, float time)
    {
        Vector3 InitialScale = transform.localScale;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            Vector3 EnemyPos = targetObject.transform.position;
            transform.position = Vector3.Lerp(transform.position, EnemyPos, elapsedTime / time);
            transform.localScale = Vector3.Lerp(InitialScale, targetScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private IEnumerator SlowDownEnemy(float time)
    {
        float elapsedTime = 0f;
        float initialDrag = 2f;
        var rb = targetObject.GetComponent<Rigidbody2D>();
        while (elapsedTime < time)
        {
            rb.drag = Mathf.Lerp(0, initialDrag, elapsedTime / time);
            yield return null;
        }
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
}
