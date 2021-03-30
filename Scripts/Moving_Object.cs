using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Object : MonoBehaviour
{
    
    Vector3 pos1;
    [SerializeField]
    Vector3 pos2 = new Vector3(1, 0, 0);
    [SerializeField]
    bool canDelete = true;
    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    bool MoveBetweenTwoPointsChecker;
    [SerializeField]
    bool rotateChecker;

    private void Start()
    {
        pos1 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    void Update()
    {
        MoveBetweenTwoPoints();
        rotateAround();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isPlayer = collision.gameObject.GetComponent<Player>();

        if (isPlayer && canDelete)
        {
            Destroy(collision.gameObject);
        }
    }

    void MoveBetweenTwoPoints()
    {
        if (MoveBetweenTwoPointsChecker)
        {
            transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed, 1.0f));
        }
    }
    
    void rotateAround()
    {
        if (rotateChecker)
        {
            gameObject.transform.Rotate(0.0f, 0.0f, 0.5f, Space.World);

        }
    }


}
