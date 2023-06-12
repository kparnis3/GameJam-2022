using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseGame : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Pause;
    void Start()
    {
        Pause = GameObject.FindGameObjectWithTag("Pause");
        Pause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause.SetActive(true);
        }

    }
}
