using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    #region Public Variables
    public Slider playerHealthBar;
    public PlayerCombat playerCombat;
    public Text inGameMoney;
    public int coinInlevel = -1;
    public GameObject pauseMenuPanel;
    public GameObject settingMenuPanel;
    public GameObject questPanel;
    public GameObject rulePanel;
    public GameObject deathPanel;
    public int totalCoin = 0;              //Coin in total that player gain
    #endregion

    #region Private Variable
    public int healthPot;
    [SerializeField] private Text totalCoins;
    [SerializeField] private Animator animator;
    [SerializeField] private Text inGameHealthPot;
    private bool isPausePanelActive = false;
    private bool isSettingPanelActive = false;
    #endregion
        
    private void Awake()
    {
        UpdatePlayerTotalCoin();
        healthPot = 1;
        UpdatePlayerInGameHealthPot();
        UpdatePlayerInGameCoin();
        CoinSpawn.CoinCollected += UpdatePlayerInGameCoin;          //use event to active updatecoin function when coincollected be called
        HealthPotSpawner.HealthPotCollected += UpdatePlayerInGameHealthPot;
    }

    #region Setting Button
    public void OnClickSettingButton()
    {

        if(isSettingPanelActive == false && isPausePanelActive == false)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            settingMenuPanel.SetActive(true);
            isSettingPanelActive = true;
            PauseGame();
        }
    }

    public void OnClickRuleButton()
    {
        if (isSettingPanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            rulePanel.SetActive(true);
        }
    }

    public void OnOkayButton()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        rulePanel.SetActive(false);
    }

    public void OnClickRestartButton()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        ResumeGame();
    }

    #endregion

    #region Pause Button

    public void OnClickPauseButton()
    {
        if (isPausePanelActive == false && isSettingPanelActive == false)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            pauseMenuPanel.SetActive(true);
            isPausePanelActive = true;
            PauseGame();
        }
    }

    public void OnClickResumeButton() 
    {
        if (isPausePanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            pauseMenuPanel.SetActive(false);
            isPausePanelActive = false;
            ResumeGame();
        }
        else if (isSettingPanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            settingMenuPanel.SetActive(false);
            isSettingPanelActive = false;
            ResumeGame();
        }
    }

    public void OnClickQuestButton()
    {
        if(isPausePanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            questPanel.SetActive(true);
        }
    }

    public void OnClickYesQuestButton()
    {
        if(isPausePanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            questPanel.SetActive(false);
        }
    }

    public void OnClickQuitButton()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        SceneManager.LoadScene(0);
        ResumeGame();
    }

    #endregion

    #region Death Panel
    public void DeathPanel()
    {
        deathPanel.SetActive(true);
        PauseGame();
    }

    public void OnClickMainMenuButton()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        SceneManager.LoadScene(0);
        ResumeGame();
    }

    #endregion

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {

    }

    public void UpdatePlayerHealthBar()
    {
        playerHealthBar.value = playerCombat.playerCurrentHealth;
    }

    public void UpdatePlayerInGameCoin()
    {
        coinInlevel += 1;
        inGameMoney.text = coinInlevel.ToString();
    }

    public void UpdatePlayerTotalCoin() 
    {
        totalCoins.text = totalCoin.ToString();
    }

    #region Health Pot

    private void UpdatePlayerInGameHealthPot()
    {
        if (healthPot <= 3)
        {
            healthPot += 1;
            inGameHealthPot.text = healthPot.ToString();
        }
    }

    public void OnClickHealthPotButton()
    {
        AudioManager.Instance.PlaySFX("Healing");
        animator.SetTrigger("Healing");
        int playerHealth = playerCombat.playerCurrentHealth;
        if (healthPot > 0)
        {
            if(playerHealth > 70)
            {
                playerCombat.playerCurrentHealth += 100 - playerHealth;
            }
            else if(playerHealth > 0 && playerHealth <= 70)
            {
                playerCombat.playerCurrentHealth += 30;
            }
            healthPot -= 1;
            inGameHealthPot.text = healthPot.ToString();
            UpdatePlayerHealthBar();
        }
    }

    #endregion

}
