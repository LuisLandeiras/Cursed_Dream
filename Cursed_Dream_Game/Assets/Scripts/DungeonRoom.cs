using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public enum RoomType { Combat, Boss, Start } // Combat: Spawna um inimigo; Boss: Sala do Boss; Start: Sala inicial vazia
    public RoomType roomType;
    public GameObject[] enemyPrefabs; // Lista de todos os Prefabs de inimigos que podem spawnar
    public GameObject bossPrefab; // Prefab do Boss
    public Transform[] spawnPoints; // Define o ponto base de spawn e quantos inimigos spawnam

    void Start(){
        if (roomType == RoomType.Combat) SpawnEnemies();
        else if(roomType == RoomType.Boss) SpawnBoss();
    }

    // Metodos responsável por spawnar os inimigos
    void SpawnEnemies(){
        foreach (Transform spawnPoint in spawnPoints){
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // Escolhe um Prefab random da lista de Prefabs para ser o inimigo visual na dungeon
            Vector3 SapwnPosition = new Vector3(Random.Range(spawnPoint.position.x - 3,spawnPoint.position.x + 4),-2.5f,0f); // Spawn o inimigo numa posição random da sala
            Instantiate(enemyPrefab, SapwnPosition, Quaternion.identity); 
        }
    }

    void SpawnBoss(){
        foreach(Transform spawnPoint in spawnPoints){
            Vector3 SapwnPosition = new Vector3(Random.Range(spawnPoint.position.x - 3,spawnPoint.position.x + 4),-2.5f,0f); // Spawn o inimigo numa posição random da sala
            Instantiate(bossPrefab, SapwnPosition, Quaternion.identity);
        }
    }
}
