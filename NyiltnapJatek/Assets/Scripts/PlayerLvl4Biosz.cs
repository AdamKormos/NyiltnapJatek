using System.Collections;
using UnityEngine;

public class PlayerLvl4Biosz : MonoBehaviour
{
    private Rigidbody2D body = default;
    private GameObject arrow = default;

    [SerializeField] private float velocity = 40f;
    [SerializeField] private float rotationScale = -20f;
    //private float rotation = 0.0f;
    //private float rotationCount = 10f;


    bool canRotate = true;
    bool checkCollision = false;
    bool rotationSupport = true;


    

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        arrow = transform.GetComponentsInChildren<Transform>()[1].gameObject;
    }

    void Update()
    {
        if (canRotate)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //body.velocity = new Vector2(0.0f, 0.0f);
                //body.AddRelativeForce(new Vector2(arrow.transform.position.x, arrow.transform.position.y), ForceMode2D.Force);
                //body.AddForceAtPosition(new Vector2(0.0f, 15.0f), new Vector2(arrow.transform.position.x, arrow.transform.position.y), ForceMode2D.Impulse);
                //body.AddTorque(velocity, ForceMode2D.Impulse);
                body.AddForceAtPosition(new Vector3(0.0f, 0.0f, velocity), new Vector3(transform.position.x, transform.position.y, transform.position.z), ForceMode2D.Impulse);
                arrow.SetActive(false);
                //canRotate = false;
                checkCollision = true;
            }
            else if ((rotationSupport && transform.rotation.eulerAngles.z >= 90.0f ) || (!rotationSupport && transform.rotation.eulerAngles.z <= 270))
            {
                rotationScale = -rotationScale;
                rotationSupport = !rotationSupport;
                //transform.rotation.eulerAngles.y = 2f;
                //transform.rotation.eulerAngles.Set(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationSupport ? 275f : 75f);
                //transform.rotation.eulerAngles.Set(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + rotationScale);
                transform.rotation.Normalize();
                //transform.Rotate(Vector3.forward * rotationScale * Time.deltaTime * 15, Space.Self);
            }
            transform.Rotate(Vector3.forward * rotationScale * Time.deltaTime, Space.Self);
        }
    }


    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (checkCollision)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            arrow.SetActive(true);
            canRotate = true;
            checkCollision = false;
        }
    }
}
