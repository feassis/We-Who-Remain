using System;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name;
    public string Bio;
    public Sprite Portrait;
    public int CurrentHP;
    public int MaxHP;
    public int Strength;
    public int Inteligence;
    public int Dexterity;

    public bool IsOnExpedition;
    public bool IsOnHeal;
    public bool IsWaitingOnGift;
    public bool IsEngineer;

    public Action Healed;
    public Action Damaged;

    private List<CharacterDialogSetup> progressionMailChain;
    private List<CharacterDialogSetup> giftMailChain;
    private List<CharacterDialogSetup> expeditionMainChain;

    public Character(string name, string bio, Sprite portrait, int maxHP, int strength, int inteligence, int dexterity, bool isEngineer, List<CharacterDialogSetup> progressionChain, List<CharacterDialogSetup> expeditionChain, List<CharacterDialogSetup> giftChain)
    {
        Name = name;
        Bio = bio;
        Portrait = portrait;
        MaxHP = maxHP;
        CurrentHP = MaxHP;
        Strength = strength;
        Inteligence = inteligence;
        Dexterity = dexterity;
        IsEngineer = isEngineer;
        progressionMailChain = progressionChain;
        giftMailChain = giftChain;
        expeditionMainChain = expeditionChain;
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, 0, MaxHP);  
        Healed?.Invoke();
    }

    public void FullHeal()
    {
        CurrentHP = MaxHP;
        Healed?.Invoke();
    }

    public void Damage(int amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP - amount, 0, MaxHP);
        Damaged?.Invoke();
    }

    public void Kill()
    {
        CurrentHP = 0;
        Damaged?.Invoke();
    }

    public List<CharacterDialogSetup> GetProgressionMailChain() => progressionMailChain;
    public List<CharacterDialogSetup> GetGiftMailChain() => giftMailChain;
    public List<CharacterDialogSetup> GetExpeditionMailChain() => expeditionMainChain;
}