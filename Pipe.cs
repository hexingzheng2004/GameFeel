using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnTimer = 3f;

    private ColorChanger _colorChanger;

    //Unity消息 | 0个引用
    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
    }

    //Unity消息 | 0个引用
    private void Start() {
        StartCoroutine(SpawnRoutine());
    }
    
    // 1个引用
    private IEnumerator SpawnRoutine() {
        while (true)
        {
            //管道随机变换颜色
            _colorChanger.SetRandomColor();
            Enemy enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
            //敌人初始化颜色和管道颜色一致
            enemy.Init(_colorChanger.DefaultColor);
            yield return new WaitForSeconds(_spawnTimer);
        }
    }
}