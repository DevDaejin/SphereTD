using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get => instance; }
    private static EnemySpawner instance = null;

    [SerializeField] public Vector3 SpawnPoint;

    [SerializeField] private GameObject enemyPrefab;
    private EnemyPool pool;
    private EnemyBase target;

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

        pool.InitializePool(enemyPrefab, GetEnemy, ReleaseEnemy);

        //생성 포인트
        foreach (TurnningPoint tp in FindObjectsOfType<TurnningPoint>())
        {
            if (tp.name.Equals("Start"))
            {
                SpawnPoint = tp.transform.position;
                break;
            }
        };

        //Test logic
        target = pool.GetEnemy();

        Invoke("TestFunc", 1);
    }

    private void TestFunc()
    {
        pool.ReturnEnemy(target);
        pool.GetEnemy();
        pool.GetEnemy();
        pool.GetEnemy();
    }

    private void GetEnemy(EnemyBase enemy)
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

    private void ReleaseEnemy(EnemyBase enemy) 
    {
        enemy.gameObject.SetActive(false);
    }
}