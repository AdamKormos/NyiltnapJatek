using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    [SerializeField] Vector3 movePerFrame = default;
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
        if (isOnScreen) transform.position += movePerFrame * Time.deltaTime;
    }
}
