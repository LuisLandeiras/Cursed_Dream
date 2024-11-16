using System.Collections;
using UnityEngine;

public class ButtonPuzzle : MonoBehaviour
{
    public GameObject door; // Reference to the door to be opened
    private bool isSolved = false;
    private bool playerInRange = false;

    void Update(){
        if (playerInRange && !isSolved && Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Puzzle Resolvido");
            isSolved = true;
            StartCoroutine(OpenDoorWithDelay());
        }
    }

    IEnumerator OpenDoorWithDelay(){
        yield return new WaitForSeconds(1);
        CloseDoor doorController = door.GetComponent<CloseDoor>();
        doorController.OpenDoor();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")) playerInRange = true;
        
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")) playerInRange = false;
    }
}
