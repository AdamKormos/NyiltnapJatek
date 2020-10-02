using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpStrength = 3;
    bool isOnGround = true;
    public static bool isOnScreen { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GameNS::StaticData.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(IsOnGround());
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isOnGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isOnGround = false;
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
        // Értékelés
    }
}
