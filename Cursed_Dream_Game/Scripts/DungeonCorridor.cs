using UnityEngine;

public class DungeonCorridor : MonoBehaviour
{
    public enum CorridorType { DoorOpen, BossPrep, LeverPuzzle, ButtonPuzzle }
    public CorridorType corridorType;
    public GameObject DoorPrefab, PuzzlePrefab;
    public Transform[] spawnPoints;

    void Start(){
        if (corridorType == CorridorType.ButtonPuzzle || corridorType == CorridorType.BossPrep || corridorType == CorridorType.LeverPuzzle) 
            SpawnDoors();
    }

    void SpawnDoors(){   
        GameObject DoorGO, PuzzleGO;
        foreach (Transform spawnPoint in spawnPoints){
            if (corridorType == CorridorType.LeverPuzzle){
                Vector3 SpawnPosition = new Vector3(spawnPoint.position.x + 10.0f, -2.5f, 0f);
                DoorGO = Instantiate(DoorPrefab, SpawnPosition, Quaternion.identity);
                PuzzleGO = Instantiate(PuzzlePrefab, spawnPoint.position, Quaternion.identity);
                LeverPuzzle leverPuzzle = PuzzleGO.GetComponent<LeverPuzzle>();
                leverPuzzle.door = DoorGO;
                
                for (int i = 0; i < leverPuzzle.levers.Length; i++){
                    Vector3 leverPosition = spawnPoint.position + new Vector3(i * 3.0f, 0, 0);
                    GameObject leverObj = Instantiate(leverPuzzle.levers[i].gameObject, leverPosition, Quaternion.identity);
                    Lever lever = leverObj.GetComponent<Lever>();
                    lever.SetLeverIndex(i);
                    lever.leverPuzzle = leverPuzzle;
                    leverPuzzle.levers[i] = lever;
                }
                
                return;
            }else if(corridorType == CorridorType.ButtonPuzzle){
                Vector3 SpawnPosition = new Vector3(spawnPoint.position.x + 10.0f, -2.5f, 0f);
                DoorGO = Instantiate(DoorPrefab, SpawnPosition, Quaternion.identity);
                PuzzleGO = Instantiate(PuzzlePrefab, spawnPoint.position, Quaternion.identity);
                ButtonPuzzle puzzle = PuzzleGO.GetComponent<ButtonPuzzle>();

                if (puzzle != null){puzzle.door = DoorGO;}
                return;
            }else if(corridorType == CorridorType.BossPrep){
                Vector3 SpawnPosition = new Vector3(spawnPoint.position.x + 10.0f, -2.5f, 0f);
                DoorGO = Instantiate(DoorPrefab, SpawnPosition, Quaternion.identity);
                PuzzleGO = Instantiate(PuzzlePrefab, spawnPoint.position, Quaternion.identity);
                PotionPuzzle potionpuzzle = PuzzleGO.GetComponent<PotionPuzzle>();
                
                if (potionpuzzle != null){potionpuzzle.door = DoorGO;}
                return;
            }
        }
    }
}
