using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{
    //Unity消息 | 0个引用
    private void OnEnable()
    {
        Health.OnDeath += SpawnDeathSplatter;
        Health.OnDeath += SpawnDeathVFX;
    }
    //Unity消息 | 0个引用
    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathSplatter;
        Health.OnDeath -= SpawnDeathVFX;
    }

    // 2个引用
    private void SpawnDeathSplatter(Health sender)
    {
        //生成死亡飞溅预设体
        GameObject splatter = Instantiate(sender.SplatterPrefab, sender.transform.position, sender.transform.rotation);
        //设置预设体父对象为当前handler
        splatter.transform.SetParent(this.transform);
        SpriteRenderer splatterSR = splatter.GetComponent<SpriteRenderer>();
        //获得当前enemy的默认颜色
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        Color defaultColor = colorChanger.DefaultColor;
        //设置死亡飞溅预设体的颜色和enemy颜色一致
        splatterSR.color = defaultColor;
    }
    // 2个引用
    private void SpawnDeathVFX(Health sender)
    {
        GameObject deathVFX = Instantiate(sender.DeathVFX, sender.transform.position, sender.transform.rotation);
        deathVFX.transform.SetParent(this.transform);
        //获取粒子系统渲染器
        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;
        //获得当前enemy的默认颜色
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        Color currentColor = colorChanger.DefaultColor;
        //设置死亡特效的颜色和enemy颜色一致
        ps.startColor = currentColor;
    }
}