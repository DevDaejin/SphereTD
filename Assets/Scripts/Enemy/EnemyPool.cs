using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int InitCount = 20;
    [SerializeField] private int MaxCount = 30;

    private ObjectPool<EnemyBase> pool;
    private bool isInitialized = false;

    public void InitializePool(GameObject prefab, Action<EnemyBase> OnGetEnemy, Action<EnemyBase> OnReleaseEnemy)
    {
        enemyPrefab = prefab;

        pool = new ObjectPool<EnemyBase>(
            CreateEnemy,
            OnGetEnemy, // 꺼내기 시 콜백
            OnReleaseEnemy, // 넣기 시 콜백
            DestroyEnemy,
            false, // collectionCheck: 풀의 오브젝트가 중복으로 반환되지 않도록 할지, 추가 메모리 사용
            InitCount,
            MaxCount);

        isInitialized = true;
    }

    private EnemyBase CreateEnemy()
    {// 풀 오브젝트 생성
        EnemyBase enemy = Instantiate(enemyPrefab, transform).GetComponent<EnemyBase>();
        enemy.gameObject.SetActive(false);
        return enemy;
    }

    private void DestroyEnemy(EnemyBase enemy)
    {// 풀 오브젝트 제거
        Destroy(enemy.gameObject);
    }

    public EnemyBase GetEnemy()
    {// 오브젝트 꺼내기
        CheckInit();
        return pool.Get();
    }

    public void ReturnEnemy(EnemyBase enemy)
    {// 오브젝트 반환
        CheckInit();
        pool.Release(enemy);
    }

    private void CheckInit()
    {// 초기화 체크
        if (!isInitialized) throw new System.Exception("Pool initialization did not proceed.");
    }
}