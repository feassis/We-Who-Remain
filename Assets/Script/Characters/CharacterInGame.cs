using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInGame : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI currentHPText;
    [SerializeField] private TextMeshProUGUI maxHPText;
    [SerializeField] private TextMeshProUGUI strText;
    [SerializeField] private TextMeshProUGUI intText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private Button expeditionButtom;
    [SerializeField] private Button healButtom;
    [SerializeField] private Button giftButtom;
    
    
    private Character character;

    private void Awake()
    {
        expeditionButtom.onClick.AddListener(OnExpeditionButtomClicked);
        healButtom.onClick.AddListener(OnHealButtomClicked);
        giftButtom.onClick.AddListener(OnGiftButtomClicked);
    }

    private void OnGiftButtomClicked()
    {
        GameMaster.Instance.AddGiftToCharacter(this);
    }

    private void OnHealButtomClicked()
    {
        GameMaster.Instance.AddToHealToHeal(this);
    }

    private void OnExpeditionButtomClicked()
    {
        if (character.IsOnExpedition)
        {
            GameMaster.Instance.TryGoToExpedition(this);
        }
        else
        {
            GameMaster.Instance.CancelExpedition(this);
        }
    }

    public void Setup(Character character)
    {
        this.character = character;
        portrait.sprite = character.Portrait;
        charName.text = character.Name;
        UpdateHP();
        this.character.Healed += UpdateHP;
        this.character.Damaged += UpdateHP;

        strText.text = character.Strength.ToString();
        intText.text = character.Inteligence.ToString();
        dexText.text = character.Dexterity.ToString();

        giftButtom.gameObject.SetActive(character.IsEngineer);
    }

    public void Heal(int amount)
    {
        character.Heal(amount);
    }

    public void Damage(int amount)
    {
        character.Damage(amount);
    }

    public void Kill()
    {
        character.Kill();
    }

    public void FullyHeal()
    {
        character.FullHeal();
    }

    private void UpdateHP()
    {
        currentHPText.text = character.CurrentHP.ToString();
        maxHPText.text = character.MaxHP.ToString();
    }
}
