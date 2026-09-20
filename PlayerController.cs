using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Vector2 MoveInput=> _frameInput.Move;
    public static Action OnJump;//跳跃事件

    [SerializeField] private Transform _feetBox;
    [SerializeField] private LayerMask _groundLayer;//地面层级
    [SerializeField] private Vector2 _feetBoxSize;//重叠盒大小
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;//额外重力
    [SerializeField] private float _gravityDelay = 0.3f;//额外重力生效延迟时间
    [SerializeField] private float _coyoteTime = 0.5f;//郊狼时间


    private float _timeInAir;//玩家在空中持续时间
    private float _coyoteTimer;//郊狼计时器 
    private bool _doubleJumpAvailable;//二段跳可用状态
    //private bool _isGrounded = false;
    //private Vector2 _movement;
    private FrameInput _frameInput;

    private Rigidbody2D _rigidBody;
    private PlayerInput _playerInput;
    private Movement _movement;

    public void Awake()
    {
        if (Instance == null) { Instance = this; }
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        GatherInput();
        GravityDelay();
        PlayerMovement();
        HandleJump();
        HandleSpriteFlip();
        CoyoteTimer();
    }
    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
    }
    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
    }

    private void FixedUpdate()
    {
        // Move();
        ExtraGravity();
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        _isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        _isGrounded = false;
    //    }
    //}

    // 0个引用
    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    // 1个引用
    private void GatherInput()
    {
        // float moveX = Input.GetAxis("Horizontal");
        // _movement = new Vector2(moveX * _moveSpeed, _rigidBody.velocity.y);
        //_movement = new Vector2(_frameInput.Move.x * _moveSpeed, _rigidBody.velocity.y);
        _frameInput = _playerInput.FrameInput;
    }

    // 1个引用
    // private void Move()
    // {
    //     _rigidBody.velocity = _movement;
    // }
    private void PlayerMovement()
    {
        _movement.SetCurrentDirection(_frameInput.Move.x);
    }

    // 1个引用
    private void HandleJump()
    {
        //如果没有按下空格，直接退出
        if (!_frameInput.Jump) return;
        //按下了空格，且位于地面，施加向上的力
        if (CheckGrounded())
        {
            OnJump?.Invoke();
        }
        else if (_coyoteTimer>0f)
        {
            OnJump?.Invoke();
        }
        else if (_doubleJumpAvailable)
        {
            _doubleJumpAvailable = false;
            OnJump?.Invoke();
        }
    }
    private void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            _coyoteTimer = _coyoteTime;
            _doubleJumpAvailable = true;
        }
        else
        {
            //开启计时
            _coyoteTimer -= Time.deltaTime;
        }
    }


    private void ApplyJumpForce()
    {
        _timeInAir = 0f;//重置空中持续时间s
        _rigidBody.velocity = Vector2.zero;//重置跳跃时的速度为0
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }
    //通过物理重叠盒方法判断玩家是否站在地面上
    public bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(_feetBox.position, _feetBoxSize, 0f, _groundLayer);
        return isGrounded;
    }
    /// <summary>
    /// 管理玩家在空中时间计时器
    /// </summary>
    private void GravityDelay()
    {
        if (!CheckGrounded())
        {
            //玩家不在地面上
            _timeInAir += Time.deltaTime;
        }
        else
        {
            //玩家在地面
            _timeInAir = 0f;
        }
    }
    /// <summary>
    /// 施加额外重力
    /// </summary>
    private void ExtraGravity()
    {
        if(_timeInAir > _gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0f,-1 * _extraGravity * Time.fixedDeltaTime));
        }
    }
    //Unity消息 | 0个引用
    public void OnDrawGizmos()
    {
        //辅助绘图工具
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetBox.position, _feetBoxSize);
    }
     private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    } 
}