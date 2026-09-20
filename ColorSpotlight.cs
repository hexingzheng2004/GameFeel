using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpotlight : MonoBehaviour
{
    //随机角度旋转摆动，在一定范围内
    [SerializeField] private float _maxRotation = 45f;
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private float _discoRotationSpeed = 120f;
    [SerializeField] private GameObject _spotlight;

    private float _currentRotation;

    //Unity消息 | 0个引用
    private void Start()
    {
        RandomStartRotation();
    }

    //Unity消息 | 0个引用
    private void Update()
    {
        RotateHead();
    }

    // 1个引用
    private void RotateHead()
    {
        _currentRotation += Time.deltaTime * _rotationSpeed;
        float z = Mathf.PingPong(_currentRotation, _maxRotation);
        _spotlight.transform.localRotation = Quaternion.Euler(0, 0, z);
    }
    
    // 1个引用
    private void RandomStartRotation()
    {
        float randomStartZ = Random.Range(-_maxRotation, _maxRotation);
        _spotlight.transform.localRotation = Quaternion.Euler(0, 0, randomStartZ);
        _currentRotation = randomStartZ + _maxRotation;
    }

    // 1个引用
    public IEnumerator SpotlightDiscoPartyCoroutine(float discoTime)
    {
        float defaultRotationSpeed = _rotationSpeed;
        _rotationSpeed = _discoRotationSpeed;
        yield return new WaitForSeconds(discoTime);
        _rotationSpeed = defaultRotationSpeed;
    }
}