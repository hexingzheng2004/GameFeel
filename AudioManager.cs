using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float _masterVolume = 1.0f;//主音量
    //音频资源
    [SerializeField] private SoundsCollectionSO _soundsList;

    //音频混合器分组
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    private AudioSource _currentMusic;//当前音乐播放器

    //Unity消息 | 0个引用
    private void Start()
    {
        PlayBGM();
    }

    //Unity消息 | 0个引用
    private void OnEnable()
    {
        Gun.OnShoot += Gun_Shoot;
        PlayerController.OnJump += Player_Jump;
        Health.OnDeath += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent += PlayDiscoParty;
    }

    //Unity消息 | 0个引用
    private void OnDisable()
    {
        Gun.OnShoot -= Gun_Shoot;
        PlayerController.OnJump -= Player_Jump;
        Health.OnDeath -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent -= PlayDiscoParty;
    }

    // 1个引用
    private void SoundToPlay(SoundSO soundSO)
    {
        //设置音频属性
        AudioClip clip = soundSO.Clip;
        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume * _masterVolume;
        bool loop = soundSO.loop;

        if (soundSO.RandomizePitch)
        {
            float randomPitchModifer = Random.Range(-soundSO.RandomPitchRangeModifer, soundSO.RandomPitchRangeModifer);
            pitch += randomPitchModifer;
        }

        AudioMixerGroup audioMixerGroup;
        switch (soundSO.Type)
        {
            case SoundSO.AudioType.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            case SoundSO.AudioType.SFX:
                audioMixerGroup = _sfxMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }

    /// <summary>
    /// 随机播放音频
    /// </summary>
    /// <param name="soundSOs">音频数组</param>
    // 5个引用
    private void RandomPlaySound(SoundSO[] soundSOs)
    {
        if (soundSOs.Length > 0 && soundSOs != null)
        {
            SoundSO soundSO = soundSOs[Random.Range(0, soundSOs.Length)];
            SoundToPlay(soundSO);
        }
    }

    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="soundSO">音频资源</param>
    // 1个引用
    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject temp = new GameObject("Temp Audio Source");
        AudioSource audioSource = temp.AddComponent<AudioSource>();//音频播放器
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();

        //开始时，播放BGM，击中DiscoBall播放DiscoParty，结束后，重新播放BGM
        if (audioMixerGroup == _musicMixerGroup)
        {
            if (_currentMusic != null)
            {
                _currentMusic.Stop();
            }
            _currentMusic = audioSource;
        }

        //如果不是循环音频，播放完后销毁音频播放对象
        if (!loop) { Destroy(temp, clip.length); }
    }

    //播放背景音乐
    // 1个引用
    private void PlayBGM()
    {
        RandomPlaySound(_soundsList.BGM);
    }

    //播放DiscoParty
    // 2个引用
    private void PlayDiscoParty()
    {
        RandomPlaySound(_soundsList.DiscoParty);
        //DiscoParty音乐时间
        float soundLength = _soundsList.DiscoParty[0].Clip.length;
        Invoke("PlayBGM", soundLength);
    }

    //播放射击音效
    // 2个引用
    private void Gun_Shoot()
    {
        RandomPlaySound(_soundsList.GunShoot);
    }

    //播放跳跃音效
    // 2个引用
    private void Player_Jump()
    {
        RandomPlaySound(_soundsList.Jump);
    }

    //敌人死亡音效
    // 2个引用
    private void Health_OnDeath(Health sender)
    {
        RandomPlaySound(_soundsList.Enemy_Death);
    }
}