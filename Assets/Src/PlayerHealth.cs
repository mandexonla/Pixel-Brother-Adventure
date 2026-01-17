using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HeartUI heartUI;

    private SpriteRenderer spriteRenderer;

    public static event Action onPlayerDied;
    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth;
        HealthItem.OnHealthCollect += Heal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
            SoundEffectManager.Play("PlayerHit");
        }
        Trap trap = collision.GetComponent<Trap>();
        if (trap && trap.damage > 0)
        {
            TakeDamage(trap.damage);
        }
        else if (trap)
        {
            //play Random sound
            SoundEffectManager.Play("Bounce");
        }
    }

    void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        heartUI.UpdateHeart(currentHealth);
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        heartUI.SetMaxHeart(maxHealth);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        heartUI.UpdateHeart(currentHealth);

        StartCoroutine(Flashred());

        //Flash red
        if (currentHealth <= 0)
        {
            //Player Death
            onPlayerDied.Invoke();
        }
    }

    private IEnumerator Flashred()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

}
