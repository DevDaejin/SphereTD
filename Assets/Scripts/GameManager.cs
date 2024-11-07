using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                var e = EnemySpawner.Instance.GetEnemy();
            }
            yield return null;
        }
    }
}
