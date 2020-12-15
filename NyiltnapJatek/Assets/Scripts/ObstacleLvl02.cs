using UnityEngine;

/// <summary>
/// Class for the brick objects on Lvl02.
/// TODO: More brick types were planned, like 2x speed, /2 speed, etc. Add new stuff?
/// TODO: Randomly generate which bricks are questions.
/// </summary>
public class ObstacleLvl02 : MonoBehaviour
{
    public enum BrickType { Normal, Question};
    [SerializeField] public BrickType brickType = BrickType.Normal;
    [SerializeField] public int health = 1;

    public bool IsQuestionAndDestroyedOnNextHit() { return brickType == BrickType.Question && health == 1; }

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
