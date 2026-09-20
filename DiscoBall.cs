using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour, IHitable
{
    private Flash _flash;
    //private DiscoBallManager _discoManager;

    //Unity消息 | 0个引用
    private void Start()
    {
        _flash = GetComponent<Flash>();
        //_discoManager = FindFirstObjectByType<DiscoBallManager>();
        //print(_discoManager.gameObject);
    }

    // 2个引用
    public void TakeHit()
    {
        //闪烁
        _flash.StartFlash();
        DiscoBallManager.OnDiscoBallHitEvent?.Invoke();
    }
}