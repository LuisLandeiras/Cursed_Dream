using UnityEngine;

public class Lever : MonoBehaviour
{
    public int LeverIndex;
    public LeverPuzzle leverPuzzle;
    private bool playerInRange = false;
    private int State = 0;

    void Update(){
        if (playerInRange && Input.GetKeyDown(KeyCode.Space)) FlipLever();
    }
    
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) playerInRange = false;
    }

    private void FlipLever(){
        if(State < 5){
            if (leverPuzzle != null){
                State += 1;
                leverPuzzle.LeverFlipped(State, LeverIndex);
            }
        }else{
            State = 0;
            leverPuzzle.LeverFlipped(State, LeverIndex);
        }
    }

    public void SetLeverIndex(int LeverIndex){this.LeverIndex = LeverIndex;}
}
