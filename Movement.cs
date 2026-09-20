using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 6f;//移动速度

    private bool _canMove = true;
    private float _moveX;//水平方向移动控制值
    private Rigidbody2D _rigidbody2D;
    private KnockBack _knockback;

    //Unity消息 | 0个引用
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<KnockBack>();
    }

    //Unity消息 | 0个引用
    private void FixedUpdate()
    {
        Move();
    }

    //Unity消息 | 0个引用
    private void OnEnable()
    {
        //当前对象被击退时，停止玩家移动
        _knockback.OnKnockbackStart += SetCanMoveFalse;
        //当前对象被击退事件结束时，启用玩家移动
        _knockback.OnKnockbackEnd += SetCanMoveTrue;
    }

    //Unity消息 | 0个引用
    private void OnDisable()
    {
        _knockback.OnKnockbackStart -= SetCanMoveFalse;
        _knockback.OnKnockbackEnd -= SetCanMoveTrue;
    }

    // 2个引用
    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }

    // 1个引用
    private void Move()
    {
        //当前对象被击退，_canMove == false
        if (!_canMove) return;

        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = movement;
    }

    // 2个引用
    private void SetCanMoveTrue()
    {
        _canMove = true;
    }

    // 2个引用
    private void SetCanMoveFalse()
    {
        _canMove = false;
    }
}