using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public Image image;
    public Text text;

    GameObject []players;
    private int numberOfPlayers;

    public bool isPlayerSucceed = false;
    public bool isGameFinished = false;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayers = players.Length;
    }

    
    void Update()
    {
        GameStart();
        GameFinish();
    }

    void GameStart()
    {
        bool firstClick = false;
        if (Input.GetMouseButtonUp(0) && !firstClick)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i].GetComponent<Player>().isGameStarted = true;
            }
            firstClick = true;
            image.enabled = false;
            text.enabled = false;
        }
    }

    void GameFinish()
    {
        if (isGameFinished)
        {
            if (isPlayerSucceed)  //Başarı Şartları buraya
            {
                Debug.Log("Win");
            }
            else
            {
                Debug.Log("Lose");
            }
            Time.timeScale = 0f;
            
        }
    }
}
