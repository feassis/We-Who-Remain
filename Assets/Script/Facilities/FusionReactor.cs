using UnityEngine;

public class FusionReactor : Facility
{
    [SerializeField] protected int energyGeneratadeBase = 6;
    [SerializeField] protected int energyGeneratedIncPerLevel = 1;

    public int GetGeneratedEnergy() => life > 0 ? energyGeneratadeBase + energyGeneratedIncPerLevel * level : 0;
}