using UnityEngine;
using System;
using UnityEngine.Pool;
using Cinemachine;
using System.Collections;

public class Gun : MonoBehaviour
{
    //public Transform BulletSpawnPoint => _bulletSpawnPoint;

    //武器发射事件
    public static Action OnShoot;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private GameObject _muzzleFlash;//枪口闪光对象
    [SerializeField] private Bullet _bulletPrefab;//序列化的 （可以在unity inspector中显示）
    [SerializeField] private float _gunFireCD = 0.5f;//子弹发射冷却时间
    [SerializeField] private float _muzzleFlashTime = 0.2f;//子弹发射冷却时间

    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;//鼠标位置
    private float _lastFireTime = 0;//上次发射时间

    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;//屏幕抖动脉冲源
    private Coroutine _muzzleFlashCoroutine;//枪口闪光携程
    //子弹对象池
    private ObjectPool<Bullet> _bulletPool;

    //Unity消息 | 0个引用
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    //Unity消息 | 0个引用
    private void Start()
    {
        CreateBulletPool();
    }
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    //Unity消息 | 0个引用
    private void OnEnable()
    {
        //注册武器发射事件的监听方法
        OnShoot += ShootProjectile;
        OnShoot += ResetFireTime;
        OnShoot += FireAnimation;
        OnShoot += ScreenShaked;
        OnShoot += MuzzleFlash;
    }

    //Unity消息 | 0个引用
    private void OnDisable()
    {
        //解除注册武器发射事件的监听方法
        OnShoot -= ShootProjectile;
        OnShoot -= ResetFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= ScreenShaked;
        OnShoot -= MuzzleFlash;
    }

    // 1个引用
    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(
            () => { return Instantiate(_bulletPrefab); },//创建子弹预设体
            bullet => { bullet.gameObject.SetActive(true); },//从对象池中取子弹对象 （激活）
            bullet => { bullet.gameObject.SetActive(false); },//将当前子弹对象放回到对象池 (隐藏，不激活)
            bullet => { Destroy(bullet.gameObject); },//销毁当前子弹对象
            false,//不进行回收机制检查
            20,//子弹对象池列表初始数量
            50//子弹对象池列表最大容量
        );
    }

    // 1个引用
    public void ReleaseBullet(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    // 1个引用
    private void Shoot()
    {
        //鼠标左键按下（持续状态）
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime)
        {
            //ShootProjectile();
            OnShoot?.Invoke();
        }
    }

    // 2个引用
    private void ShootProjectile()
    {
        //_lastFireTime = _gunFireCD + Time.time;
        //生成子弹预设体
        //Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        Bullet newBullet = _bulletPool.Get();
        //初始化子弹
        newBullet.Init(this, _mousePos, _bulletSpawnPoint.position);
    }

    // 2个引用
    private void ResetFireTime()
    {
        _lastFireTime = _gunFireCD + Time.time;
    }

    // 2个引用
    private void FireAnimation()
    {
        //播放开火动画
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    //新增屏幕震动方法
    private void ScreenShaked()
    {
        _impulseSource.GenerateImpulse();
    }

    // 1个引用
    private void RotateGun()
    {
        //获得鼠标位置 世界坐标空间
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //计算鼠标位置 - gun位置 向量
        //Vector2 direction = _mousePos - (Vector2)transform.position;
        //转回基于player的本地坐标
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);

        //计算旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //设置gun旋转
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    private void MuzzleFlash()
    {
        if(_muzzleFlashCoroutine!=null)
        {
            StopCoroutine(_muzzleFlashCoroutine);
        }
        //射击时调用该方法，1. 启用闪烁对象，2. 持续一定时间后禁用该对象
        _muzzleFlashCoroutine = StartCoroutine(MuzzleFlashCoroutine());
    }

    // 1个引用
    private IEnumerator MuzzleFlashCoroutine()
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
}