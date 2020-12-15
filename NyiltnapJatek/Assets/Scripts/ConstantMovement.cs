using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used on entities that move to a given direction every frame.
/// </summary>
public class ConstantMovement : MonoBehaviour
{
    [SerializeField] protected Vector3 movePerFrame = default;
    protected bool isOnScreen = false;

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
