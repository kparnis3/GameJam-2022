using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject myGameManager;
    public Score other;
    /*
     * Stored within player object, updates the score by first checking if a collision with a pickup is found,
    * If it is, the gameObject is deactivated and the score is updated. 
    *
    */
    
    void Awake()
    {
        // Setting up the reference.
        GameObject gd = GameObject.FindGameObjectWithTag("Data");
        other = gd.GetComponent<Score>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("Pickup"))
        {
            collision.gameObject.SetActive(false);
            other.updatePlayerScore();
            // Do something else?
        }
    }
}
