using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    GameObject Pause;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            Time.timeScale = 0;
            Pause.SetActive(true);
        }
    }
}