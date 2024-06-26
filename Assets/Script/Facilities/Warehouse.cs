﻿using System;
using UnityEngine;

public class Warehouse : Facility
{
    [SerializeField] protected int initialScrap = 20;
    [SerializeField] protected int maxScrapBase = 30;
    [SerializeField] protected int maxScrapIncPerLevel = 5;
    [SerializeField] protected int initialFood = 6;
    [SerializeField] protected int maxFoodBase = 12;
    [SerializeField] protected int maxFoodIncPerLevel = 2;


    [SerializeField] private RectTransform whereToAddFood;
    [SerializeField] private RectTransform whereToAddScrap;
    [SerializeField] private ResourceChange resourceChangePrefab;

    public Action<int> FoodChanged;
    public Action<int> ScrapChanged;

    protected int currentScrap;
    protected int maxScrap;

    protected int currentFood;
    protected int maxFood;

    protected int GetMaxFoodPerLevel() => maxFoodBase + maxFoodIncPerLevel * level;
    protected int GetMaxScrapPerLevel() => maxScrapBase + maxScrapIncPerLevel * level;


    protected override void Initialize()
    {
        base.Initialize();

        currentFood = initialFood;
        maxFood = GetMaxFoodPerLevel();

        currentScrap = initialScrap;
        maxScrap = GetMaxScrapPerLevel();
    }

    public int GetCurrentFood() => currentFood;

    public int GetCurrentScrap() => currentScrap; 

    public int GetMaxFood() => maxFood;

    public int GetMaxScrap() => maxScrap;

    private void InstantiateResourceChange(int amount, bool isFood)
    {
        if(isFood)
        {
            var resourceChange = Instantiate<ResourceChange>(resourceChangePrefab, whereToAddFood);
            resourceChange.Setup(amount);
        }
        else
        {
            var resourceChange = Instantiate<ResourceChange>(resourceChangePrefab, whereToAddScrap);
            resourceChange.Setup(amount);
        }
    }

    public void AddFood(int amount)
    {
        currentFood = Math.Clamp(currentFood + amount, 0, maxFood);
        FoodChanged?.Invoke(currentFood);
        InstantiateResourceChange(amount, true);
    }

    public void AddScrap(int amount) 
    {  
        currentScrap = Math.Clamp(currentScrap + amount, 0, maxScrap);
        ScrapChanged?.Invoke(currentScrap);
        InstantiateResourceChange(amount, false);
    }

    public void RemoveFood(int amount) 
    {
        currentFood = Math.Clamp(currentFood - amount, 0, maxFood);
        FoodChanged?.Invoke(currentFood);
        InstantiateResourceChange(-amount, true);
    }

    public void RemoveScrap(int amount)
    {
        currentScrap = Math.Clamp(currentScrap - amount, 0, maxScrap);
        ScrapChanged?.Invoke(currentScrap);
        InstantiateResourceChange(-amount, false);
    }

    public override void Upgrade()
    {
        level++;
        maxFood = GetMaxFoodPerLevel();
        maxScrap = GetMaxScrapPerLevel();
        Upgraded?.Invoke();
        AnyFacilityUpgraded?.Invoke();
    }
}