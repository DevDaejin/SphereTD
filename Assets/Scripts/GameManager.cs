using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        yield return null;

        var es = FindObjectOfType<EnemySpawner>();

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        yield return null;

        var e = es.GetEnemy();

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        yield return null;

        es.GetEnemy();

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        yield return null;

        es.ReturnEnemy(e);

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        yield return null;

        es.GetEnemy();

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        yield return null;

        es.GetEnemy();
    }
}
