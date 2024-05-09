using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private ObjectPool<EnemyBase> pool;
    private bool isInitialized = false;
    private readonly int InitCount = 20;
    private readonly int MaxCount = 30;

    public void InitailizePool(GameObject prefab)
    {
        enemyPrefab = prefab;

        pool = new ObjectPool<EnemyBase>(
            CreateEnemy,
            OnGetEnemy,
            OnReleaseEnemy,
            DestroyEnemy,
            false, // collectionCheck: 풀의 오브젝트가 중복으로 반환되지 않도록 할지, 추가 메모리 사용
            InitCount,
            MaxCount);

        isInitialized = true;
    }
    private EnemyBase CreateEnemy()
    {
        EnemyBase enemy = Instantiate(enemyPrefab, transform).GetComponent<EnemyBase>();
        enemy.gameObject.SetActive(false);
        return enemy;
    }

    private void OnGetEnemy(EnemyBase enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseEnemy(EnemyBase enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void DestroyEnemy(EnemyBase enemy)
    {
        Destroy(enemy.gameObject);
    }

    public EnemyBase GetEnemy()
    {
        CheckInit();
        return pool.Get();
    }

    public void ReturnEnemy(EnemyBase enemy)
    {
        CheckInit();
        pool.Release(enemy);
    }

    private void CheckInit()
    {
        if (!isInitialized) throw new System.Exception("Pool initialization did not proceed.");
    }
}