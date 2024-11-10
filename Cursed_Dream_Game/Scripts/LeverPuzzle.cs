using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    public GameObject door;
    public Lever[] levers;
    private List<int> correctSequence = new List<int>();
    private List<int> currentSequence = new List<int> { 0, 0, 0 };

    void Start(){
        correctSequence.Add(Random.Range(0,6));
        correctSequence.Add(Random.Range(0,6));
        correctSequence.Add(Random.Range(0,6));
        Debug.Log($"Correct Sequence: {string.Join(", ", correctSequence)}");
        foreach (Lever lever in levers) lever.leverPuzzle = this;
    }
    public void LeverFlipped(int State, int leverIndex){
        currentSequence[leverIndex] = State;
        Debug.Log($"Current Sequence: {string.Join(", ", currentSequence)}");
        
        if (currentSequence.SequenceEqual(correctSequence)){
            Debug.Log("Puzzle solved!");
            CloseDoor doorController = door.GetComponent<CloseDoor>();
            doorController.OpenDoor();
        }
    }
}
