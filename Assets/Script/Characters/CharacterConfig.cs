using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New Character Config", menuName = "Configs/Character")]
public class CharacterConfig : ScriptableObject
{
    [Header("Character Description")]
    [SerializeField] private string charName;
    [SerializeField] private string bio;
    [ShowAssetPreview][SerializeField] private Sprite portrait;
    [SerializeField] private int hp;
    [SerializeField] private int strength;
    [SerializeField] private int inteligence;
    [SerializeField] private int dexterity;
    [SerializeField] private bool IsEngineer;

    public Character GetCharacterObject() => new Character(charName, bio, portrait, hp, strength, inteligence, dexterity, IsEngineer);
}
