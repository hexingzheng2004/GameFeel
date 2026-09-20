using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    //1. 管道颜色随机变换
    //2. 变换默认颜色
    //3. 设置默认颜色
    public Color DefaultColor { get; private set; }//默认颜色

    //颜色数组
    [SerializeField] private Color[] _colors;
    //fill精灵渲染器
    [SerializeField] private SpriteRenderer _fillSR;

    // 1个引用
    public void SetRandomColor()
    {
        //随机颜色数组索引 [,)
        int randIndex = Random.Range(0, _colors.Length);
        DefaultColor = _colors[randIndex];
        _fillSR.color = DefaultColor;
    }

    // 2个引用
    public void SetColor(Color color)
    {
        _fillSR.color = color;
    }

    // 2个引用
    public void SetDefaultColor(Color color)
    {
        DefaultColor = color;
        SetColor(color);
    }
}