using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    private bool playerInRange = false;
    public ChoicePuzzle choicePuzzle;
    public string word;

    void Update(){
        if (playerInRange && Input.GetKeyDown(KeyCode.Space)) ShowInfo();
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) playerInRange = false;
    }

    private void ShowInfo(){
        Debug.Log(word);
    }

    public void SetWord(string word){this.word = word;}
}
