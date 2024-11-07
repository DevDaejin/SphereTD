using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get => instance; }
    private static EnemySpawner instance = null;

    [SerializeField] private GameObject enemyPrefab;
    private EnemyPool pool;

    public Vector3 SpawnPoint { 
        get
        {
            if(spawnPoint == default)
            {
                spawnPoint = FindFirstObjectByType<GridManager>().GetSpawnPoint();
            }

            return spawnPoint;
        }
    }

    private Vector3 spawnPoint;

    public Vector3 SpawnRotation { 
        get
        {
            if(spawnRotation == default)
            {
                spawnRotation = FindFirstObjectByType<GridManager>().GetSpawnRotation();
            }

            return spawnRotation;
        }
    }

    private Vector3 spawnRotation;

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pool = GetComponent<EnemyPool>() 
            ?? gameObject.AddComponent<EnemyPool>();

        pool.InitializePool(enemyPrefab, OnGetEnemy, OnReleaseEnemy);
    }

    public EnemyBase GetEnemy()
    {
        EnemyBase e = pool.GetEnemy();
        return e;
    }

    public void ReturnEnemy(EnemyBase enemy)
    {
        pool.ReturnEnemy(enemy);
    }


    // callback func
    private void OnGetEnemy(EnemyBase enemy)
    {
        enemy.transform.position = SpawnPoint;
        enemy.transform.eulerAngles = SpawnRotation;
        enemy.gameObject.SetActive(true);

        if(enemy.OnArriveCallback == null)
        {
            enemy.OnArriveCallback += (() =>
            {
                Debug.Log("Arrive");
                // 플레이어 체력 깎이는 로직
            });
        }

        if (enemy.OnDeathCallback == null)
        {
            enemy.OnDeathCallback += (() =>
            {
                Debug.Log("Death");
                // 남은 적 갯수 --
                // 보상이있다면 보상
                // 반환 pool.ReturnEnemy(enemy);
            });
        }
    }

    // callback func
    private void OnReleaseEnemy(EnemyBase enemy) 
    {
        enemy.gameObject.SetActive(false);
    }
}