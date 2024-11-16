using UnityEngine;

public class Player_TB : MonoBehaviour
{
    private float Health, Damage, Defense, Corruption, Luck;
    public float GetDamage(){return Damage;}
    public float GetHealth(){return Health;}
    public float GetDefense(){return Defense;}
    public float GetLuck(){return Luck;}
    public float GetCorruption(){return Corruption;}
    public void SetHealth(float Health){this.Health = Health;}
    public void SetDamage(float Damage){this.Damage = Damage;}
    public void SetDefense(float Defense){this.Defense = Defense;}
    public void SetLuck(float Luck){this.Luck = Luck;}
    public void SetCorruption(float Corruption){this.Corruption = Corruption;}
    public bool IsDead(){
        if(Health <= 0){
            return true;
        }
        return false;
    }
}
