using Unity.Mathematics;
using UnityEngine;


public class CapsuleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    enum Status
    {
        walking,
        floating,
        inBubble
    }

    public enum bubbleColor
    {
        red,
        orange,
        yellow,
        green,
        cyan,
        blue,
        purple
    }
    //public float movingSpeed=4f;
    public float maxSpeed = 5f;
    public float acceleration = 5f;
    public float currentSpeed = 0f;
    public float JumpVelocity = 5f;
    public LayerMask groundLayer;
    public GameObject bubble;
    public bubbleColor currentColor = bubbleColor.red;
    public float bubbleSpeed = 10f;
    public Vector3 viewDir;
    private CapsuleCollider2D _col;
    private Rigidbody2D _rb;
    private bool _isJumping;
    private bool _isShooting;
    public bool isGrounded;
    private float _vInput;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        viewDir = (mousePos - transform.position).normalized;
        //_isShooting|=Input.GetKeyDown(KeyCode.J);
        if (Input.GetMouseButtonDown(0))
        {
            _isShooting = true;
        }

        _isJumping |= Input.GetKeyDown(KeyCode.Space);
        //Simple movementcontrol using acceleration(friction force not dealed yet)
        _vInput = Input.GetAxisRaw("Horizontal");
        if (_vInput != 0)
        {
            currentSpeed += _vInput * acceleration * Time.deltaTime;

        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, acceleration * Time.deltaTime);
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        _rb.velocity = new Vector2(currentSpeed, _rb.velocity.y);
    }

    void FixedUpdate()
    {
        isGrounded = IsGrounded();
        if (_isJumping && isGrounded)
        {
            Debug.Log("Jump!");
            _rb.AddForce(Vector2.up * JumpVelocity, ForceMode2D.Impulse);
        }
        _isJumping = false;
        if (_isShooting)
        {
            GameObject newBubble = Instantiate(bubble, transform.position + viewDir, new quaternion(0, 0, 0, 0)) as GameObject;
            Rigidbody2D bubbleRB = newBubble.GetComponent<Rigidbody2D>();
            bubbleRB.velocity = viewDir * bubbleSpeed;
            Debug.Log("Shot a Bubble!");
        }
        _isShooting = false;
    }
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.3f, groundLayer);
        return hit.collider != null;
    }
}
