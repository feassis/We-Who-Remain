using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    public Action<GameStage> StageChanged;

    [Header("Game Settings")]
    [SerializeField] private int turnsToObjective = 100;
    [SerializeField] private CharacterConfig engineerConfig;
    [SerializeField] private int initialCrewNumber = 1;
    [SerializeField] private CharacterInGame characterInGamePrefab;
    [SerializeField] private CharacterInGame engineerCharacter;
    [SerializeField] private RectTransform charListHolder;
    [SerializeField] private int maxNumberOfChoices = 3;
    [SerializeField] private int passiveFoodCost = 1;
    [SerializeField] private int expeditionFoodCost = 2;
    [SerializeField] private int healFoodCost = 5;
    [SerializeField] private int giftFoodCost = 3;
    [SerializeField] private int damageForLackOFood = 1;
    [SerializeField] private int damageForLackOfEnergy = 1;
    [SerializeField] private int passiveDamage = 1;


    [Header("Expeditions And Events")]
    [SerializeField][Range(0f, 1f)] private float expeditionFailChance = 0.25f;
    [SerializeField] private int minExpeditionScrapReward = 1;
    [SerializeField] private int maxExpeditionScrapReward = 6;
    [SerializeField] private List<DialogConfig> eventPool = new List<DialogConfig>();

    [Header("Email")]
    [SerializeField] private MailBox mailBox;
    [SerializeField] private Button openCloseMailButtom;
    [SerializeField] private GameObject newEmailGameObject;

    [Header("Gamestate Graphics")]
    [SerializeField] private TextMeshProUGUI turnCountDown;
    [SerializeField] private Button endTurnButtom;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private List<Button> choiceButtoms;
    [SerializeField] private TextMeshProUGUI compromizedFoodText;
    [SerializeField] private TextMeshProUGUI endTurnText;

    [Header("For Testing Purpose")]
    [SerializeField] private DialogConfig testingDialog;


    [Header("Facilities")]
    [SerializeField] private Cryogeincs cryogenics;
    [SerializeField] private Dock dock;
    [SerializeField] private FusionReactor reactor;
    [SerializeField] private GreenHouse greenhouse;
    [SerializeField] private Warehouse warehouse;

    [Header("Cryogenics Graphics")]
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private TextMeshProUGUI crewText;
    [SerializeField] private LifeBar cryogenicsLifeBar;
    [SerializeField] private Button cryogenicsRepairButtom;
    [SerializeField] private TextMeshProUGUI cryogenicsRepairText;
    [SerializeField] private Button cryogenicsUpgradeButtom;
    [SerializeField] private TextMeshProUGUI cryogenicsUpgradeText;

    [Header("Docks Graphics")]
    [SerializeField] private TextMeshProUGUI shipsOnExpeditionText;
    [SerializeField] private TextMeshProUGUI shipsText;
    [SerializeField] private LifeBar docksLifeBar;
    [SerializeField] private Button docksRepairButtom;
    [SerializeField] private TextMeshProUGUI docksRepairText;
    [SerializeField] private Button docksUpgradeButtom;
    [SerializeField] private TextMeshProUGUI docksUpgradeText;

    [Header("Fusion Reactor Graphics")]
    [SerializeField] private TextMeshProUGUI energyDemandText;
    [SerializeField] private TextMeshProUGUI energyGeneratedText;
    [SerializeField] private LifeBar reactorLifeBar;
    [SerializeField] private Button reactorRepairButtom;
    [SerializeField] private TextMeshProUGUI reactorRepairText;
    [SerializeField] private Button reactorUpgradeButtom;
    [SerializeField] private TextMeshProUGUI reactorUpgradeText;

    [Header("Greenhouse Graphics")]
    [SerializeField] private TextMeshProUGUI foodGeneratedText;
    [SerializeField] private LifeBar greenhouseLifeBar;
    [SerializeField] private Button greenhouseRepairButtom;
    [SerializeField] private TextMeshProUGUI greenhouseRepairText;
    [SerializeField] private Button greenhouseUpgradeButtom;
    [SerializeField] private TextMeshProUGUI greenhouseUpgradeText;

    [Header("Warehouse Graphics")]
    [SerializeField] private TextMeshProUGUI foodAmountGeneratedText;
    [SerializeField] private TextMeshProUGUI foodMaxGeneratedText;
    [SerializeField] private TextMeshProUGUI scrapAmountGeneratedText;
    [SerializeField] private TextMeshProUGUI scrapMaxGeneratedText;
    [SerializeField] private LifeBar warehouseLifeBar;
    [SerializeField] private Button warehouseRepairButtom;
    [SerializeField] private TextMeshProUGUI warehouseRepairText;
    [SerializeField] private Button warehouseUpgradeButtom;
    [SerializeField] private TextMeshProUGUI warehouseUpgradeText;

    

    public Action<int> TurnAmountChanged;
    private List<CharacterInGame> charactersInGame = new List<CharacterInGame>();
    

    private GameStage stage = GameStage.Preparetion;
    private DialogConfig loadedDialog;
    private CharacterInGame dialogCharacter;
    private int compromisedFood;
    private List<CharacterInGame> chatactersToGoOnExpedition = new List<CharacterInGame>();
    private List<CharacterInGame> chatactersToHeal = new List<CharacterInGame>();
    private List<CharacterInGame> chatactersToGift = new List<CharacterInGame>();

    private List<Mail> mail = new List<Mail>();

    private CharacterInGame eventCharacter;
    private bool isEmailOpen = false;

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
        openCloseMailButtom.onClick.AddListener(OnOpenCloseEmailClicked);

        SetupCryogenics();
        SetupDocks();
        SetupReactor();
        SetupGreenhouse();
        SetupWarehouse();

        InitializeGame();
    }

    private void OnOpenCloseEmailClicked()
    {
        if(stage != GameStage.Preparetion)
        {
            return;
        }

        if(isEmailOpen)
        {
            CloseEmail();
        }
        else
        {
            OpenEmails();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitializeGame()
    {
        for (int i = 0; i < initialCrewNumber; i++)
        {
            var crewMember = cryogenics.GenerateCrewMenber();
            var inGame = Instantiate<CharacterInGame>(characterInGamePrefab, charListHolder);
            inGame.Setup(crewMember);
            charactersInGame.Add(inGame);
        }

        engineerCharacter?.Setup(engineerConfig.GetCharacterObject());
        Facility.AnyFacilityDestroyed += Facility_OnAnyFacilityDestroyed;

        SetupChoiceButtons();
    }

    private void Facility_OnAnyFacilityDestroyed()
    {
        ChackIfLost();
    }

    private void ChackIfLost()
    {
        if(cryogenics.GetLife() == 0 && dock.GetLife() == 0 && greenhouse.GetLife() == 0 
            && reactor.GetLife() == 0 && warehouse.GetLife() == 0)
        {
            EndGame(EndGameMode.Lost);
        }
    }

    private void EndGame(EndGameMode mode)
    {
        EndGameScene.OpenEndGameScene(mode);
    }

    private void SetupChoiceButtons()
    {
        choiceButtoms[0].onClick.AddListener(OnFirstChoiceButtonClicked);
        choiceButtoms[1].onClick.AddListener(OnSecondChoiceButtonClicked);
        choiceButtoms[2].onClick.AddListener(OnThirdChoiceButtonClicked);
    }

    private void OnThirdChoiceButtonClicked()
    {
        LoadChoice(2);
    }

    private void OnSecondChoiceButtonClicked()
    {
        LoadChoice(1);
    }

    private void OnFirstChoiceButtonClicked()
    {
        LoadChoice(0);
    }

    private void LoadChoice(int index)
    {
        var choices = loadedDialog.GetChoices();

        LoadDialog(choices[index]);
    }

    private void OnEndTurnButtomClicked()
    {
        if (stage == GameStage.Event)
        {
            return;
        }

        ProgressToNextStage();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.P))
        {
            ProgressToNextStage();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadDialog(testingDialog, engineerCharacter);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            engineerCharacter.Damage(1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            charactersInGame[0].Damage(1);
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
        TryToUpgradeFacility(cryogenics);
    }

    private void CryogenicsRepaired()
    {
        cryogenicsRepairText.text = cryogenics.GetRepairCostForLevel().ToString();
    }

    private void OnCryogenicsRepairButtomClicked()
    {
        TryToRepairFacility(cryogenics);
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
        TryToRepairFacility(dock);
    }

    private void OnDocksUpgradeButtomClick()
    {
        TryToUpgradeFacility(dock);
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
        shipsOnExpeditionText.text = shipsOnExpedition.ToString();
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
        TryToUpgradeFacility(reactor);
    }

    private void OnReactorRepairButtomClick()
    {
        TryToRepairFacility(reactor);
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
        greenhouseUpgradeButtom.onClick.AddListener(GreenHouseUpgradeButton);

        greenhouseRepairText.text = greenhouse.GetRepairCostForLevel().ToString();
        greenhouseRepairButtom.onClick.AddListener(GreenhouseRepairButtom);
        greenhouse.Repaired += GreenHouse_OnRepair;

        greenhouse.LifeChanged += greenhouseLifeBar.UpdateCurrentHP;
        greenhouse.MaxLifeChanged += greenhouseLifeBar.UpdateMaxHP;
        greenhouseLifeBar.UpdateMaxHP(greenhouse.GetMaxLife());
        greenhouseLifeBar.UpdateCurrentHP(greenhouse.GetLife());
    }

    private void GreenHouseUpgradeButton()
    {
        TryToUpgradeFacility(greenhouse);
    }

    private void GreenHouse_OnRepair()
    {
        greenhouseRepairText.text = greenhouse.GetRepairCostForLevel().ToString();
    }

    private void GreenhouseRepairButtom()
    {
        TryToRepairFacility(greenhouse);
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

        warehouseRepairButtom.onClick.AddListener(WarehouseRepairButton);
        warehouseUpgradeButtom.onClick.AddListener(WarehouseUpgradeButton);
    }

    private void WarehouseUpgradeButton()
    {
        TryToUpgradeFacility(warehouse);
    }

    private void WarehouseRepairButton()
    {
        TryToRepairFacility(warehouse);
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

    #region CharacterActions
    public void TryGoToExpedition(CharacterInGame characterInGame)
    {
        if (compromisedFood + expeditionFoodCost > warehouse.GetCurrentFood() || characterInGame.GetCharacter().IsOnHeal || characterInGame.GetCharacter().IsWaitingOnGift)
        {
            return;
        }

        if (!dock.CanSendToExpediditon())
        {
            return;
        }

        dock.SendToExpedition(characterInGame.GetCharacter());

        AddCompromisedFood(expeditionFoodCost);

        characterInGame.SetIsOnExpedition(true);
        chatactersToGoOnExpedition.Add(characterInGame);
    }

    public void CancelOrFinishExpedition(CharacterInGame characterInGame)
    {
        if (!chatactersToGoOnExpedition.Contains(characterInGame))
        {
            return;
        }

        dock.CancelOrFinishExpedition(characterInGame.GetCharacter());

        RemoveCompromisedFood(expeditionFoodCost);
        characterInGame.SetIsOnExpedition(false);
        chatactersToGoOnExpedition.Remove(characterInGame);
    }

    public void AddToHeal(CharacterInGame characterInGame)
    {
        if (compromisedFood + healFoodCost > warehouse.GetCurrentFood() || characterInGame.GetCharacter().IsOnExpedition || characterInGame.GetCharacter().IsWaitingOnGift)
        {
            return;
        }

        AddCompromisedFood(healFoodCost);
        characterInGame.SetIsOnHeal(true);
        chatactersToHeal.Add(characterInGame);
    }

    public void CancelHeal(CharacterInGame characterInGame)
    {
        if (!chatactersToHeal.Contains(characterInGame))
        {
            return;
        }

        RemoveCompromisedFood(healFoodCost);
        characterInGame.SetIsOnHeal(false);
        charactersInGame.Remove(characterInGame);
    }

    public void AddGiftToCharacter(CharacterInGame characterInGame)
    {
        if(compromisedFood + giftFoodCost > warehouse.GetCurrentFood() || characterInGame.GetCharacter().IsOnExpedition || characterInGame.GetCharacter().IsOnHeal)
        {
            return;
        }

        AddCompromisedFood(giftFoodCost);
        characterInGame.SetIsWaitingOnGift(true);
        chatactersToGift.Add(characterInGame);
    }

    public void CancelGiftToCharacter(CharacterInGame characterInGame)
    {
        if(!chatactersToGift.Contains(characterInGame))
        {
            return;
        }

        RemoveCompromisedFood(giftFoodCost);
        characterInGame.SetIsWaitingOnGift(false);
        chatactersToGift.Remove(characterInGame);
    }

    private void AddCompromisedFood(int amount)
    {
        compromisedFood += amount;
        compromizedFoodText.text = compromisedFood.ToString();
    }

    private void RemoveCompromisedFood(int amount)
    {
        compromisedFood -= amount;
        compromizedFoodText.text = compromisedFood.ToString();
    }

    #endregion

    
    private void TryToUpgradeFacility(Facility facility)
    {
        if (facility.GetUpgradeCostForLevel() > warehouse.GetCurrentScrap())
        {
            return;
        }

        LoseScrap(facility.GetUpgradeCostForLevel());

        facility.Upgrade();
    }

    private void TryToRepairFacility(Facility facility)
    {
        if (facility.GetRepairCostForLevel() > warehouse.GetCurrentScrap())
        {
            return;
        }

        if(facility.GetLife() == facility.GetMaxLife())
        {
            return;
        }

        LoseScrap(facility.GetUpgradeCostForLevel());

        facility.Repair();
    }

    public void OpenEmails()
    {
        UnloadDialog();

        mailBox.LoadMails(mail);
        isEmailOpen = true;
    }

    public void CloseEmail()
    {
        mailBox.UnloadMails();
        VerifyIfHasUnreadEmail();
        isEmailOpen = false;
    }


    public void LoadDialog(DialogConfig dialogConfig, CharacterInGame character = null)
    {
        CloseEmail();

        if (character != null)
        {
            dialogCharacter = character;
        }
        
        loadedDialog = dialogConfig;

        var dialogChoices = loadedDialog.GetChoices();

        if(dialogChoices != null)
        {
            for (int i = 0; i < maxNumberOfChoices; i++)
            {
                choiceButtoms[i].gameObject.SetActive(i < dialogChoices.Count);

                if(i < dialogChoices.Count)
                {
                    choiceButtoms[i].GetComponentInChildren<TextMeshProUGUI>().text = dialogChoices[i].GetPreviewText();
                }
            }
        }

        if (loadedDialog.GetBeginDialogAction() != null)
        {
            dialogText.text = String.Format(loadedDialog.GetDialogText(), loadedDialog.GetBeginDialogAction().ExecuteAction());
        }
        else
        {
            dialogText.text = loadedDialog.GetDialogText();
        }
    }

    public void UnloadDialog()
    {
        loadedDialog = null;
        dialogCharacter = null;

        foreach (Button choiceButton in choiceButtoms)
        {
            choiceButton.gameObject.SetActive(false);
        }
    }

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
                ProcessHealingCharacters();
                ProcessGifts();
                endTurnText.text = "Continue";
                ProcessExpeditions();
                ProcessEnergy();
                break;
            case GameStage.Expedition:
                ChangeState(GameStage.Event);
                endTurnButtom.gameObject.SetActive(false);
                dialogText.text = "";
                ProcessEvent();
                break;
            case GameStage.Event:
                ChangeState(GameStage.Preparetion);
                AdvanceTurns(1);
                endTurnText.text = "End Turn";
                endTurnButtom.gameObject.SetActive(true);
                eventCharacter = null;
                break;
                
        }
    }

    public void AdvanceTurns(int amount)
    {
        turnsToObjective -= amount;

        if(turnsToObjective <= 0)
        {
            if(engineerCharacter == null)
            {
                EndGameScene.OpenEndGameScene(EndGameMode.WithOutEngineer);
            }
            else
            {
                EndGameScene.OpenEndGameScene(EndGameMode.Won);
            }

            return;
        }


        TurnAmountChanged?.Invoke(turnsToObjective);
        turnCountDown.text = turnsToObjective.ToString();

        foreach (var character in charactersInGame)
        {
            character.ProgressTurn();
        }

        engineerCharacter?.ProgressTurn();
    }

    private void ProcessEnergy()
    {
        cryogenics.Damage(passiveDamage);
        dock.Damage(passiveDamage);
        warehouse.Damage(passiveDamage);
        reactor.Damage(passiveDamage);
        greenhouse.Damage(passiveDamage);

        if (GetTotalEnergyDemand() > reactor.GetGeneratedEnergy())
        {
            cryogenics.Damage(damageForLackOfEnergy);
            dock.Damage(damageForLackOfEnergy);
            warehouse.Damage(damageForLackOfEnergy);
            reactor.Damage(damageForLackOfEnergy);
            greenhouse.Damage(damageForLackOfEnergy);
        }
    }

    private void ProcessEvent()
    {
        if(eventCharacter == null)
        {
            ProgressToNextStage();
            return;
        }

        DialogConfig eventDialog = ListUtils.GetRandomMember<DialogConfig>(eventPool);

        LoadDialog(eventDialog, eventCharacter);
    }

    private void ProcessExpeditions()
    {
        if(chatactersToGoOnExpedition == null || chatactersToGoOnExpedition.Count == 0)
        {
            return;
        }


        string expeditionReport = string.Empty;

        var charOnExpedition = new List<CharacterInGame>();
        charOnExpedition.AddRange(chatactersToGoOnExpedition);

        eventCharacter = ListUtils.GetRandomMember<CharacterInGame>(chatactersToGoOnExpedition);

        foreach (CharacterInGame character in charOnExpedition)
        {
            if (!character.CanGoToExpedition())
            {
                continue;
            }

            float randNumber = Random.Range(0f, 1f);

            if(randNumber <= expeditionFailChance)
            {
                expeditionReport += $"{character.GetCharacter().Name} did not find a single useful thing\n";
            }
            else
            {
                int scrapFound = Random.Range(minExpeditionScrapReward, maxExpeditionScrapReward + 1);
                expeditionReport += $"{character.GetCharacter().Name} found {scrapFound} scrap\n";
                GainScrap(scrapFound);
            }

            character.WentOnExpedition();
            CancelOrFinishExpedition(character);
        }

        dialogText.text = expeditionReport;
    }

    public void MarkEmailAsRead(Mail email)
    {
        var mail = this.mail.Find(m => m == email);

        mail.wasRead = true;
    }

    private void ProcessGifts()
    {
        foreach (var character in chatactersToGift)
        {
            character.Gift();
            character.SetIsWaitingOnGift(false);
        }

        chatactersToGift.Clear();
    }

    private void ProcessHealingCharacters()
    {
        foreach (var character in chatactersToHeal)
        {
            character.FullyHeal();
            character.SetIsOnHeal(false);
        }

        chatactersToHeal.Clear();
    }

    public void GenerateResources()
    {
        warehouse.AddFood(greenhouse.GetFoodProduction());
    }

    public void GainFood(int amount)
    {
        warehouse.AddFood(amount);
    }

    public void LoseFood(int amount)
    {
        warehouse.RemoveFood(amount);
    }

    public void GainScrap(int amount)
    {
        warehouse.AddScrap(amount);
    }

    public void LoseScrap(int amount)
    {
        warehouse.RemoveScrap(amount);
    }

    public void HealDialog(int amount)
    {
        dialogCharacter.Heal(amount);
    }
    public void DamageDialog(int amount)
    {
        dialogCharacter.Damage(amount);
    }
    public void FullyHealDialog()
    {
        dialogCharacter.FullyHeal();
    }
    public void KillDialog()
    {
        dialogCharacter.Kill();
    }

    public void ConsumeResources()
    {
        foreach(var characterInexpedition in chatactersToGoOnExpedition)
        {
            warehouse.RemoveFood(expeditionFoodCost);
        }

        foreach (var charactersBeingHealed in chatactersToHeal)
        {
            warehouse.RemoveFood(healFoodCost);
        }

        foreach (var characterBeingGift in chatactersToGift)
        {
            warehouse.RemoveFood(giftFoodCost);
        }

        foreach (CharacterInGame character in charactersInGame)
        {
            if (warehouse.GetCurrentFood() < passiveFoodCost && !character.GetCharacter().IsOnExpedition)
            {
                character.Damage(damageForLackOFood);
            }
            else
            {
                warehouse.RemoveFood(passiveFoodCost);
            }
        }

        if(engineerCharacter == null)
        {
            return;
        }

        if (warehouse.GetCurrentFood() < passiveFoodCost && !engineerCharacter.GetCharacter().IsOnExpedition)
        {
            engineerCharacter.Damage(damageForLackOFood);
        }
        else
        {
            warehouse.RemoveFood(passiveFoodCost);
        }
    }

    public int RollSkillCheck(SkillType type)
    {
        int skill = type switch
        {
            SkillType.Strength => dialogCharacter.GetCharacter().Strength,
            SkillType.Intteligence => dialogCharacter.GetCharacter().Inteligence,
            SkillType.Dexterity => dialogCharacter.GetCharacter().Dexterity,
            _ => throw new NotImplementedException(),
        };


        return Random.Range(1, 21) + skill;
    }

    public void AddMail(DialogConfig dialogConfig, CharacterInGame characterInGame)
    {
        mail.Add(new Mail(dialogConfig,characterInGame));
        VerifyIfHasUnreadEmail();
    }

    public void VerifyIfHasUnreadEmail()
    {
        foreach (Mail email in mail)
        {
            if (!email.wasRead)
            {
                newEmailGameObject.SetActive(true);
                return;
            }
        }

        newEmailGameObject.SetActive(false);
    }

    internal void KillCharacter(CharacterInGame characterInGame)
    {
        if(characterInGame.GetCharacter().IsEngineer) 
        {
            engineerCharacter = null;
        }
        else
        {
            charactersInGame.Remove(characterInGame);
        }

        Destroy(characterInGame.gameObject);
    }
}

public enum GameStage
{
    Preparetion = 0,
    Expedition = 1,
    Event = 2,
}

public enum SkillType
{
    Strength = 0,
    Intteligence = 1,
    Dexterity = 2,
}


public enum EndGameMode
{
    Lost = 0,
    WithOutEngineer = 1,
    Won = 3,
}