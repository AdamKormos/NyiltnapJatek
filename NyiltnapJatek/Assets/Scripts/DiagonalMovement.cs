using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalMovement : MonoBehaviour
{
    bool isOnScreen = false;

    private void OnBecameVisible()
    {
        isOnScreen = true;    
    }

    private void OnBecameInvisible()
    {
        if (isOnScreen) Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnScreen) transform.position += new Vector3(-transform.right.x, -transform.up.y) * Time.deltaTime;
    }
}
