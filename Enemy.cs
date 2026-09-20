using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;

    private Rigidbody2D _rigidBody;
    private Movement _movement;
    private ColorChanger _colorChanger;
    private Health _health;
    private KnockBack _knockback;
    private Flash _flash;
    //迪斯科管理器
    private DiscoBallManager _discoManager;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();
        _health = GetComponent<Health>();
        _knockback = GetComponent<KnockBack>();
        _flash = GetComponent<Flash>();
        //全局查找管理器
        _discoManager = FindFirstObjectByType<DiscoBallManager>();
    }

    private void Start()
    {
        StartCoroutine(ChangeDirection());
        StartCoroutine(RandomJump());
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            float curDirection = UnityEngine.Random.Range(0, 2) * 2 - 1;
            _movement.SetCurrentDirection(curDirection);
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    private IEnumerator RandomJump()
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Init(Color color)
    {
        _colorChanger.SetDefaultColor(color);
    }

    public void TakeDamage(int damageAmount, float knockbackThrust)
    {
        //迪斯科派对激活时，直接扣满全部血量秒杀
        if (_discoManager != null && _discoManager.IsDiscoPartyActive)
        {
            _health.TakeDamage(_health.StartingHealth);
        }
        else
        {
            //正常扣血
            _health.TakeDamage(damageAmount);
        }

        //击退效果保留
        _knockback.GetKnockBack(PlayerController.Instance.transform.position, knockbackThrust);
    }

    public void TakeHit()
    {
        _flash.StartFlash();
    }
}