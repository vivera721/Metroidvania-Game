using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    public Slider bossHealthSlider;

    public int currentHealth = 30;

    public BossBattle theBoss;


    void Start()
    {
        bossHealthSlider.maxValue = currentHealth;
        bossHealthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            theBoss.EndBattle();
        }

        bossHealthSlider.value = currentHealth;
    }


}