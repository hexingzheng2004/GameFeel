using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public static Action<Health> OnDeath;

    // 1个引用
    public GameObject SplatterPrefab { get { return _splatterPrefab; } }
    // 1个引用
    public GameObject DeathVFX => _deathVFX;
    //获取初始最大血量
    public int StartingHealth => _startingHealth;

    [SerializeField] private GameObject _splatterPrefab;//死亡飞溅预设体
    [SerializeField] private GameObject _deathVFX;//死亡特效

    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    //Unity消息 | 0个引用
    private void Start() {
        ResetHealth();
    }

    // 1个引用
    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    // 1个引用
    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}