using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, LOST, WON }

public class Battle_System : MonoBehaviour
{
    public GameObject PlayerPrefab, EnemyPrefab, BossPrefab;
    public Transform PlayerBattleStation, EnemyBattleStation;
    public BattleState state;
    private Player_TB PlayerUnit;
    private Enemy_TB EnemyUnit;
    private Text Text_PH, Text_E1, Text_E2, Text_E3;
    private Button HealButton, WideButton, StunButton, CorruptionButton;
    private List<Enemy_TB> Enemies, defeatedEnemies;
    private List<Text> EnemiesHP;
    private int TurnWide, TurnCorruption, TurnStun, TurnRound, Level, CurrentLevel;
    private float NormalAttack, WideAttack, CorruptionAttack, StunAttack, EnemyAttack; // Cada uma destas variaveis tem a fórmula de dano da abilidades
    void Start(){
        Enemies = new List<Enemy_TB>();
        EnemiesHP = new List<Text>();
        defeatedEnemies = new List<Enemy_TB>();
        state = BattleState.START;
        Text_PH = GameObject.FindWithTag("Text_PH").GetComponent<Text>();
        HealButton = GameObject.FindWithTag("Btn_Heal").GetComponent<Button>();
        WideButton = GameObject.FindWithTag("Btn_WideAttack").GetComponent<Button>();
        StunButton = GameObject.FindWithTag("Btn_StunAttack").GetComponent<Button>();
        CorruptionButton = GameObject.FindWithTag("Btn_CorruptionAttack").GetComponent<Button>();
        Text_E1 = GameObject.FindWithTag("E1").GetComponent<Text>();
        Text_E2 = GameObject.FindWithTag("E2").GetComponent<Text>();
        Text_E3 = GameObject.FindWithTag("E3").GetComponent<Text>();
        EnemiesHP.Add(Text_E1);
        EnemiesHP.Add(Text_E2);
        EnemiesHP.Add(Text_E3);
        for(int i = 0; i < EnemiesHP.Count; i++) EnemiesHP[i].enabled = false; 
        TurnWide = TurnCorruption = TurnStun = TurnRound = Level = 0;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle(){
        Debug.Log("RoomLevel: " + SaveSystem.RoomLevel);
        GameObject PlayerGO = Instantiate(PlayerPrefab, PlayerBattleStation);
        PlayerUnit = PlayerGO.GetComponent<Player_TB>();

        PlayerUnit.SetHealth(SaveSystem.Health);
        PlayerUnit.SetDamage(SaveSystem.Damage);
        PlayerUnit.SetCorruption(SaveSystem.Corruption);
        PlayerUnit.SetLuck(SaveSystem.Luck);
        PlayerUnit.SetDefense(SaveSystem.Defense);
        CurrentLevel = SaveSystem.RoomLevel;
        Level = 0;
        Enemies.Clear();

        Vector3 NextEnemyBattleStation = EnemyBattleStation.position;
        if(CurrentLevel == 5){ // Batalha do Boss no Battle Mode
            for(int i = 0; i < 2; i++){
                NextEnemyBattleStation = new Vector3(NextEnemyBattleStation.x + 3.0f, EnemyBattleStation.position.y, 0.0f);
                GameObject EnemyGO = Instantiate(EnemyPrefab, NextEnemyBattleStation, Quaternion.identity);
                EnemyUnit = EnemyGO.GetComponent<Enemy_TB>();
                EnemyUnit.SetLevel(1);
                EnemyUnit.SetStats();
                EnemyUnit.SetStuned(false);
                Enemies.Add(EnemyUnit);
            }
            NextEnemyBattleStation = new Vector3(NextEnemyBattleStation.x + 3.0f, EnemyBattleStation.position.y, 0.0f);
            GameObject BossGO = Instantiate(BossPrefab, NextEnemyBattleStation, Quaternion.identity);
            EnemyUnit = BossGO.GetComponent<Enemy_TB>();
            EnemyUnit.SetLevel(5);
            EnemyUnit.SetStats();
            EnemyUnit.SetStuned(false);
            Enemies.Add(EnemyUnit);
        }else{ // Batalhas normais no Battle Mode
            while(Level < CurrentLevel){
                NextEnemyBattleStation = new Vector3(NextEnemyBattleStation.x + 3.0f, EnemyBattleStation.position.y, 0.0f);
                GameObject EnemyGO = Instantiate(EnemyPrefab, NextEnemyBattleStation, Quaternion.identity);
                EnemyUnit = EnemyGO.GetComponent<Enemy_TB>();
                int LVL = Random.Range(1,4);
                EnemyUnit.SetLevel(LVL);
                EnemyUnit.SetStats();
                EnemyUnit.SetStuned(false);

                if(EnemyUnit.GetLevel() + Level <= CurrentLevel){
                    Enemies.Add(EnemyUnit);
                    Level += EnemyUnit.GetLevel();
                }else{
                    NextEnemyBattleStation = new Vector3(NextEnemyBattleStation.x - 3.0f, EnemyBattleStation.position.y, 0.0f);
                    Destroy(EnemyGO);
                }
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

        /*
        Aqui é para alterar o número de turnos que as abilidades têm de Cooldown
        */

        if(TurnWide + 3 <= TurnRound || TurnWide == 0) WideButton.interactable = true;
        else WideButton.interactable = false;

        if(TurnStun + 3 <= TurnRound || TurnStun == 0) StunButton.interactable = true;
        else StunButton.interactable = false;

        if(TurnCorruption + 4 <= TurnRound || TurnCorruption == 0) CorruptionButton.interactable = true;
        else CorruptionButton.interactable = false;

        TurnRound++;
        Debug.Log("Vez do Jogador");
    }

    public void OnAttackButton(){
        if(state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerAttack());
        Debug.Log("Atacou");
    }

    IEnumerator PlayerAttack(){
        NormalAttack = PlayerUnit.GetDamage() * PlayerUnit.GetDamage() / Enemies[0].GetDefense();
        Enemies[0].SetHealth(Enemies[0].GetHealth() - NormalAttack);

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

    public void OnCorruptionAttackButton(){
        if(state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerCorruptionAttack());
        Debug.Log("Atacou");
    }

    IEnumerator PlayerCorruptionAttack(){
        TurnCorruption = TurnRound;

        CorruptionAttack = PlayerUnit.GetDamage() * PlayerUnit.GetDamage() / Enemies[0].GetDefense() * (1.2f + PlayerUnit.GetCorruption() * 0.2f);
        Enemies[0].SetHealth(Enemies[0].GetHealth() - CorruptionAttack);

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

    public void OnStunAttackButton(){
        if(state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerStunAttack());
        Debug.Log("Atacou");
    }

    IEnumerator PlayerStunAttack(){
        TurnStun = TurnRound;

        StunAttack = PlayerUnit.GetDamage() * PlayerUnit.GetDamage() / Enemies[0].GetDefense();
        Enemies[0].SetHealth(Enemies[0].GetHealth() - StunAttack);
        if(PlayerUnit.GetLuck() * 0.4 > Random.value){
            Enemies[0].SetStuned(true);
        }
        
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

    public void OnWideAttackButton(){
        if(state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerWideAttack());
        Debug.Log("Wide Attack Used");
    }

    IEnumerator PlayerWideAttack(){
        TurnWide = TurnRound;

        yield return new WaitForSeconds(1);

        for(int i = 0; i < Enemies.Count; i++){
            WideAttack = PlayerUnit.GetDamage() * PlayerUnit.GetDamage() / Enemies[i].GetDefense() * 0.7f;
            Enemies[i].SetHealth(Enemies[i].GetHealth() - WideAttack);
            Debug.Log("Dano: " + WideAttack);
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
            SaveSystem.Corruption += 1f;
            PlayerPrefs.Save();
            SceneManager.UnloadSceneAsync("Battle_Mode");
            SceneManager.LoadScene("Mapa");
        }
    }

    IEnumerator EnemyTurn(){
        Debug.Log("Turno do Inimigo"); 
        for(int i = 0; i < Enemies.Count; i++){
            if(Enemies[i].GetStuned() == false){
                EnemyAttack = Enemies[i].GetDamage() * Enemies[i].GetDamage() / PlayerUnit.GetDefense();
                PlayerUnit.SetHealth(PlayerUnit.GetHealth() - EnemyAttack);    
            }else{
                Enemies[i].SetStuned(false);
            }

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
