using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDialogue : MonoBehaviour
{
    public Dialogue dialogue;

    void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.CompareTag("Player")){

            //Debug.Log("collided with pickup");

            TriggerDialogue();
        }

    }


    public void TriggerDialogue(){

        //Debug.Log("trigger");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);

    }
}
