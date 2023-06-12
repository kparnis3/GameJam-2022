using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeFloorColor : MonoBehaviour
{
    //public MeshRenderer Floor;
    public GameObject[] Sprites;
    public float temp = 0.05f;
    private bool show = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            for (int x = 0; x < Sprites.Length; x++)
            {
                show = true;
            }

            // Do something else?
        }
    }

    private void FixedUpdate()
    {
        if(show)
        {
            temp += Time.deltaTime;
            //float temp2 = temp;
            for (int x = 0; x < Sprites.Length; x++)
            {
                
               //temp2 -= 0.01f;   
               Sprites[x].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, temp);
              
            }

        }
    }


}
