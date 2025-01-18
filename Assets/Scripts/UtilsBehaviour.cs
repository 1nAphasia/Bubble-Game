using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UtilsBehaviour : MonoBehaviour
{
    public bool firstCol=true;
    public float capturingTime=0.3f;
    public float floatingTime=0.3f;
    public float inflationTime=0.5f;
    public GameObject targetObject;
    public Vector3 bubbleInflatingScale=new Vector3(3f,3f,3f);
    public Vector2 bubbleCaptureEnemyVelo=new Vector2(0f,1f);

    private GameObject _player;
    private GameObject _bubble;
    private GameObject _enemy;
    
    void Start(){
        _player=GameObject.Find("Player");
        _bubble=GameObject.Find("Bubble");
        _enemy=GameObject.Find("Enemy");
        StartCoroutine(BubbleStartInflation(new Vector3(1f,1f,1f),inflationTime));//开始的BubbleScale应该是0,0,0。
    }
    void Update(){
        
    }

    

    //这个函数应当放在泡泡行为脚本里,检测泡泡与其他物体的相交状态,并做出反应
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name=="Enemy"&&firstCol){
            firstCol=false;
            targetObject=other.gameObject;
            //这一行设置Enemy状态机
            CapturingEnemy();
        }
        if(other.gameObject.name=="Player"&&firstCol){
            firstCol=false;
            targetObject=other.gameObject;
            //这一行设置Player状态机 也就是中断玩家的输入的Captured状态
            CapturingPlayer();
        }
    }

    private void CapturingEnemy(){
        
    }
    //这里应该完成两件事：在0.3秒内player泡泡设为同速,将player在0.3秒内移到中心,更改泡泡尺寸,最后修改Player的状态为InBubble;
    //我猜这个协程的作用就是单独在线程外运行一个内有单独计时的函数,然后在计时结束的第一个tick后更新游戏状态
    private void CapturingPlayer(){
        StartCoroutine(SettingPlayers(capturingTime));
        StartCoroutine(MorphingBubble(bubbleInflatingScale,capturingTime));
        StartCoroutine(SettingPlayersStatusMachine(floatingTime));
    }

    //在捕获敌人时,在规定时间内将气泡速度转为预设值
    private IEnumerator bubbleSlowDown(float time){
        Rigidbody2D bubbleRB=_bubble.GetComponent<Rigidbody2D>();
        float elapsedTime=0f;
        while(elapsedTime<time){
            //通过插值尝试将Bubble的Velo平滑过渡到与预设值相同
                bubbleRB.velocity=Vector3.Lerp(bubbleRB.velocity,bubbleRB.velocity,elapsedTime/time);
                elapsedTime+=Time.deltaTime;
                yield return null;
            }
        bubbleRB.velocity=bubbleRB.velocity;
    } 

    private IEnumerator BubbleStartInflation(Vector3 targetScale,float time){
        Vector3 InitialScale=transform.localScale;
        float elapsedTime=0f;
        while(elapsedTime<time){
            //同样的根据时间来将Scale动态插值到目标值。
            transform.localScale=Vector3.Lerp(InitialScale,targetScale,elapsedTime/time);
            elapsedTime+=Time.deltaTime;
            yield return null;
        }
        transform.localScale=targetScale;
    }
    private IEnumerator SettingPlayers(float time){
        Rigidbody2D playerRB=_player.GetComponent<Rigidbody2D>();
        Rigidbody2D bubbleRB=_bubble.GetComponent<Rigidbody2D>();
        //如果是已经得到了bubble的RigidBody2d对象可以替换成
        //Rigidbody2D bubbleRB=_rb;
        float elapsedTime=0f;
        while(elapsedTime<time){
            //通过插值尝试将Player的Pos和Velo平滑过渡到与泡泡相同
                _player.transform.position=Vector3.Lerp(_player.transform.position,transform.position,elapsedTime/time);
                playerRB.velocity=Vector3.Lerp(playerRB.velocity,bubbleRB.velocity,elapsedTime/time);
                elapsedTime+=Time.deltaTime;
                yield return null;
            }
        _player.transform.position=transform.position;
        playerRB.velocity=bubbleRB.velocity;
    }
    private IEnumerator MorphingBubble(Vector3 targetScale,float time){
            Vector3 InitialScale=transform.localScale;
            float elapsedTime=0f;
            while(elapsedTime<time){
                //同样的根据时间来将Scale和transform.position动态插值到目标值。
                Vector3 EnemyPos=targetObject.transform.position;
                transform.position=Vector3.Lerp(transform.position,EnemyPos,elapsedTime/time);
                transform.localScale=Vector3.Lerp(InitialScale,targetScale,elapsedTime/time);
                elapsedTime+=Time.deltaTime;
                yield return null;
            }
            transform.localScale=targetScale;
        }
    private IEnumerator SettingPlayersStatusMachine(float time){
        yield return new WaitForSeconds(time);
        //首先获取player状态机状态,如果还在inBubble就要改成air,如果不在inbubble就啥也不做
        //思路是在泡泡结束之后如果角色没有操作就直接修改角色的状态使其下坠。 
    }

    
}
