﻿using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl4Biosz : Player
{
    private Rigidbody2D body = default;
    //bool upsideDown = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;

        StartCoroutine(Move());
    }

    void Update()
    {
        if (body.velocity.y == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                body.gravityScale = -body.gravityScale;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (checkCollision)
        //{
        //    body.velocity = new Vector2(0f, 0f);
        //    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
        //    arrow.SetActive(true);
        //    canRotate = true;
        //    checkCollision = false;
        //}
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;

        if (Camera.main != null)
        {
            if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
            {
                OnGameOver();
            }
            else if (transform.position.y < Camera.main.transform.position.y + Camera.main.orthographicSize)
            {
                if (levelCompletionPanelParent != null)
                {
                    LevelSelection.OnLevelCompleted();
                }
            }
        }
    }
}
