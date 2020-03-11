using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;
    void Start()
    {
        InitHearts();
    }

    void Update()
    {
        UpdateHearts();
    }
    public void InitHearts()
    {
        for(int i = 0; i < heartContainers.RuntimeValue; i++)
        {
            if (i < hearts.Length)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].sprite = fullHeart;
            }
        }
    }

    public void UpdateHearts()
    {
        InitHearts();
        float tempHealth = playerCurrentHealth.RuntimeValue / 2;
        for (int i = 0; i < heartContainers.RuntimeValue; i++) {
            if (i <= tempHealth - 1)
            {
                //Corazon completo:
                hearts[i].sprite = fullHeart;
            }else if (i >= tempHealth) {
                
                //Corazon vacio:
                hearts[i].sprite = emptyHeart;
            }else{
                //Mitad de corazon:
                hearts[i].sprite = halfFullHeart;
                
            }
        }
    }
}
