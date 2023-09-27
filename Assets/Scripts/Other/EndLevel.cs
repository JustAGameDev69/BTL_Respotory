using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public bool isVictoryPanelActive = false;

    [SerializeField] private IngameUIManager inGameUI;
    [SerializeField] private GameObject VictoryPanel;
    [SerializeField] private PlayerUpgradeSystem upgradeSystem;
    [SerializeField] private SceneTransition sceneTransition;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inGameUI.PauseGame();
            VictoryPanel.SetActive(true);
            isVictoryPanelActive = true;
            AudioManager.Instance.PlaySFX("Winning");
            inGameUI.totalCoin = inGameUI.coinInlevel;
            inGameUI.UpdatePlayerTotalCoin();
            upgradeSystem.UpdatePlayerRealCoin();
        }
    }

    public void OnClickNextLevelButton()
    {
        inGameUI.ResumeGame();
        VictoryPanel.SetActive(false);
        isVictoryPanelActive = false;
        sceneTransition.LoadNextLevel();
        inGameUI.coinInlevel = 0;
        inGameUI.UpdatePlayerInGameCoin();
    }

    public void OnClickMainMenuButton()
    {
        inGameUI.OnClickMainMenuButton();
    }

    public void OnClickUpgradeButton()
    {
        inGameUI.UpdatePlayerTotalCoin();
        upgradeSystem.OnClickUpgradeButton();
    }

}