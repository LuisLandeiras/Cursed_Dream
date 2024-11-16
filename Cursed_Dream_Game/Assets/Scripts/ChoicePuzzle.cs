using UnityEngine;

public class ChoicePuzzle : MonoBehaviour
{
    public GameObject door;
    public Choice[] choices;
    public ChoiceTroll troll;
    public string[] words;
    private string Sentence;
    private int index;

    void Start(){
        Debug.Log($"Correct Sentence: {string.Join(", ", Sentence)}");
        foreach(Choice choice in choices) choice.choicePuzzle = this;
        troll.choicePuzzle = this;
    }

    void Update(){
        if(troll.CheckCorrect() == index) CorrectChoice();
    }

    public void CorrectChoice(){
        Debug.Log("Puzzle solved!");
        CloseDoor doorController = door.GetComponent<CloseDoor>();
        doorController.OpenDoor();
    }

    public void SetSentence_Index(string Sentence, int index){
        this.Sentence = Sentence;
        this.index = index;
    }

    public string[] CutSentence(){
        return Sentence.Trim().Split(' ');
    }

    public string GetSentence(){
        return Sentence;
    }
}
