using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{

    public Sprite[] sprites;
    public float Probability;

    void Start()
    {
        float randNum = Random.Range(0f, 1f);
        GetComponent<SpriteRenderer>().sprite = sprites[returnWall(randNum)];
    }


    int returnWall(float n)
    {
        if (n <= Probability) //current probability is set at 80%, but for later levels this will change
        {
            return 0;
        }
        else
        {
            return Random.Range(1, sprites.Length);
        }
    }
}