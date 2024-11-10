using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static float MaxHealth{
        get => PlayerPrefs.GetFloat("MaxHealth", 100f);
        set => PlayerPrefs.SetFloat("MaxHealth", value);
    }
    public static float Health{
        get => PlayerPrefs.GetFloat("Health", 100f);
        set => PlayerPrefs.SetFloat("Health", value);
    }

    public static float Damage{
        get => PlayerPrefs.GetFloat("Damage", 10f);
        set => PlayerPrefs.SetFloat("Damage", value);
    }

    public static int HealPotions{
        get => PlayerPrefs.GetInt("HealPotions", 3);
        set => PlayerPrefs.SetInt("HealPotions", value);
    }
}
