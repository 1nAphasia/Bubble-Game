using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    enum bubbleColor{
        red=1,
        orange,
        yellow,
        green,
        cyan,
        blue,
        purple
    }
    enum bubbleStatus{
        floating,
        carrying_enemy,
        carrying_player,
        capturing_enemy
    }

    float bubbleExistTime=10f;
    float floatingTime=3f;
    float capturingTime=0.3f;
    Vector3 CapturedScale=new Vector3(3f,3f,3f);

    private bubbleStatus _st;
    private Rigidbody2D _rb;
    private CircleCollider2D _col;
    private GameObject targetObject;
    // Start is called before the first frame update

    void Start()
    {
        Destroy(gameObject,bubbleExistTime);
        _rb=GetComponent<Rigidbody2D>();
        _col=GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(_st){
        case bubbleStatus.carrying_enemy:
        {
            _rb.velocity=new Vector2(0,1);
            _rb.gravityScale=0f;
            targetObject.GetComponent<Rigidbody2D>().velocity=_rb.velocity;

            Destroy(gameObject,floatingTime);
            break;
        }
        case bubbleStatus.capturing_enemy:
        {
            _rb.gravityScale=0f;
            break;
        }
        }

    }   
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name=="Enemy"){
            targetObject=other.gameObject;
            _st=bubbleStatus.capturing_enemy;
            CapturingEnemy();
        }
        if(other.gameObject.name=="Capsule"){
            targetObject=other.gameObject;
            _st=bubbleStatus.carrying_player;
            CapturingPlayer();
        }
            
        }
        private void CapturingEnemy(){
            //Vector3 predictedEnemyPosition=targetEnemy.transform.position;
            //Vector3 currentPosition=transform.position;
            //Vector3 v=(predictedEnemyPosition-currentPosition)/0.3f;
            //_rb.velocity=v;
            StartCoroutine(MorphingBubble(CapturedScale,0.3f));
            StartCoroutine(StartingBubble(0.3f));
            StartCoroutine(SetEnemyState(floatingTime,EnemyBehavior.EnemyState.Idle));
        }

        private void CapturingPlayer(){

        }
        private IEnumerator StartingBubble(float delay){
            yield return new WaitForSeconds(delay);
            _st=bubbleStatus.carrying_enemy;
            var enemyScript=targetObject.GetComponent<EnemyBehavior>();
            enemyScript.currentState=EnemyBehavior.EnemyState.InBubble;
        }

        private IEnumerator MorphingBubble(Vector3 targetScale,float time){
            Vector3 InitialScale=transform.localScale;
            Vector3 InitialPos=transform.position;
            float elapsedTime=0f;
            while(elapsedTime<time){
                Vector3 EnemyPos=targetObject.transform.position;
                transform.position=Vector3.Lerp(InitialPos,EnemyPos,elapsedTime/time);
                transform.localScale=Vector3.Lerp(InitialScale,targetScale,elapsedTime/time);
                elapsedTime+=Time.deltaTime;
                yield return null;
            }
            transform.localScale=targetScale;
        }

        private IEnumerator SetEnemyState(float delay,EnemyBehavior.EnemyState EState){
            yield return new WaitForSeconds(delay);
            targetObject.GetComponent<EnemyBehavior>().currentState=EState;
        }

}