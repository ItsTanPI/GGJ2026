using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private bool invulnerable = false;


    [SerializeField] Animator _animatorController;


    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public bool IsAlive => currentHealth > 0;
    public bool IsDead => currentHealth <= 0;
    public float HealthPercentage => (float)currentHealth / maxHealth;

    public Action<int, int> OnDamageTaken; 
    public Action<int, int> OnHealed; 
    public Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public bool TakeDamage(int damage)
    {
        if (invulnerable || IsDead)
            return false;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        OnDamageTaken?.Invoke(currentHealth, damage);
        _animatorController.SetTrigger("HeavyHit");
        if (IsDead)
        {
            Die();
        }

        return true;
    }

    public bool Heal(int healAmount)
    {
        if (IsDead)
            return false;

        int previousHealth = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        int actualHealed = currentHealth - previousHealth;

        if (actualHealed > 0)
        {
            OnHealed?.Invoke(actualHealed, currentHealth);
            return true;
        }

        return false;
    }


    public void FullHeal()
    {
        Heal(maxHealth);
    }

    public void Death()
    {
        TakeDamage(currentHealth);
    }

    public void SetInvulnerable(bool state)
    {
        invulnerable = state;
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        invulnerable = false;
    }
}
