using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    [SerializeField] private float _flashTime = 0.1f;

    //纹理渲染器组件数组
    private SpriteRenderer[] _spriteRenderers;

    private ColorChanger _colorChanger;

    //Unity消息 | 0个引用
    private void Awake()
    {
        //找到当前对象子物体身上的SpriteRenderer
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    // 1个引用
    public void StartFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    // 1个引用
    private IEnumerator FlashCoroutine()
    {
        //遍历渲染器数组 将所有渲染器的材质替换成闪烁材质
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.material = _whiteFlashMaterial;
            if (_colorChanger)
            {
                _colorChanger.SetColor(Color.white);
            }
        }
        //等待闪烁时间
        yield return new WaitForSeconds(_flashTime);
        //变回默认颜色
        SetDefaultMaterial();
    }

    // 1个引用
    private void SetDefaultMaterial()
    {
        //遍历渲染器数组 将所有渲染器的材质替换成默认材质
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.material = _defaultMaterial;
            if (_colorChanger)
            {
                _colorChanger.SetDefaultColor(_colorChanger.DefaultColor);
            }
        }
    }
}