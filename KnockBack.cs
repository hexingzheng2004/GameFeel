using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KnockBack : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;

    [SerializeField] private float _knockbackTime = .2f;//击退时间

    private Vector3 _hitDirection;//被击中方向
    private float _knockbackThrust;//击退推力
    private Rigidbody2D _rigidbody;

    //Unity消息 | 0个引用
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    //Unity消息 | 0个引用
    private void OnEnable()
    {
        OnKnockbackStart += ApplyKnockbackThrust;
        OnKnockbackEnd += StopKnockBack;
    }

    //Unity消息 | 0个引用
    private void OnDisable()
    {
        OnKnockbackStart -= ApplyKnockbackThrust;
        OnKnockbackEnd -= StopKnockBack;
    }

    // 1个引用
    public void GetKnockBack(Vector3 hitDirection, float knockbackThrust)
    {
        _hitDirection = hitDirection;
        _knockbackThrust = knockbackThrust;
        OnKnockbackStart?.Invoke();
        Debug.Log("knock back!");
    }

    // 2个引用
    private void ApplyKnockbackThrust()
    {
        Vector3 direction = (transform.position - _hitDirection).normalized;
        _rigidbody.AddForce(direction * _knockbackThrust * _rigidbody.mass, ForceMode2D.Impulse);
        StartCoroutine(KnockBackCoroutine());
    }

    // 1个引用
    private IEnumerator KnockBackCoroutine()
    {
        yield return new WaitForSeconds(_knockbackTime);
        OnKnockbackEnd?.Invoke();
    }

    // 2个引用
    private void StopKnockBack()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}