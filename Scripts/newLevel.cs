using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class newLevel : MonoBehaviour
{
    public void generateNextLevel()
    {
        SceneManager.LoadScene("SecondScene");
    }
}
