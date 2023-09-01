using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] int EnemyHP = 3;


    public void TakeDamage(int i)
    {
        EnemyHP -= i;
        if (EnemyHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
