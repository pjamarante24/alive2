using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float startingHealth = 100f;
    public float maxHealth = 100f;
    public Slider slider;
    public Image fillImage;
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;
    public float currentHealth;
    public bool isDead;

    void OnEnable()
    {
        currentHealth = maxHealth;
        startingHealth = currentHealth;
        isDead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        SetHealthUI();

        if (currentHealth <= 0f && !isDead)
        {
            OnDeath();
        }
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        SetHealthUI();
    }

    private void SetHealthUI()
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / maxHealth);
    }

    private void OnDeath()
    {
        isDead = true;
    }
}
