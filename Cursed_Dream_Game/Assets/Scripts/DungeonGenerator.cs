using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject RoomPrefab, OpenCorridorPrefab, ButtonCorridorPrefab, BossCorridorPrefab, BossRoomPrefab, LeverCorridorPrefab, ChoiceCorridorPrefab;
    private int maxRooms = 4;
    private float roomSpacing = 10.0f;

    void Start() { GenerateDungeon(); }

    void GenerateDungeon(){
        Vector3 nextPosition = Vector3.zero;
        GameObject room, CorridorPrefab;
        DungeonRoom roomScript;
        DungeonCorridor corridorScript;

        for (int i = 0; i < maxRooms; i++){
            if (i == 0){
                room = Instantiate(RoomPrefab, nextPosition, Quaternion.identity);
                roomScript = room.GetComponent<DungeonRoom>();
                roomScript.roomType = DungeonRoom.RoomType.Start;

                nextPosition += Vector3.right * (roomSpacing + 10);

                CorridorPrefab = Instantiate(ButtonCorridorPrefab, nextPosition, Quaternion.identity);
                corridorScript = CorridorPrefab.GetComponent<DungeonCorridor>();
                corridorScript.corridorType = DungeonCorridor.CorridorType.ButtonPuzzle;
            
                nextPosition += Vector3.right * (roomSpacing + 10);
            }else if (i == 1){
                room = Instantiate(RoomPrefab, nextPosition, Quaternion.identity);
                roomScript = room.GetComponent<DungeonRoom>();
                roomScript.roomType = DungeonRoom.RoomType.Combat;

                nextPosition += Vector3.right * (roomSpacing + 10);

                CorridorPrefab = Instantiate(ChoiceCorridorPrefab, nextPosition, Quaternion.identity);
                corridorScript = CorridorPrefab.GetComponent<DungeonCorridor>();
                corridorScript.corridorType = DungeonCorridor.CorridorType.ChoicePuzzle;

                nextPosition += Vector3.right * (roomSpacing + 10);
            }else if (i == 2){
                room = Instantiate(RoomPrefab, nextPosition, Quaternion.identity);
                roomScript = room.GetComponent<DungeonRoom>();
                roomScript.roomType = DungeonRoom.RoomType.Combat;

                nextPosition += Vector3.right * (roomSpacing + 10);

                CorridorPrefab = Instantiate(LeverCorridorPrefab, nextPosition, Quaternion.identity);
                corridorScript = CorridorPrefab.GetComponent<DungeonCorridor>();
                corridorScript.corridorType = DungeonCorridor.CorridorType.LeverPuzzle;

                nextPosition += Vector3.right * (roomSpacing + 10);
            }else if (i == maxRooms - 1){
                room = Instantiate(RoomPrefab, nextPosition, Quaternion.identity);
                roomScript = room.GetComponent<DungeonRoom>();
                roomScript.roomType = DungeonRoom.RoomType.Combat;

                nextPosition += Vector3.right * (roomSpacing + 10);

                CorridorPrefab = Instantiate(BossCorridorPrefab, nextPosition, Quaternion.identity);
                corridorScript = CorridorPrefab.GetComponent<DungeonCorridor>();
                corridorScript.corridorType = DungeonCorridor.CorridorType.BossPrep;

                nextPosition += Vector3.right * (roomSpacing + 10);

                room = Instantiate(BossRoomPrefab, nextPosition, Quaternion.identity);
                roomScript = room.GetComponent<DungeonRoom>();
                roomScript.roomType = DungeonRoom.RoomType.Boss;
            }
        }
    }
}
