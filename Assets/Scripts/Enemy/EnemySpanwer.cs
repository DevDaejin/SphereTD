using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get => instance; }
    private static EnemySpawner instance = null;

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

        pool.InitailizePool(enemyPrefab);
        target = pool.GetEnemy();

        Invoke("A", 1);
    }

    private void A()
    {
        pool.ReturnEnemy(target);
        pool.GetEnemy();
        pool.GetEnemy();
        pool.GetEnemy();
    }
}