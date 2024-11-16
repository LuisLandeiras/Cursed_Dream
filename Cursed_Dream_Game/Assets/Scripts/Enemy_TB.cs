using UnityEngine;

/*
Aqui é para alterar os Stats dos inimigos
!!!Atenção!!! Level 5 é o Boss
*/

public class Enemy_TB : MonoBehaviour
{
    private float Damage, Health, Defense;
    private int Level;
    private bool Stuned;
    public float GetHealth(){return Health;}
    public float GetDamage(){return Damage;}
    public float GetDefense(){return Defense;}
    public int GetLevel(){return Level;}
    public bool GetStuned(){return Stuned;}
    public void SetLevel(int Level){this.Level = Level;}
    public void SetHealth(float Health){this.Health = Health;}
    public void SetDamage(float Damage){this.Damage = Damage;}
    public void SetDefense(float Defense){this.Defense = Defense;}
    public void SetStuned(bool Stuned){this.Stuned = Stuned;}
    public bool IsDead(){
        if(Health <= 0){
            return true;
        }
        return false;
    }

    public void SetStats(){
        if(Level == 1){
            SetHealth(Random.Range(5,11));
            SetDamage(Random.Range(1,5));
            SetDefense(Random.Range(5,20));
        }else if(Level == 2){
            SetHealth(Random.Range(5,11));
            SetDamage(Random.Range(1,5));
            SetDefense(Random.Range(5,20));
        }else if(Level == 3){
            SetHealth(Random.Range(5,11));
            SetDamage(Random.Range(1,5));
            SetDefense(Random.Range(5,20));
        }else if(Level == 5){
            SetHealth(Random.Range(10,11));
            SetDamage(Random.Range(4,5));
            SetDefense(Random.Range(19,20));
        }
    }
}
