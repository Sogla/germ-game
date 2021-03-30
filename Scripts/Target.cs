using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    GameObject manager;

    GameObject []players;
    private int numberOfPlayer;
    private int successCount;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");

        players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayer = players.Length;
    }

    // Update is called once per frame
    void Update()
    {
        SuccessControl();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            successCount++;
        }
    }

    void SuccessControl()
    {
        if (successCount == numberOfPlayer)
        {
            manager.GetComponent<Game_Manager>().isPlayerSucceed = true;
            manager.GetComponent<Game_Manager>().isGameFinished = true;
        }
    }

}
