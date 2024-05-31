using System;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Facility : MonoBehaviour
{
    [SerializeField] protected int baseLife = 5;
    [SerializeField] protected int lifeIncPerlevel = 1;
    [SerializeField] protected int repairBaseCost = 5;
    [SerializeField] protected int repairIncPerLevel = 0;
    [SerializeField] protected int repairDestroyedCost = 20;
    [SerializeField] protected int repairDestroyedIncPerLEvel = 3;
    [SerializeField] protected int energyDemandBase = 1;
    [SerializeField] protected int energyDemandIntPerLevel = 1;
    [SerializeField] protected int upgradeCostBase = 15;
    [SerializeField] protected int upgradeCostIncPerLevel = 1;

    public Action<int> LifeChanged;
    public Action<int> MaxLifeChanged;
    public Action Upgraded;
    public static Action AnyFacilityUpgraded;
    public Action Repaired;

    protected int level = 0;
    protected int maxLife;
    protected int life;

    protected virtual void Awake()
    {
        Initialize();
    }

    protected int GetMaxLifeForLevel() => baseLife + lifeIncPerlevel * level;

    public int GetRepairCostForLevel()
    {
        if(life <= 0)
        {
            return repairDestroyedCost + repairDestroyedIncPerLEvel * level;
        }

        return repairBaseCost + repairIncPerLevel * level;
    }

    public int GetEnergyDemandForLevel() => energyDemandBase + energyDemandIntPerLevel * level;

    public int GetUpgradeCostForLevel() => upgradeCostBase + upgradeCostIncPerLevel * level;

    public int GetLife() => life;
    public int GetMaxLife() => maxLife;

    protected void SetLife(int amount)
    {
        life = amount;
        LifeChanged?.Invoke(life);
    }

    protected void SetMaxLife(int amount)
    {
        maxLife = amount;
        MaxLifeChanged?.Invoke(maxLife);
    }

    public void Damage(int amount)
    {
        SetLife(Mathf.Clamp(life - amount, 0, maxLife));
    }

    protected virtual void Initialize()
    {
        maxLife = GetMaxLifeForLevel();
        life = maxLife;

    }

    public void Repair()
    {
        life = maxLife;
        Repaired?.Invoke();
    }

    public virtual void Upgrade()
    {
        level++;
        Upgraded?.Invoke();
        AnyFacilityUpgraded?.Invoke();
    }
}
