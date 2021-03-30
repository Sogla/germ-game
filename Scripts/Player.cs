using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Vector3 firstPosition, lastPosition; //Mouse Position Saves
    Rigidbody2D rb;

    LineRenderer lr;

    [SerializeField] float force = 10;
    bool isReadyToLaunch = true;
    bool freeToLaunch = true;
    bool isGermSucceed = false;
    public bool isGameStarted = false;


    [SerializeField] float deadZone = 1f;
    [SerializeField] bool isMirrorDirOn = false;

    private int jumpCounter = 0;

    private GameObject point;
    public Sprite pointSprite;

    void Start()
    {
        lr = gameObject.AddComponent<LineRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (isGameStarted && !isGermSucceed)
        {
            InputControl();
        }
    }

    void InputControl()
    {
        if (isReadyToLaunch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MakeAPoint();
            }
            if (Input.GetMouseButton(0))
            {
                lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if ((lastPosition - firstPosition).magnitude <= deadZone)
                {
                    SetColor(Color.red);
                    Debug.Log("Red" + (lastPosition - firstPosition).magnitude);
                }
                else
                {
                    SetColor(Color.green);
                    Debug.Log("Green" + (lastPosition - firstPosition).magnitude);
                }
                Vector3 dir = (firstPosition - lastPosition);
                if (isMirrorDirOn)
                {
                    dir = MirroredVector3(dir);
                }
                lr.enabled = true;
                DrawLine(transform.position, dir.normalized);
                RaycastControl(dir);
            }
            else
            {
                
                lr.enabled = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                lr.enabled = false;
                Destroy(point);
                if ((lastPosition - firstPosition).magnitude > deadZone && freeToLaunch)
                {
                    gameObject.transform.SetParent(null);
                    isReadyToLaunch = false;
                    Vector3 dir = (firstPosition - lastPosition).normalized;
                    if (isMirrorDirOn)
                    {
                        dir = MirroredVector3(dir);
                    }
                    rb.AddForce(dir * force, ForceMode2D.Impulse);
                    jumpCounter++;
                }
            }
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isReadyToLaunch = true;
            rb.velocity = Vector2.zero;
            gameObject.transform.SetParent(collision.transform);

        }
        if (collision.gameObject.CompareTag("Target"))
        {
            isGermSucceed = true;
            rb.velocity = Vector2.zero;
            gameObject.transform.SetParent(collision.transform);
        }
    }

    void RaycastControl(Vector3 dir)
    {
        int layermask = 1 << 8;
        layermask = ~layermask;
        transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.up, dir.normalized);
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, layermask);
        RaycastHit2D _1hit = Physics2D.Raycast(transform.GetChild(0).GetChild(0).position, dir, Mathf.Infinity, layermask);
        RaycastHit2D _2hit = Physics2D.Raycast(transform.GetChild(0).GetChild(1).position, dir, Mathf.Infinity, layermask);

        //Debug Block
        Debug.DrawRay(transform.GetChild(0).position, dir.normalized * 10, Color.red);
        Debug.DrawRay(transform.GetChild(0).GetChild(0).position, dir.normalized * 10, Color.green);
        Debug.DrawRay(transform.GetChild(0).GetChild(1).position, dir.normalized * 10, Color.blue);

        if (transform.parent != null)
        {
            GameObject tempObject = transform.parent.gameObject;
            if (_hit.collider.gameObject.Equals(tempObject) || _1hit.collider.gameObject.Equals(tempObject) || _2hit.collider.gameObject.Equals(tempObject))
            {
                freeToLaunch = false;
                lr.enabled = false;
            }
            else
            {
                freeToLaunch = true;
            }
        }
       
    }

    Vector3 MirroredVector3 (Vector3 dir)
    {
        dir.x *= -1f;
        return dir;
    }

    void DrawLine(Vector3 startPos, Vector3 dir)
    {
        List<Vector3> pos = new List<Vector3>();
        pos.Add(startPos);
        pos.Add(startPos + dir*1.5f);
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.SetPositions(pos.ToArray());
        lr.useWorldSpace = true;
    }

    public int GetJumpCounter()
    {
        return jumpCounter;
    }

    GameObject MakeAPoint()
    {
        point = new GameObject("Point");
        point.transform.position = firstPosition - new Vector3(0,0,-1);
        point.transform.localScale = new Vector3(2, 2, 1);
        SpriteRenderer pointSR = point.AddComponent<SpriteRenderer>();
        pointSR.sprite = pointSprite;
        return point;
    }

    void SetColor(Color color)
    {
        lr.startColor = color;
        lr.endColor = color;
    }
}
