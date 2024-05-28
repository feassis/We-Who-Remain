using System;
using UnityEngine;

public class Cryogeincs : Facility
{
    [SerializeField] private int turnsToDefrostBase = 15;
    [SerializeField] private int turnsToDefrostDecPerLevel = 1;

    [SerializeField] private int numberOfDefrosts = 1;
    [SerializeField] private int numberOfDefrostsIncEachTwoLevels = 1;

    public Action<int> TurnsToDefrostChanged;

    private int turnsUntilNextDefrost;
    public int GetTurnsUntilNextDefrost() => turnsUntilNextDefrost;


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

        //setup characters
    }
}