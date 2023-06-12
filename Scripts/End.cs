using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    GameObject Panel;

    void Start()
    {
        Panel = GameObject.FindGameObjectWithTag("Panel");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {

            Debug.Log("Quit");
            Application.Quit();
        }


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Self")
        {

            Panel.SetActive(true);
        }
    }
}
