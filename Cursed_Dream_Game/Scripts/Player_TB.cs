using UnityEngine;

public class Player_TB : MonoBehaviour
{
    private float Health, Damage;
    public float GetDamage(){return Damage;}
    public float GetHealth(){return Health;}
    public void SetHealth(float Health){this.Health = Health;}
    public void SetDamage(float Damage){this.Damage = Damage;}
    public bool IsDead(){
        if(Health <= 0){
            return true;
        }
        return false;
    }
}
