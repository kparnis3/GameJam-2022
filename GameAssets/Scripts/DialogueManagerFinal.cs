using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManagerFinal : MonoBehaviour
{

    public Text dialogueText;
    public Image dialogueImage;

    public GameObject gameCanvas;
    public GameObject finalCanvas;

    public Animator dialogueAnimator;
    public Animator finalAnimator;

    private Queue<string> sentences;
    public Score other;
    private CanvasGroup other2;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

        GameObject gd = GameObject.FindGameObjectWithTag("Data");
        other = gd.GetComponent<Score>();

        GameObject gd2 = GameObject.FindGameObjectWithTag("end2");
        other2 = gd2.GetComponent<CanvasGroup>();



    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue){

        gameCanvas.SetActive(false);
        dialogueAnimator.SetBool("IsOpen", true);
        Debug.Log("opening dialogue");
        

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

        Debug.Log("ending dialogue");
        dialogueAnimator.SetBool("IsOpen", false);

        if (other2.alpha == 1)
        {
            finalAnimator.SetBool("endGame", true);
        }
        
    }
}
