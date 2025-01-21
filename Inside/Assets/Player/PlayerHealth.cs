using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;
    
    public int health;
    public int maxHealth = 6;

    
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        health -= amount;
        OnPlayerDamaged?.Invoke();

        if (health <= 0)
        {
            health = 0;
            Debug.Log("you're dead");
            OnPlayerDeath?.Invoke();
        }
    }
}
