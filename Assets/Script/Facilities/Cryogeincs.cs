using System;
using System.Collections.Generic;
using UnityEngine;

public class Cryogeincs : Facility
{
    [SerializeField] private int turnsToDefrostBase = 15;
    [SerializeField] private int turnsToDefrostDecPerLevel = 1;

    [SerializeField] private int numberOfDefrosts = 1;
    [SerializeField] private int numberOfDefrostsIncEachTwoLevels = 1;

    [SerializeField] private List<CharacterConfig> characterConfigs = new List<CharacterConfig>();

    private List<Character> charactesToOffer = new List<Character>();

    public Action<int> TurnsToDefrostChanged;

    private int turnsUntilNextDefrost;
    public int GetTurnsUntilNextDefrost() => turnsUntilNextDefrost;

    private int listIndex = 0;

    public int GetTurnsToDefrost() => Mathf.Clamp( turnsToDefrostBase - turnsToDefrostDecPerLevel * level , 0, int.MaxValue);

    public int GetNumberOfDefrosts() => numberOfDefrosts + numberOfDefrostsIncEachTwoLevels * (level / 2);

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GameMaster.Instance.TurnAmountChanged += GameMaster_OnTurnAmountChanged;
    }

    private void GameMaster_OnTurnAmountChanged(int turns)
    {
        if(life <= 0)
        {
            return;
        }

        turnsUntilNextDefrost--;


        if(turnsUntilNextDefrost == 0)
        {
            //add characters

            turnsUntilNextDefrost = GetTurnsToDefrost();
        }

        TurnsToDefrostChanged?.Invoke(turnsUntilNextDefrost);
    }

    protected override void Initialize()
    {
        base.Initialize();

        turnsUntilNextDefrost = GetTurnsToDefrost();

        var charList = new List<Character>();

        foreach (var config in characterConfigs)
        {
            charList.Add(config.GetCharacterObject());
        }

        charactesToOffer = ListRandomizer.ShuffleList(charList);
    }

    public Character GenerateCrewMenber()
    {
        var character = charactesToOffer[listIndex % charactesToOffer.Count];
        listIndex++;
        return character;
    }
}
