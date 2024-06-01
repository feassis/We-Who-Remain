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
    [SerializeField] private Image expeditionButtomImage;
    [SerializeField] private Button healButtom;
    [SerializeField] private Image healButtomImage;
    [SerializeField] private Button giftButtom;
    [SerializeField] private Image giftButtomImage;


    private Character character;
    private int giftsRecived = 0;
    private int expeditionsWent = 0;
    private int progressionCounter = 0;

    private bool blockedForExpedition = true;

    public Character GetCharacter() => character;

    public bool CanGoToExpedition() => blockedForExpedition;
    public void ResetCanGoToExpedition() => blockedForExpedition = true;

    private void Awake()
    {
        expeditionButtom.onClick.AddListener(OnExpeditionButtomClicked);
        healButtom.onClick.AddListener(OnHealButtomClicked);
        giftButtom.onClick.AddListener(OnGiftButtomClicked);
    }

    private void OnGiftButtomClicked()
    {
        if (!character.IsWaitingOnGift)
        {
            GameMaster.Instance.AddGiftToCharacter(this);
        }
        else
        {
            GameMaster.Instance.CancelGiftToCharacter(this);
        }
    }

    private void OnHealButtomClicked()
    {
        if (!character.IsOnHeal)
        {
            GameMaster.Instance.AddToHeal(this);
        }
        else
        {
            GameMaster.Instance.CancelHeal(this);
        }
    }

    private void OnExpeditionButtomClicked()
    {
        if(!CanGoToExpedition())
        {
            return;
        }

        if (!character.IsOnExpedition)
        {
            GameMaster.Instance.TryGoToExpedition(this);
        }
        else
        {
            GameMaster.Instance.CancelOrFinishExpedition(this);
        }
    }

    public void SetIsOnExpedition(bool isOnExpedition)
    {
        character.IsOnExpedition = isOnExpedition;

        if (isOnExpedition)
        {
            expeditionButtomImage.color= new Color(expeditionButtomImage.color.r, expeditionButtomImage.color.g, expeditionButtomImage.color.b, 0.5f);
        }
        else
        {
            expeditionButtomImage.color = new Color(expeditionButtomImage.color.r, expeditionButtomImage.color.g, expeditionButtomImage.color.b, 1f);
        }
    }

    public void SetIsOnHeal(bool isOnHeal)
    {
        character.IsOnHeal = isOnHeal;

        if (isOnHeal)
        {
            healButtomImage.color = new Color(healButtomImage.color.r, healButtomImage.color.g, healButtomImage.color.b, 0.5f);
        }
        else
        {
            healButtomImage.color = new Color(healButtomImage.color.r, healButtomImage.color.g, healButtomImage.color.b, 1f);
        }
    }

    public void SetIsWaitingOnGift(bool isWaitingOnGift)
    {
        character.IsWaitingOnGift = isWaitingOnGift;

        if (isWaitingOnGift)
        {
            giftButtomImage.color = new Color(giftButtomImage.color.r, giftButtomImage.color.g, giftButtomImage.color.b, 0.5f);
        }
        else
        {
            giftButtomImage.color = new Color(giftButtomImage.color.r, giftButtomImage.color.g, giftButtomImage.color.b, 1f);
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

        if (character.IsEngineer)
        {
            giftButtom.gameObject.SetActive(character.IsEngineer);
            Facility.AnyFacilityReapired += Facility_OnFacilityAnyRepaired;
            Facility.AnyFacilityUpgraded += Facility_OnFacilityAnyUpgraded;
        }
    }

    private void Facility_OnFacilityAnyUpgraded()
    {
        blockedForExpedition = false;
    }

    private void Facility_OnFacilityAnyRepaired()
    {
        blockedForExpedition = false;
    }

    public void Heal(int amount)
    {
        character.Heal(amount);
    }

    public void Damage(int amount)
    {
        character.Damage(amount);

        if (character.CurrentHP <= 0)
        {
            GameMaster.Instance.KillCharacter(this);
        }
    }

    public void Kill()
    {
        character.Kill();
    }

    public void FullyHeal()
    {
        character.FullHeal();
    }

    public void Gift()
    {
        giftsRecived++;

        if (character.GetGiftMailChain() == null || character.GetGiftMailChain().Count == 0)
        {
            return;
        }

        foreach (var mailNode in character.GetGiftMailChain())
        {
            if (giftsRecived == mailNode.TriggerTrashold)
            {
                GameMaster.Instance.AddMail(mailNode.Dialog, this);
            }
        }
    }

    public void ProgressTurn()
    {
        progressionCounter++;
        ResetCanGoToExpedition();

        if(character.GetProgressionMailChain() == null || character.GetProgressionMailChain().Count == 0)
        {
            return;
        }

        foreach (var mailNode in character.GetProgressionMailChain())
        {
            if(progressionCounter == mailNode.TriggerTrashold)
            {
                GameMaster.Instance.AddMail(mailNode.Dialog, this);
            }
        }
    }

    public void WentOnExpedition()
    {
        expeditionsWent++;

        if (character.GetExpeditionMailChain() == null || character.GetExpeditionMailChain().Count == 0)
        {
            return;
        }

        foreach (var mailNode in character.GetExpeditionMailChain())
        {
            if (expeditionsWent == mailNode.TriggerTrashold)
            {
                GameMaster.Instance.AddMail(mailNode.Dialog, this);
            }
        }
    }

    private void UpdateHP()
    {
        currentHPText.text = character.CurrentHP.ToString();
        maxHPText.text = character.MaxHP.ToString();
    }
}
