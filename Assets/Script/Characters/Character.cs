using System;
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
    public bool IsEngineer;

    public Action Healed;
    public Action Damaged;

    public Character(string name, string bio, Sprite portrait, int maxHP, int strength, int inteligence, int dexterity, bool isEngineer)
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
}