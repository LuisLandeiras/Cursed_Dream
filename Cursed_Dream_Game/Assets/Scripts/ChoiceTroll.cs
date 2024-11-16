using UnityEngine;
using System.Collections.Generic;

public class ChoiceTroll : MonoBehaviour
{
    private bool playerInRange = false;
    public ChoicePuzzle choicePuzzle;
    private int currentCorrectIndex = -1;
    private List<string> Options;

    void Update(){
        if(playerInRange) CheckInput();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            playerInRange = true;
            ShowInfo();
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) playerInRange = false;
    }

    private void ShowInfo(){
        foreach(string Option in Options) Debug.Log(Option);
    }

    private void CheckInput(){
        if(Input.GetKeyDown(KeyCode.Z)) currentCorrectIndex = 0;
        else if(Input.GetKeyDown(KeyCode.X)) currentCorrectIndex = 1;
        else if(Input.GetKeyDown(KeyCode.C)) currentCorrectIndex = 2;
        else if(Input.GetKeyDown(KeyCode.V)) currentCorrectIndex = 3;
        else if(Input.GetKeyDown(KeyCode.B)) currentCorrectIndex = 4;
    }

    public int CheckCorrect(){return currentCorrectIndex;}
    public void SetList(List<string> Options){this.Options = Options;}
}
