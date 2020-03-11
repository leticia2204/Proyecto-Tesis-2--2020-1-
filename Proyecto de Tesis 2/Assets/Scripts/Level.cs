using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int exp;
    public int level;
    public string rango; 

    void Update()
    {
        UpdateExp(5);
    }

    public void UpdateExp(int newExp)
    {
        exp += newExp;
        int nivelActual = (int)(0.1f + Mathf.Sqrt(exp));

        if(nivelActual != level)
        {
            level = nivelActual;
        }
        int expNextLevel = 100 * (level + 1) * (level + 1);
        int difExp = expNextLevel - exp;
        int totalDif = expNextLevel - (100 * level * level);
    }
}
