using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    public Action<GameStage> StageChanged;

    [SerializeField] private int turnsToObjective = 100;

    [Header("Facilities")]
    [SerializeField] private Cryogeincs cryogenics;
    [SerializeField] private Dock dock;
    [SerializeField] private FusionReactor reactor;
    [SerializeField] private GreenHouse greenhouse;
    [SerializeField] private Warehouse warehouse;

    [Header("Cryogenics Graphics")]
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private TextMeshProUGUI crewText;
    [SerializeField] private FacilityLifeBar cryogenicsLifeBar;
    [SerializeField] private Button cryogenicsRepairButtom;
    [SerializeField] private TextMeshProUGUI cryogenicsRepairText;
    [SerializeField] private Button cryogenicsUpgradeButtom;
    [SerializeField] private TextMeshProUGUI cryogenicsUpgradeText;

    [Header("Docks Graphics")]
    [SerializeField] private TextMeshProUGUI shipsOnExpeditionText;
    [SerializeField] private TextMeshProUGUI shipsText;
    [SerializeField] private FacilityLifeBar docksLifeBar;
    [SerializeField] private Button docksRepairButtom;
    [SerializeField] private TextMeshProUGUI docksRepairText;
    [SerializeField] private Button docksUpgradeButtom;
    [SerializeField] private TextMeshProUGUI docksUpgradeText;

    [Header("Fusion Reactor Graphics")]
    [SerializeField] private TextMeshProUGUI energyDemandText;
    [SerializeField] private TextMeshProUGUI energyGeneratedText;
    [SerializeField] private FacilityLifeBar reactorLifeBar;
    [SerializeField] private Button reactorRepairButtom;
    [SerializeField] private TextMeshProUGUI reactorRepairText;
    [SerializeField] private Button reactorUpgradeButtom;
    [SerializeField] private TextMeshProUGUI reactorUpgradeText;

    [Header("Greenhouse Graphics")]
    [SerializeField] private TextMeshProUGUI foodGeneratedText;
    [SerializeField] private FacilityLifeBar greenhouseLifeBar;
    [SerializeField] private Button greenhouseRepairButtom;
    [SerializeField] private TextMeshProUGUI greenhouseRepairText;
    [SerializeField] private Button greenhouseUpgradeButtom;
    [SerializeField] private TextMeshProUGUI greenhouseUpgradeText;

    [Header("Warehouse Graphics")]
    [SerializeField] private TextMeshProUGUI foodAmountGeneratedText;
    [SerializeField] private TextMeshProUGUI foodMaxGeneratedText;
    [SerializeField] private TextMeshProUGUI scrapAmountGeneratedText;
    [SerializeField] private TextMeshProUGUI scrapMaxGeneratedText;
    [SerializeField] private FacilityLifeBar warehouseLifeBar;
    [SerializeField] private Button warehouseRepairButtom;
    [SerializeField] private TextMeshProUGUI warehouseRepairText;
    [SerializeField] private Button warehouseUpgradeButtom;
    [SerializeField] private TextMeshProUGUI warehouseUpgradeText;

    [Header("Gamestate Graphics")]
    [SerializeField] private TextMeshProUGUI turnCountDown;
    [SerializeField] private Button endTurnButtom;

    public Action<int> TurnAmountChanged;

    private GameStage stage = GameStage.Preparetion;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        turnCountDown.text = turnsToObjective.ToString();
        endTurnButtom.onClick.AddListener(OnEndTurnButtomClicked);

        SetupCryogenics();
        SetupDocks();
        SetupReactor();
        SetupGreenhouse();
        SetupWarehouse();
    }

    

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void OnEndTurnButtomClicked()
    {
        if(stage == GameStage.Preparetion)
        {
            ProgressToNextStage();
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.P))
        {
            ProgressToNextStage();
        }
#endif
    }

    #region Cryogenics

    private void SetupCryogenics()
    {
        cryogenics.TurnsToDefrostChanged += Cryogenics_OnTurnsToDefrosChanged;
        cooldownText.text = cryogenics.GetTurnsUntilNextDefrost().ToString();
        crewText.text = cryogenics.GetNumberOfDefrosts().ToString();
        cryogenics.Upgraded += Cryogenics_OnUpgraded;
        
        cryogenics.LifeChanged += cryogenicsLifeBar.UpdateCurrentHP;
        cryogenics.MaxLifeChanged += cryogenicsLifeBar.UpdateMaxHP;
        cryogenicsLifeBar.UpdateMaxHP(cryogenics.GetMaxLife());
        cryogenicsLifeBar.UpdateCurrentHP(cryogenics.GetLife());

        cryogenicsRepairButtom.onClick.AddListener(OnCryogenicsRepairButtomClicked);
        cryogenics.Repaired += CryogenicsRepaired;
        cryogenicsRepairText.text = cryogenics.GetRepairCostForLevel().ToString();
        cryogenicsUpgradeButtom.onClick.AddListener(OnCryigenicsUpgradeButtomClicked);
        cryogenicsUpgradeText.text = cryogenics.GetUpgradeCostForLevel().ToString();
    }

    private void OnCryigenicsUpgradeButtomClicked()
    {
        throw new NotImplementedException();
    }

    private void CryogenicsRepaired()
    {
        cryogenicsRepairText.text = cryogenics.GetRepairCostForLevel().ToString();
    }

    private void OnCryogenicsRepairButtomClicked()
    {
        throw new NotImplementedException();
    }

    private void Cryogenics_OnUpgraded()
    {
        crewText.text = cryogenics.GetNumberOfDefrosts().ToString();
        cryogenicsUpgradeText.text = cryogenics.GetUpgradeCostForLevel().ToString();
    }

    private void Cryogenics_OnTurnsToDefrosChanged(int turns)
    {
        cooldownText.text = turns.ToString();
    }

    #endregion

    #region Docks

    private void SetupDocks()
    {
        shipsOnExpeditionText.text = dock.GetShipsOnExpedition().ToString();
        dock.shipsOnExpeditionChanged += Docks_OnShipsOnExpeditionChanged;
        shipsText.text = dock.GetShipsAvailablePerLevel().ToString();
        dock.Upgraded += Docks_Upgraded;
        docksUpgradeText.text = dock.GetUpgradeCostForLevel().ToString();
        dock.Repaired += Docks_Repaired;
        docksRepairText.text = dock.GetRepairCostForLevel().ToString();

        dock.LifeChanged += docksLifeBar.UpdateCurrentHP;
        dock.MaxLifeChanged += docksLifeBar.UpdateMaxHP;
        docksLifeBar.UpdateMaxHP(dock.GetMaxLife());
        docksLifeBar.UpdateCurrentHP(dock.GetLife());

        docksRepairButtom.onClick.AddListener(OnDocksRepairButtomClick);
        docksUpgradeButtom.onClick.AddListener(OnDocksUpgradeButtomClick);
    }

    private void OnDocksRepairButtomClick()
    {
        throw new NotImplementedException();
    }

    private void OnDocksUpgradeButtomClick()
    {
        throw new NotImplementedException();
    }

    private void Docks_Repaired()
    {
        docksRepairText.text = dock.GetRepairCostForLevel().ToString();
    }

    private void Docks_Upgraded()
    {
        shipsText.text = dock.GetShipsAvailablePerLevel().ToString();
        docksUpgradeText.text = dock.GetUpgradeCostForLevel().ToString();
    }

    private void Docks_OnShipsOnExpeditionChanged(int shipsOnExpedition)
    {
        shipsOnExpeditionText.text += shipsOnExpedition.ToString();
    }

    #endregion

    #region Reactor

    private void SetupReactor()
    {
        energyDemandText.text = GetTotalEnergyDemand().ToString();
        Facility.AnyFacilityUpgraded += Facility_AnyFacilityUpgraded;
        energyGeneratedText.text = reactor.GetGeneratedEnergy().ToString();
        reactorUpgradeText.text = reactor.GetUpgradeCostForLevel().ToString();
        reactor.Upgraded += Reactor_OnUpgraded;

        reactor.LifeChanged += reactorLifeBar.UpdateCurrentHP;
        reactor.MaxLifeChanged += reactorLifeBar.UpdateMaxHP;
        reactorLifeBar.UpdateMaxHP(reactor.GetMaxLife());
        reactorLifeBar.UpdateCurrentHP(reactor.GetLife());

        reactorRepairText.text = reactor.GetRepairCostForLevel().ToString();
        reactor.Repaired += Reactor_OnRepaired;

        reactorRepairButtom.onClick.AddListener(OnReactorRepairButtomClick);
        reactorUpgradeButtom.onClick.AddListener(OnReactorUpgradeButtomClick);
    }

    private void Reactor_OnRepaired()
    {
        reactorRepairText.text = reactor.GetRepairCostForLevel().ToString();
    }

    private void OnReactorUpgradeButtomClick()
    {
        throw new NotImplementedException();
    }

    private void OnReactorRepairButtomClick()
    {
        throw new NotImplementedException();
    }

    private void Reactor_OnUpgraded()
    {
        energyGeneratedText.text += reactor.GetGeneratedEnergy().ToString();
        reactorUpgradeText.text += reactor.GetUpgradeCostForLevel();
    }

    #endregion

    #region Greenhouse
    private void SetupGreenhouse()
    {
        foodGeneratedText.text = greenhouse.GetFoodProduction().ToString();
        greenhouseUpgradeText.text = greenhouse.GetUpgradeCostForLevel().ToString();
        greenhouse.Upgraded += Greenhouse_OnUpgraded;

        greenhouseRepairText.text = greenhouse.GetRepairCostForLevel().ToString();
        greenhouseRepairButtom.onClick.AddListener(GreenhouseRepairButtom);
        greenhouse.Repaired += GreenHouse_OnRepair;

        greenhouse.LifeChanged += greenhouseLifeBar.UpdateCurrentHP;
        greenhouse.MaxLifeChanged += greenhouseLifeBar.UpdateMaxHP;
        greenhouseLifeBar.UpdateMaxHP(greenhouse.GetMaxLife());
        greenhouseLifeBar.UpdateCurrentHP(greenhouse.GetLife());
    }

    private void GreenHouse_OnRepair()
    {
        greenhouseRepairText.text = greenhouse.GetRepairCostForLevel().ToString();
    }

    private void GreenhouseRepairButtom()
    {
        throw new NotImplementedException();
    }

    private void Greenhouse_OnUpgraded()
    {
        foodGeneratedText.text = greenhouse.GetFoodProduction().ToString();
        greenhouseUpgradeText.text = greenhouse.GetUpgradeCostForLevel().ToString();
    }

    #endregion

    #region Warehouse

    private void SetupWarehouse()
    {
        foodAmountGeneratedText.text = warehouse.GetCurrentFood().ToString();
        foodMaxGeneratedText.text = warehouse.GetMaxFood().ToString();
        warehouse.FoodChanged += Warehouse_OnFoodChanged;

        scrapAmountGeneratedText.text = warehouse.GetCurrentScrap().ToString();
        scrapMaxGeneratedText.text = warehouse.GetMaxScrap().ToString();
        warehouse.ScrapChanged += Warehouse_OnScrapChanged;

        warehouseUpgradeText.text = warehouse.GetUpgradeCostForLevel().ToString();
        warehouse.Upgraded += Warehouse_OnUpgraded;

        warehouseRepairText.text = warehouse.GetRepairCostForLevel().ToString();
        warehouse.Repaired += Warehouse_OnRepaired;

        warehouse.LifeChanged += warehouseLifeBar.UpdateCurrentHP;
        warehouse.MaxLifeChanged += warehouseLifeBar.UpdateMaxHP;
        warehouseLifeBar.UpdateMaxHP(warehouse.GetMaxLife());
        warehouseLifeBar.UpdateCurrentHP(warehouse.GetLife());
    }

    private void Warehouse_OnRepaired()
    {
        warehouseRepairText.text = warehouse.GetRepairCostForLevel().ToString();
    }

    private void Warehouse_OnUpgraded()
    {
        foodMaxGeneratedText.text = warehouse.GetMaxFood().ToString();
        scrapMaxGeneratedText.text = warehouse.GetMaxScrap().ToString();
        warehouseUpgradeText.text = warehouse.GetUpgradeCostForLevel().ToString();
    }

    private void Warehouse_OnScrapChanged(int scrapAmount)
    {
        scrapAmountGeneratedText.text = scrapAmount.ToString();
    }

    private void Warehouse_OnFoodChanged(int foodAmount)
    {
        foodAmountGeneratedText.text = foodAmount.ToString();
    }

    #endregion

    public int GetTotalEnergyDemand()
    {
        int energyRequired = 0;

        energyRequired += cryogenics.GetEnergyDemandForLevel();
        energyRequired += dock.GetEnergyDemandForLevel();
        energyRequired += reactor.GetEnergyDemandForLevel();
        energyRequired += greenhouse.GetEnergyDemandForLevel();
        energyRequired += warehouse.GetEnergyDemandForLevel();

        return energyRequired;
    }

    private void Facility_AnyFacilityUpgraded()
    {
        energyDemandText.text = GetTotalEnergyDemand().ToString();
    }

    private void ChangeState(GameStage stageToGo)
    {
        stage = stageToGo;

        StageChanged?.Invoke(stage);
    }

    public void ProgressToNextStage()
    {
        switch (stage)
        {
            case GameStage.Preparetion:
                ChangeState(GameStage.Expedition);
                GenerateResources();
                ConsumeResources();
                break;
            case GameStage.Expedition:
                ChangeState(GameStage.Event);
                break;
            case GameStage.Event:
                ChangeState(GameStage.Preparetion);
                AdvanceTurns(1);
                break;
                
        }
    }

    public void GenerateResources()
    {
        warehouse.AddFood(greenhouse.GetFoodProduction());
    }

    public void ConsumeResources()
    {
        //consume food for characters
    }

    public void AdvanceTurns(int amount)
    {
        turnsToObjective -= amount;
        TurnAmountChanged?.Invoke(turnsToObjective);
        turnCountDown.text = turnsToObjective.ToString();
    }
}

public enum GameStage
{
    Preparetion = 0,
    Expedition = 1,
    Event = 2,
}