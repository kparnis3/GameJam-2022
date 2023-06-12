using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    /*
     * This script handles the games scoring.
     * It works as a using Singleton and DontDestroyOnLoad to ensure only one of these exist per scene.
     * 
     */

    [SerializeField] MazeGenerator genRef;
    [SerializeField] newLevel levelRef;
   // public static Score singleton;
    public int scorePlayer;
    public Text scoreTextPlayer;
    public int TotalPickup;

    // gameDataLength is used to retrieve how many pickups are set within "GameData" object.
    public void Awake()
    {
        
        GameObject[] gameDataLength = GameObject.FindGameObjectsWithTag("Data");
        /*
        if(gameDataLength.Length > 1) //ensure only one exists.
        {
            Destroy(this.gameObject);

        }

        DontDestroyOnLoad(this.gameObject);
        singleton = this;
        */
    }

    //updatePlayerScore score is called by "Collectable" script, everytime a new Pickup is collected.
    public void updatePlayerScore()
    {
        scorePlayer += 1;
        setScorePlayer();
    }

    /* setScorePlayer is called initially by "MazeGenerator" to set the initial score eg: 0/6,
     * and its called everytime the score needs to be updated.
     * setPlayerScore also keeps track if the player managed to aquire everything there is to collect,
     * to move to the next scene.
     */
    public void setScorePlayer()
    {
        TotalPickup = genRef.TotalPickups;
        scoreTextPlayer.text = "Memories: " + scorePlayer.ToString() + "/" + TotalPickup.ToString();
/*
        if(TotalPickup == scorePlayer)
        {
            //levelRef.generateNextLevel();
            SceneManager.LoadScene("SecondScene");

        } */
    }
}
