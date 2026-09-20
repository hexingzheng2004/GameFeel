using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    //当子弹击中disco ball 1.调低全局光照 2.加快spotlights的旋转速度 3.持续一定时候后恢复
    public static Action OnDiscoBallHitEvent;

    [SerializeField] private Light2D _light2D; //全局光照
    [SerializeField] private float _discoLightIntensity = 0.2f;//disco光照强度
    [SerializeField] private float _discoballPartyTime = 3.0f;//disco party 时间

    private ColorSpotlight[] _colorSpotlights;//聚光灯数组
    private float _defaultGlobalLightIntensity;//默认光照强度
    private Coroutine _discoballPartyCoroutine; //舞池协程
    //派对激活标记，给Enemy读取
    public bool IsDiscoPartyActive { get; private set; }

    //Unity消息 | 0个引用
    private void Awake()
    {
        _colorSpotlights = FindObjectsByType<ColorSpotlight>(FindObjectsSortMode.None);
    }

    //Unity消息 | 0个引用
    private void Start()
    {
        if (_light2D != null)
            _defaultGlobalLightIntensity = _light2D.intensity;
        IsDiscoPartyActive = false;
    }

    //Unity消息 | 0个引用
    private void OnEnable()
    {
        OnDiscoBallHitEvent += DiscoBallParty;
    }

    //Unity消息 | 0个引用
    private void OnDisable()
    {
        OnDiscoBallHitEvent -= DiscoBallParty;
    }

    // 2个引用
    public void DiscoBallParty()
    {
        if (_discoballPartyCoroutine != null) return;
        _discoballPartyCoroutine = StartCoroutine(GlobalLightResetCoroutine());
        IsDiscoPartyActive = true;

        foreach (ColorSpotlight spotlight in _colorSpotlights)
        {
            StartCoroutine(spotlight.SpotlightDiscoPartyCoroutine(_discoballPartyTime));
        }
    }

    // 1个引用
    private IEnumerator GlobalLightResetCoroutine()
    {
        if (_light2D == null) yield break;
        
        _light2D.intensity = _discoLightIntensity;
        yield return new WaitForSeconds(_discoballPartyTime);
        _light2D.intensity = _defaultGlobalLightIntensity;
        _discoballPartyCoroutine = null;
        //派对结束关闭秒杀标记
        IsDiscoPartyActive = false;
    }
}