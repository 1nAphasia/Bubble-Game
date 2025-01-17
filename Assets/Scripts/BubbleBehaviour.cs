using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        carrying_player
    }
    float bubbleExistTime=3.0f;

    private bubbleStatus _st;
    private Rigidbody2D _rb;
    private CircleCollider2D _col;
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
        if(_st==bubbleStatus.carrying_enemy){
            
        }
    }   
    private void OnCollisionEnter(Collision other) {
            if(other.gameObject.name=="enemy"){
                _st=bubbleStatus.carrying_enemy;
            }
        }
}
