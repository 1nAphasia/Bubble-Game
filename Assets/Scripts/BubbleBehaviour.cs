using System.Collections;
using System.Collections.Generic;
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
    float bubbleExistTime=3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,bubbleExistTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
