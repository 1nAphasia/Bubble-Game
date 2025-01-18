using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    protected enum BubbleColor
    {
        red = 1,
        orange,
        yellow,
        green,
        cyan,
        blue,
        purple
    }

    [Header("Bubble Info")]
    [SerializeField] private float bubbleExistTime = 5f;
    [SerializeField] private float floatingTime = 3f;
    [SerializeField] private float capturingTime = 0.3f;

    public Rigidbody2D rb { get; private set; }
    public GameObject targetObject;
    protected BubbleColor bubbleColor;
    public Animator anim { get; private set; }

    public BubbleStateMachine stateMachine { get; private set; }

    public BubbleGenerateState generateState { get; private set; }
    public FloatingState floatingState { get; private set; }

    public VanishState vanishState { get; private set; }

    public virtual void Awake()
    {
        stateMachine = new BubbleStateMachine();

        generateState = new BubbleGenerateState(this, stateMachine, "Generate");
        floatingState = new FloatingState(this, stateMachine, "Floating");
        vanishState = new VanishState(this, stateMachine, "Vanish");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        stateMachine.Initialize(generateState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void Vanish()
    {
        Destroy(gameObject);
    }
}
