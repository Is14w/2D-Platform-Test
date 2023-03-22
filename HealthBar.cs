using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Text healthText;
    private Image healthBar;


    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    public void AlterHPValue(int playerHP, int playerMaxHP)
    {
        healthBar.fillAmount = (float)playerHP / (float)playerMaxHP;
        healthText.text = playerHP.ToString() + "/" + playerMaxHP.ToString();
    }
}
