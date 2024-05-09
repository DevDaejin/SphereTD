using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get => instance; }
    private static EnemySpawner instance = null;

    [SerializeField] public Vector3 SpawnPoint;
    [SerializeField] public Vector3 EndPoint;

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
    }

    private void ReleaseEnemy(EnemyBase enemy) 
    {
    }
}