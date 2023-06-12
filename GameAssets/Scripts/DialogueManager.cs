using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public Text dialogueText;
    public Image dialogueImage;

    public GameObject gameCanvas;

    public Animator animator;

    private Queue<string> sentences;
    public Score other;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

        GameObject gd = GameObject.FindGameObjectWithTag("Data");
        other = gd.GetComponent<Score>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue){

        gameCanvas.SetActive(false);
        animator.SetBool("IsOpen", true);

      Time.timeScale = 0;

        dialogueImage.sprite = dialogue.image;

        sentences.Clear();
        
        foreach(string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence(){



        if(sentences.Count == 0){
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }
    
    IEnumerator TypeSentence (string sentence){
        dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue(){

        gameCanvas.SetActive(true);
        animator.SetBool("IsOpen", false);
        Time.timeScale = 1;
        //Debug.Log("End of conversation");


        if (other.TotalPickup == other.scorePlayer)
        {
            //levelRef.generateNextLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }

        

    }


}
