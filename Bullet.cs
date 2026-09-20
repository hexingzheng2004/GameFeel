using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _deathVFX;//子弹销毁特效
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackThrust = 20f;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;
    private Gun _gun;

    //Unity消息 | 0个引用
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    //Unity消息 | 0个引用
    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    //Unity消息 | 0个引用
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(_damageAmount, _knockbackThrust);

        IHitable hitable = other.gameObject.GetComponent<IHitable>();
        hitable?.TakeHit();

        //Health health = other.gameObject.GetComponent<Health>();
        //health?.TakeDamage(_damageAmount);
        //击退
        //KnockBack knockback = other.gameObject.GetComponent<KnockBack>();
        //knockback?.GetKnockBack(PlayerController.Instance.transform.position, _knockbackThrust);
        //闪烁变色
        //Flash flash = other.gameObject.GetComponent<Flash>();
        //flash?.StartFlash();
        //Destroy(this.gameObject);
        //生成子弹销毁特效
        Instantiate(_deathVFX, transform.position, transform.rotation);
        _gun.ReleaseBullet(this);
    }

    // 1个引用
    public void Init(Gun gun, Vector2 mousePos, Vector2 bulletSpawnPos)
    {
        _gun = gun;
        //设置子弹生成位置
        transform.position = bulletSpawnPos;
        //初始化子弹朝向 单位化
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }
}