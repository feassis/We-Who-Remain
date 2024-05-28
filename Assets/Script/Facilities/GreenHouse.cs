using UnityEngine;

public class GreenHouse : Facility
{
    [SerializeField] protected int foodProductionBase = 3;
    [SerializeField] protected int foodProductionIncPerLevel = 1;

    public int GetFoodProduction() => foodProductionBase + foodProductionIncPerLevel*level;
}