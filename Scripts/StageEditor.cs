using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StageEditor : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Enemies;
    public float EnemySpeed;

    //public AIPath aiPath;
    void Start()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        updateEnemySpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemies.Length==0)
        {
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            updateEnemySpeed();
        }
    }

    void updateEnemySpeed()
    {
        GameObject ob;
        for (int x = 0; x <= Enemies.Length - 1; x++)
        {
            ob = Enemies[x];
            if (ob != null)
            {
                ob.GetComponent<AstarMod>().aiPath.maxSpeed = 5f;
            }

        }

    }
}

  
