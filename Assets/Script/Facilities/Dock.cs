using System;
using System.Collections.Generic;
using UnityEngine;

public class Dock : Facility
{
    [SerializeField] protected int shipsAvailableBase = 1;
    [SerializeField] protected int shipsAvailableIncPerlevel = 1;

    public Action<int> shipsOnExpeditionChanged;

    private List<Character> charactersOnExpedition = new List<Character>();

    private int shipsOnExpedition = 0;
    private int numberOfShips = 0;
    
    public int GetShipsOnExpedition() { return shipsOnExpedition;}

    public int GetShipsAvailablePerLevel() => shipsAvailableBase + shipsAvailableIncPerlevel * level;

    public bool CanSendToExpediditon() => shipsOnExpedition < numberOfShips;

    public void SendToExpedition(Character character)
    {
        charactersOnExpedition.Add(character);
        shipsOnExpedition++;
        shipsOnExpeditionChanged?.Invoke(shipsOnExpedition);
    }

    public void CancelOrFinishExpedition(Character character)
    {
        charactersOnExpedition.Remove(character);
        shipsOnExpedition--;
        shipsOnExpeditionChanged?.Invoke(shipsOnExpedition);
    }

    public List<Character> GetCharactersOnExpedition() => charactersOnExpedition;

    protected override void Initialize()
    {
        base.Initialize();

        numberOfShips = GetShipsAvailablePerLevel();
    }

    public override void Upgrade()
    {
        level++;
        numberOfShips = GetShipsAvailablePerLevel();
        Upgraded?.Invoke();
        AnyFacilityUpgraded?.Invoke();
    }
}
