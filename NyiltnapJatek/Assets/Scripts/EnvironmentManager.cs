using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] int movePerSec = 20;
    [SerializeField] int moveStrength = 1;
    [SerializeField] Transform environmentToMove = default;
    [SerializeField] GameObject lastEnvironmentObject = default;
    bool isTouchingSwitchCollider = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(!isTouchingSwitchCollider)
        {
            environmentToMove.position -= new Vector3(0.05f * moveStrength, 0f);
            yield return new WaitForSeconds(1f / movePerSec);
        }

        while(Player.isOnScreen)
        {
            GameNS::StaticData.player.transform.position += new Vector3(0.05f * moveStrength, 0f);
            yield return new WaitForSeconds(1f / movePerSec);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.transform.name);
        if(collision.gameObject.Equals(lastEnvironmentObject))
        {
            Debug.Log("H");
            isTouchingSwitchCollider = true;
        }
    }
}
