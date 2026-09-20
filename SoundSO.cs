using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundSO : ScriptableObject
{
    //音频类型：音乐和特效
    public enum AudioType
    {
        Music,
        SFX
    }

    public AudioType Type;//音频类型
    public AudioClip Clip;//音频片段
    public bool loop = false;//是否循环
    public bool RandomizePitch = false;//是否随机音高
    [Range(0f, 1f)]
    public float RandomPitchRangeModifer = 0.2f;//随机音高修改器
    [Range(0f, 2f)]
    public float Volume = 1.0f;//音量大小
    [Range(0f, 2f)]
    public float Pitch = 1.0f;//音高大小
}