using UnityEngine;
using System.Collections.Generic;

public class DungeonCorridor : MonoBehaviour
{
    public enum CorridorType { DoorOpen, BossPrep, LeverPuzzle, ButtonPuzzle, ChoicePuzzle }
    public CorridorType corridorType;
    public GameObject DoorPrefab, PuzzlePrefab, TrollPrefab;
    public Transform[] spawnPoints;

    void Start(){
        if (corridorType != CorridorType.DoorOpen) SpawnDoors();
    }

    void SpawnDoors(){   
        GameObject DoorGO, PuzzleGO, TrollGO;
        foreach (Transform spawnPoint in spawnPoints){
            Vector3 doorSpawnPos = new Vector3(spawnPoint.position.x + 10.0f, -2.5f, 0f);
            DoorGO = Instantiate(DoorPrefab, doorSpawnPos, Quaternion.identity);

            switch (corridorType){
                case CorridorType.LeverPuzzle:
                    PuzzleGO = Instantiate(PuzzlePrefab, spawnPoint.position, Quaternion.identity);
                    LeverPuzzle leverPuzzle = PuzzleGO.GetComponent<LeverPuzzle>();
                    leverPuzzle.door = DoorGO;

                    for (int i = 0; i < leverPuzzle.levers.Length; i++){
                        Vector3 leverPos = spawnPoint.position + new Vector3(i * 3.0f, 0, 0);
                        GameObject leverObj = Instantiate(leverPuzzle.levers[i].gameObject, leverPos, Quaternion.identity);
                        Lever lever = leverObj.GetComponent<Lever>();
                        lever.SetLeverIndex(i);
                        lever.leverPuzzle = leverPuzzle;
                        leverPuzzle.levers[i] = lever;
                    }
                    return;

                case CorridorType.ButtonPuzzle:
                    PuzzleGO = Instantiate(PuzzlePrefab, spawnPoint.position, Quaternion.identity);
                    ButtonPuzzle buttonPuzzle = PuzzleGO.GetComponent<ButtonPuzzle>();
                    if (buttonPuzzle != null) buttonPuzzle.door = DoorGO;
                    return;

                case CorridorType.BossPrep:
                    PuzzleGO = Instantiate(PuzzlePrefab, spawnPoint.position, Quaternion.identity);
                    PotionPuzzle potionPuzzle = PuzzleGO.GetComponent<PotionPuzzle>();
                    if (potionPuzzle != null) potionPuzzle.door = DoorGO;
                    return;

                case CorridorType.ChoicePuzzle:
                    PuzzleGO = Instantiate(PuzzlePrefab, spawnPoint.position, Quaternion.identity);
                    ChoicePuzzle choicePuzzle = PuzzleGO.GetComponent<ChoicePuzzle>();
                    choicePuzzle.door = DoorGO;

                    // Troll spawn
                    Vector3 trollSpawnPos = new Vector3(spawnPoint.position.x + 6.0f, -2.5f, 0f);
                    TrollGO = Instantiate(TrollPrefab, trollSpawnPos, Quaternion.identity);
                    ChoiceTroll troll = TrollGO.GetComponent<ChoiceTroll>();

                    Sentences sentences = new Sentences(); 
                    sentences.SetSentences();

                    int randomIndex = Random.Range(0, sentences.GetSentences().Count);
                    string correctSentence = sentences.GetSentences()[randomIndex];
                    List<string> sentenceList = GenerateRandomSentenceList(correctSentence, sentences);

                    choicePuzzle.SetSentence_Index(correctSentence, sentenceList.IndexOf(correctSentence));
                    string[] words = choicePuzzle.CutSentence();

                    troll.SetList(sentenceList);
                    choicePuzzle.troll = troll;

                    for (int i = 0; i < choicePuzzle.choices.Length; i++){
                        Vector3 choicePos = spawnPoint.position + new Vector3(i * -3.0f, 0, 0);
                        GameObject choiceObj = Instantiate(choicePuzzle.choices[i].gameObject, choicePos, Quaternion.identity);
                        Choice choice = choiceObj.GetComponent<Choice>();
                        choice.choicePuzzle = choicePuzzle;
                        choicePuzzle.choices[i] = choice;
                        choice.SetWord(words[i]);
                    }
                    return;
            }
        }
    }

    private List<string> GenerateRandomSentenceList(string correctSentence, Sentences sentences){
        List<string> displaySentences = new List<string> { correctSentence };
        List<string> randomSentences = new List<string>(sentences.GetSentences());
        randomSentences.Remove(correctSentence);

        for (int i = randomSentences.Count - 1; i > 0; i--){
            int randIdx = Random.Range(0, i + 1);
            (randomSentences[i], randomSentences[randIdx]) = (randomSentences[randIdx], randomSentences[i]);
        }

        for (int i = 0; i < 4 && i < randomSentences.Count; i++) displaySentences.Add(randomSentences[i]);

        for (int i = displaySentences.Count - 1; i > 0; i--){
            int randIdx = Random.Range(0, i + 1);
            (displaySentences[i], displaySentences[randIdx]) = (displaySentences[randIdx], displaySentences[i]);
        }

        return displaySentences;
    }
}
