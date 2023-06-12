using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class popup : MonoBehaviour
{
    public Text guiText;
    public float temp;

    private void Awake()
    {
        temp = 3f;
    }

    private void FixedUpdate()
    {

       guiText.color = new Color(1f, 1f, 1f, temp);
       temp -= Time.deltaTime;

    }

}
