using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float MoveSpeed;
    private Vector3 screen;

    void Start(){
        MoveSpeed = 5.0f;
        SaveSystem.RoomLevel = 0;
        screen = Camera.main.transform.position;
    }
    
    void Update(){
        if (Input.GetAxis("Horizontal") != 0) transform.Translate(Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime, 0f, 0f);
        
        Camera.main.transform.position = new Vector3(transform.position.x,transform.position.y,screen.z);

        Vector3 PlayerScale = transform.localScale;
        if (Input.GetAxis("Horizontal") < 0) PlayerScale.x = -1.0f;
        else if (Input.GetAxis("Horizontal") > 0) PlayerScale.x = 1.0f;
        transform.localScale = PlayerScale;
    }

    // Função que identifica a colisão com um inimigo para iniciar a cena de batalha
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Enemy")){
            Enemy dungeonEnemy = collision.gameObject.GetComponent<Enemy>();
            SaveSystem.RoomLevel += 1;
            PlayerPrefs.Save();
            SceneManager.LoadScene("Battle_Mode", LoadSceneMode.Additive);
            Destroy(dungeonEnemy.gameObject);
        }else if(collision.gameObject.CompareTag("Boss")){
            Enemy dungeonEnemy = collision.gameObject.GetComponent<Enemy>();
            SaveSystem.RoomLevel = 5;
            PlayerPrefs.Save();
            SceneManager.LoadScene("Battle_Mode", LoadSceneMode.Additive);
            Destroy(dungeonEnemy.gameObject);
        }
    }
}

