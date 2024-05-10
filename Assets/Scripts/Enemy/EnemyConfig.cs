using UnityEngine;

public class EnemyConfig : ScriptableObject
{
    [Header("Grid")]
    [SerializeField] private int baseHP = 100;
    public int Defence = 0;
    [SerializeField] private EnemySpeed speedType = EnemySpeed.Normal;

    public float Speed
    {
        get => ConvertTypeToSpeed(speedType);
    }

    private float ConvertTypeToSpeed(EnemySpeed type)
    {
        switch (type)
        {
            case EnemySpeed.VerySlow:
                return 0.5f;

            case EnemySpeed.Slow:
                return 0.75f;

            case EnemySpeed.Fast:
                return 1.25f;

            case EnemySpeed.VeryFast:
                return 1.5f;

            case EnemySpeed.Normal:
            default:
                return 1f;
        }
    }

    public int GetHP(int wave)
    {
        //1.1의 웨이브 곱으로 체력 증가
        return Mathf.RoundToInt(baseHP * Mathf.Pow(1.1f, wave - 1));
    }
}
