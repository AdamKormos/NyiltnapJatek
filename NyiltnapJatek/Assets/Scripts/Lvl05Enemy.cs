using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl05Enemy : MonoBehaviour
{
    [SerializeField] public int scoreReward = 100;
    bool isAttacking = false;

    private void Update()
    {
        if(isAttacking)
        {
            // ...
        }
    }

    private void OnBecameVisible()
    {
        isAttacking = true;
    }

    private void OnBecameInvisible()
    {
        isAttacking = false;
    }
}
