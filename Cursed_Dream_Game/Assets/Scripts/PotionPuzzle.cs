using System.Collections;
using UnityEngine;

public class PotionPuzzle : MonoBehaviour
{
    public GameObject door;
    private bool isSolved = false;
    private bool playerInRange = false;

    void Update(){
        if (playerInRange && !isSolved && Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Puzzle Resolvido");
            isSolved = true;
            SaveSystem.HealPotions = 3;
            SaveSystem.Health = 100f;
            PlayerPrefs.Save();
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
