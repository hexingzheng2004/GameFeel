using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustVFX;//移动灰尘粒子特效
    [SerializeField] private ParticleSystem _poofDustVFX;//跳跃灰尘粒子特效
    [SerializeField] private Transform _characterSpriteTransform;//玩家精灵文件
    [SerializeField] private Transform _cowboyHatSpriteTransform;//玩家牛仔帽精灵文件
    [SerializeField] private float _tiltAngle = 20f;//倾斜角度
    [SerializeField] private float _tiltSpeed = 5f;//倾斜速度
    [SerializeField] private float _tiltSpeedModifer = 2f;//牛仔帽倾斜速度调整值
    [SerializeField] private float _yLandVelocityCheck = -22f;//玩家落地速度
    [SerializeField]private CinemachineImpulseSource _impulseSource;

    private Vector2 _velocityBeforePhysicsUpdate;
    private Rigidbody2D _rigidbody;

    //Unity消息 | 0个引用
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    //Unity消息 | 0个引用
    private void FixedUpdate()
    {
        _velocityBeforePhysicsUpdate = _rigidbody.velocity;
    }

    //Unity消息 | 0个引用
    private void Update()
    {
        ApplyTilt();
        DectectMoveDust();
    }

    //Unity消息 | 0个引用
    private void OnEnable()
    {
        PlayerController.OnJump += PlayPoofDustVFX;
    }
    //Unity消息 | 0个引用
    private void OnDisable()
    {
        PlayerController.OnJump -= PlayPoofDustVFX;
    }

    //Unity消息 | 0个引用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //下落速度 y时负值
        if (_velocityBeforePhysicsUpdate.y < _yLandVelocityCheck)
        {
            PlayPoofDustVFX();
            _impulseSource.GenerateImpulse();
        }
    }

    // 1个引用
    private void ApplyTilt()
    {
        float targetAngle;
        if(PlayerController.Instance.MoveInput.x < 0f)
        {
            //往后移动,角色前倾
            targetAngle = _tiltAngle;
        }else if (PlayerController.Instance.MoveInput.x > 0f)
        {
            //往前移动,角色后倾
            targetAngle = _tiltAngle * -1.0f;
        }
        else
        {
            //没有移动,角色不倾斜
            targetAngle = 0f;
        }
        //设置玩家精灵文件旋转
        Quaternion currentCharaterRotation = _characterSpriteTransform.rotation;//当前角色旋转量
        //将倾斜角度转成欧拉角旋转（绕着某个轴旋转）
        Quaternion targetRotation = Quaternion.Euler(currentCharaterRotation.eulerAngles.x,
            currentCharaterRotation.eulerAngles.y, targetAngle);
        _characterSpriteTransform.rotation = Quaternion.Slerp(
            currentCharaterRotation, targetRotation, _tiltSpeed*Time.deltaTime);

        //设置玩家牛仔帽精灵文件旋转
        Quaternion currentCowboyHatRotation = _cowboyHatSpriteTransform.rotation;//当前牛仔帽旋转量
        //将倾斜角度转成欧拉角旋转（绕着某个轴旋转） 和玩家相反
        targetRotation = Quaternion.Euler(currentCowboyHatRotation.eulerAngles.x,
            currentCowboyHatRotation.eulerAngles.y, -1.0f * targetAngle);
        _cowboyHatSpriteTransform.rotation = Quaternion.Slerp(currentCowboyHatRotation, targetRotation,
            _tiltSpeed * _tiltSpeedModifer * Time.deltaTime);
    }

    // 1个引用
    private void DectectMoveDust()
    {
        //玩家在地面移动，播放移动灰尘粒子效果
        if (PlayerController.Instance.CheckGrounded())
        {
            if (!_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Play();
            }
        }
        else
        {
            if (_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Stop();
            }
        }
    }
    private void PlayPoofDustVFX()
    {
        _poofDustVFX.Play();
    }
}