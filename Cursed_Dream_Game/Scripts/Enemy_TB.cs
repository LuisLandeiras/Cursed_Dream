using UnityEngine;
using UnityEngine.UI;

public class Enemy_TB : MonoBehaviour
{
    private float Damage, Health;
    private int Level;
    public float GetHealth(){return Health;}
    public float GetDamage(){return Damage;}
    public int GetLevel(){return Level;}
    public void SetLevel(){this.Level = Random.Range(1,4);}
    public void SetHealth(float Health){this.Health = Health;}
    public void SetDamage(float Damage){this.Damage = Damage;}
    public bool IsDead(){
        if(Health <= 0){
            return true;
        }
        return false;
    }

    public void SetStats(){
        if(Level == 1){
            SetHealth(Random.Range(5,10));
            SetDamage(Random.Range(1,3));
        }else if(Level == 2){
            SetHealth(Random.Range(11,13));
            SetDamage(Random.Range(3,5));
        }else if(Level == 3){
            SetHealth(Random.Range(13,15));
            SetDamage(Random.Range(5,8));
        }
    }
}
