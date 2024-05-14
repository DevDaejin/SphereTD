using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get => instance; }
    private static EnemySpawner instance = null;

    [SerializeField] public Vector3 SpawnPoint;

    [SerializeField] private GameObject enemyPrefab;
    private EnemyPool pool;

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

        //생성 포인트
        foreach (TurnningPoint tp in FindObjectsOfType<TurnningPoint>())
        {
            if (tp.name.Equals("Start"))
            {
                SpawnPoint = tp.transform.position;
                break;
            }
        };
    }

    public EnemyBase GetEnemy()
    {
        return pool.GetEnemy();
    }

    public void ReturnEnemy(EnemyBase enemy)
    {
        pool.ReturnEnemy(enemy);
    }


    // callback func
    private void OnGetEnemy(EnemyBase enemy)
    {
        enemy.transform.position = SpawnPoint;
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