using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TurnningPoint : MonoBehaviour
{
    private TMP_Text turnningPointNameTxt;
    [SerializeField] private Vector3 nextPoint;

    const string ENEMY = "Enemy";

    public void Init(string turnningPointName, Vector3 nextPoint)
    {
        turnningPointNameTxt = GetComponent<TMP_Text>();
        turnningPointNameTxt.text = turnningPointName;

        gameObject.name = turnningPointName;
        this.nextPoint = nextPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(ENEMY))
        {
            Debug.Log(nextPoint);

            if (nextPoint != Vector3.down)
            {
            }
            else
            {
                other.GetComponent<NavMeshAgent>().SetDestination(nextPoint);
            }
        }
    }
}
