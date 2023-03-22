using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] public int playerHP;
    [SerializeField] public int playerMaxHP;
    [SerializeField] private float playerBlinkTime;
    [SerializeField] private float blinkTimer;
    private Renderer playerRenderer;

    private void Start()
    {
        playerHP = playerMaxHP;
        GameObject.Find("Health").GetComponent<HealthBar>().AlterHPValue(playerHP, playerMaxHP);
        playerRenderer = GetComponent<Renderer>();
    }

    public void PlayerTakeDamage(int damage)
    {
        if (Time.time > blinkTimer)
        {
            playerHP -= damage;
            GameObject.Find("Health").GetComponent<HealthBar>().AlterHPValue(playerHP, playerMaxHP);
            GetComponent<ScreenFlash>().FlashScreen();
            if (playerHP <= 0)
            {
                Destroy(gameObject);
                // 以及播放死亡动画
            }
            PlayerBlinkAnime();
            blinkTimer = Time.time + playerBlinkTime;
        }
    }

    private void PlayerBlinkAnime()
    {
        StartCoroutine(DoBlinkAnimes());
    }

    IEnumerator DoBlinkAnimes()
    {
        float blinkOnceTime = 0.15f;
        int blickCount = (int)(playerBlinkTime / blinkOnceTime);
        for (int i = 0; i < blickCount * 2; i++)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(blinkOnceTime);
            if (i == (int)(blickCount * 3 / 4))
            {
                blinkOnceTime /= 2;
            }
        }
        playerRenderer.enabled = true;
    }
}
