using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScene : MonoBehaviour
{
    [SerializeField] private List<EndGameOption> options;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private Button quitButton;

    private static EndGameMode endGame;

    [Serializable]
    private class EndGameOption
    {
        public EndGameMode Mode;
        [ResizableTextArea]
        public string EndGameText;
    }

    private EndGameOption GetEndGameOption(EndGameMode mode) => options.Find(x => x.Mode == mode);

    private void Awake()
    {
        endGameText.text = GetEndGameOption(endGame).EndGameText;
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public static void OpenEndGameScene(EndGameMode mode)
    {
        endGame = mode;

        SceneManager.LoadScene("EndGameScene");
    }
}
