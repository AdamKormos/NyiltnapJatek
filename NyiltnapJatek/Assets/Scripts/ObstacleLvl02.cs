using UnityEngine;

public class ObstacleLvl02 : MonoBehaviour
{
    public enum BrickType { Normal, Question};
    [SerializeField] BrickType brickType = BrickType.Normal;
    [SerializeField] int health = 3;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        health--;
        if(health == 0)
        {
            switch(brickType)
            {
                case BrickType.Question:
                    GetComponentInChildren<quizCollider>().OnPlayerTouch();
                    break;
            }
            PlayerLvl02MatFiz.brickCount--;
            Destroy(this.gameObject);
        }
    }
}
