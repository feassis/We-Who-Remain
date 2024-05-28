using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FacilityLifeBar : MonoBehaviour
{
    [SerializeField] private List<GameObject> lifeBars = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI currentHPText; 
    [SerializeField] private TextMeshProUGUI maxHPText;

    private int currentHP;
    private int maxHP;

    public void UpdateMaxHP(int amount)
    {
        maxHP = amount;
        maxHPText.text = amount.ToString();
        UpdateLifeBar();
    }

    public void UpdateCurrentHP(int amount)
    {

        currentHP = amount;
        currentHPText.text = amount.ToString();
        UpdateLifeBar();
    }

    private void UpdateLifeBar()
    {
        if(maxHP == 0)
        {
            return;
        }

        float percentage = (float) currentHP / (float)maxHP;

        int numberOfBarsOn = Mathf.CeilToInt(lifeBars.Count * percentage);

        for (int i = 0; i < lifeBars.Count; i++)
        {
            if(i < numberOfBarsOn)
            {
                lifeBars[i].SetActive(true);
            }
            else
            {
                lifeBars[i].SetActive(false);
            }
        }
    }
}
