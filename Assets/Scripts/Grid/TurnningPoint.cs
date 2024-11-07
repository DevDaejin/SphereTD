using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TurnningPoint : MonoBehaviour
{
    private TMP_Text turnningPointNameTxt;
    [SerializeField] private bool isEndPoint = false;
    [SerializeField] private Vector3 nextPoint;

    private const string enemy = "Enemy";
    private const int endPointValue = -9999;
    public void Init(string turnningPointName, Vector3 nextPoint)
    {
        turnningPointNameTxt = GetComponent<TMP_Text>();
        turnningPointNameTxt.text = turnningPointName;

        gameObject.name = turnningPointName;
        this.nextPoint = nextPoint;

        if(nextPoint.y == endPointValue)
        {
            isEndPoint = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(enemy))
        {
            Debug.Log(nextPoint);

            if (!isEndPoint)
            {
                other.GetComponent<NavMeshAgent>().SetDestination(nextPoint);
            }
            else
            {
                other.GetComponent<EnemyBase>().OnArriveCallback.Invoke();
            }
        }
    }
}
