using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, LOST, WON }

public class Battle_System : MonoBehaviour
{
    public GameObject PlayerPrefab, EnemyPrefab, EnemyHealthPrefab;
    public Transform PlayerBattleStation, EnemyBattleStation, EnemyHPBattleStation;
    public BattleState state;
    private Player_TB PlayerUnit;
    private Enemy_TB EnemyUnit;
    private Text Text_PH, Text_E1, Text_E2, Text_E3;
    private Button HealButton, HabilityButton;
    private List<Enemy_TB> Enemies, defeatedEnemies;
    private List<Text> EnemiesHP;
    private int Turn, TurnRound, Level;
    void Start(){
        Enemies = new List<Enemy_TB>();
        EnemiesHP = new List<Text>();
        defeatedEnemies = new List<Enemy_TB>();
        state = BattleState.START;
        Text_PH = GameObject.FindWithTag("Text_PH").GetComponent<Text>();
        HealButton = GameObject.FindWithTag("Btn_Heal").GetComponent<Button>();
        HabilityButton = GameObject.FindWithTag("Btn_Hability").GetComponent<Button>();
        Text_E1 = GameObject.FindWithTag("E1").GetComponent<Text>();
        Text_E2 = GameObject.FindWithTag("E2").GetComponent<Text>();
        Text_E3 = GameObject.FindWithTag("E3").GetComponent<Text>();
        EnemiesHP.Add(Text_E1);
        EnemiesHP.Add(Text_E2);
        EnemiesHP.Add(Text_E3);
        for(int i = 0; i < EnemiesHP.Count; i++) EnemiesHP[i].enabled = false; 
        Turn = 0;
        TurnRound = 0;
        Level = 0;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle(){
        GameObject PlayerGO = Instantiate(PlayerPrefab, PlayerBattleStation);
        PlayerUnit = PlayerGO.GetComponent<Player_TB>();

        PlayerUnit.SetHealth(SaveSystem.Health);
        PlayerUnit.SetDamage(SaveSystem.Damage);

        Level = 0;
        Enemies.Clear();

        Vector3 NextEnemyBattleStation = EnemyBattleStation.position;
        while(Level < 3){
            NextEnemyBattleStation = new Vector3(NextEnemyBattleStation.x + 3.0f, EnemyBattleStation.position.y, 0.0f);
            GameObject EnemyGO = Instantiate(EnemyPrefab, NextEnemyBattleStation, Quaternion.identity);
            EnemyUnit = EnemyGO.GetComponent<Enemy_TB>();
            EnemyUnit.SetStats();
            EnemyUnit.SetLevel();
        
            if(EnemyUnit.GetLevel() + Level <= 3){
                EnemyUnit.SetStats();
                Enemies.Add(EnemyUnit);
                Level += EnemyUnit.GetLevel();
            }else{
                NextEnemyBattleStation = new Vector3(NextEnemyBattleStation.x - 3.0f, EnemyBattleStation.position.y, 0.0f);
                Destroy(EnemyGO);
            }
        }

        Text_PH.text = PlayerUnit.GetHealth().ToString();
        for(int i = 0; i < Enemies.Count; i++){
            EnemiesHP[i].enabled = true;
            EnemiesHP[i].text = Enemies[i].GetHealth().ToString();
            Debug.Log("Enemy " + i + " HP: " + Enemies[i].GetHealth());
            Debug.Log("Enemy " + i + " DMG: " + Enemies[i].GetDamage());
            Debug.Log("Enemy " + i + " LVL: " + Enemies[i].GetLevel());
        }

        yield return new WaitForSeconds(1);

        state = BattleState.PLAYERTURN; 
        PlayerTurn();
    }

    void PlayerTurn(){
        if(SaveSystem.HealPotions <= 0) HealButton.interactable = false;

        if(Turn + 2 <= TurnRound || Turn == 0) HabilityButton.interactable = true;
        else HabilityButton.interactable = false;

        TurnRound++;
        Debug.Log("Vez do Jogador");
    }

    public void OnAttackButton(){
        if(state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerAttack());
        Debug.Log("Atacou");
    }

    IEnumerator PlayerAttack(){
        Enemies[0].SetHealth(Enemies[0].GetHealth() - PlayerUnit.GetDamage());

        if(Enemies[0].GetHealth() > 0){
            EnemiesHP[0].text = Enemies[0].GetHealth().ToString();
        }else{
            defeatedEnemies.Clear();
            foreach (Enemy_TB enemy in Enemies){
                if (enemy != null && enemy.GetHealth() <= 0){
                    defeatedEnemies.Add(enemy);
                }
            }
            foreach (Enemy_TB defeated in defeatedEnemies){
                if (Enemies.Contains(defeated)){
                    Enemies.Remove(defeated);
                    Destroy(defeated.gameObject);
                }
            }
            if (EnemiesHP.Count > 0){
                EnemiesHP[0].enabled = false;
                EnemiesHP.RemoveAt(0);
            }
            Debug.Log("Numero de inimigos: " + Enemies.Count);
        }

        yield return new WaitForSeconds(1);

        if(Enemies.Count == 0){
            state = BattleState.WON;
            SaveSystem.Health = PlayerUnit.GetHealth();
            PlayerPrefs.Save();
            Debug.Log(SaveSystem.Health);
            EndBattle();
        }else{
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnHealButton(){
        if(state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerHeal());
        Debug.Log("Healed");
        Debug.Log("Potions: " + SaveSystem.HealPotions.ToString());
    }

    IEnumerator PlayerHeal(){
        if((PlayerUnit.GetHealth() + 15f) > SaveSystem.MaxHealth){
            PlayerUnit.SetHealth(100f);
        }
        Text_PH.text = PlayerUnit.GetHealth().ToString();
        SaveSystem.HealPotions -= 1; 

        yield return new WaitForSeconds(1);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAbilityButton(){
        if(state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerAbility());
        Debug.Log("Ability Used");
    }

    IEnumerator PlayerAbility(){
        Turn = TurnRound;

        yield return new WaitForSeconds(1);

        for(int i  = 0; i < Enemies.Count; i++){
            Enemies[i].SetHealth(Enemies[i].GetHealth() - 5);
            if(Enemies[i].GetHealth() <= 0){
                defeatedEnemies.Clear();
                defeatedEnemies.Add(Enemies[i]);

                foreach (Enemy_TB defeated in defeatedEnemies){
                    if (Enemies.Contains(defeated)){
                        Enemies.Remove(defeated);
                        Destroy(defeated.gameObject);
                    }
                }
                if (EnemiesHP.Count > 0){
                    EnemiesHP[i].enabled = false;
                    EnemiesHP.RemoveAt(i);
                }
                Debug.Log("Numero de inimigos: " + Enemies.Count);
            }else{
                EnemiesHP[i].text = Enemies[i].GetHealth().ToString();
            }
        }

        if(Enemies.Count == 0){
            state = BattleState.WON;
            SaveSystem.Health = PlayerUnit.GetHealth();
            PlayerPrefs.Save();
            Debug.Log(SaveSystem.Health);
            EndBattle();
        }else{
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    
    void EndBattle(){
        if (state == BattleState.WON){
            SaveSystem.Health = PlayerUnit.GetHealth();
            PlayerPrefs.Save();
            SceneManager.UnloadSceneAsync("Battle_Mode");
        }else if (state == BattleState.LOST){
            SaveSystem.HealPotions = 3;
            SaveSystem.Health = 100f;
            SaveSystem.Damage = 10f;
            SceneManager.UnloadSceneAsync("Battle_Mode");
            SceneManager.LoadScene("Mapa");
        }
    }

    IEnumerator EnemyTurn(){
        Debug.Log("Turno do Inimigo"); 

        for(int i = 0; i < Enemies.Count; i++){
            PlayerUnit.SetHealth(PlayerUnit.GetHealth() - Enemies[i].GetDamage());
            Text_PH.text = PlayerUnit.GetHealth().ToString();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        if(PlayerUnit.IsDead()){
            state = BattleState.LOST;
            EndBattle();
        }else{
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
}
