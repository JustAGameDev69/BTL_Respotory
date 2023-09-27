using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeSystem : MonoBehaviour
{
    public int playerRealCoins;
    public int upgradeATDPrice = 3;
    public int upgradeATRPrice = 3;
    public int upgradeATSPrice = 3;
    public int upgradeHealthPrice = 3;
    public int upgradeMovePrice = 3;
    public bool isUpgradePanelActive;

    private bool isBuyFaildPanelActive = false;
    [SerializeField] private GameObject buyFailedPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private IngameUIManager inGameUI;
    [SerializeField] private EndLevel endLevel;
    [SerializeField] private Text playerHealth;
    [SerializeField] private Text playerAttackDamage;
    [SerializeField] private Text playerAttackRange;
    [SerializeField] private Text playerAttackSpeed;
    [SerializeField] private Text playerMoveSpeed;

    private void Start()
    {
        UpdatePlayerRealCoin();
        PlayerHealth();
        PlayerDamage();
        PlayerAttackSpeed();
        PlayerAttackRange();
        PlayerMoveSpeed();

    }

    public void UpdatePlayerRealCoin()
    {
        playerRealCoins = inGameUI.totalCoin;           //Get access to player total coins (real coins)
    }

    private bool CheckEnoughCoins(int price)
    {
        if(playerRealCoins >= price)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnClickUpgradeButton()
    {
        
        if (isUpgradePanelActive == false)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            upgradePanel.SetActive(true);
            isUpgradePanelActive = true;
            inGameUI.PauseGame();
        }
    }



    #region Upgrade Panel Button
    public void PlayerCoinUpdate()
    {
        inGameUI.totalCoin = playerRealCoins;
        inGameUI.UpdatePlayerTotalCoin();
    }

    public void OnClickUpgradeHealth()
    {
        if (CheckEnoughCoins(upgradeHealthPrice))
        {
            AudioManager.Instance.PlaySFX("BuySuccess");
            playerCombat.playerHealth += 20;
            playerRealCoins -= upgradeHealthPrice;
            PlayerCoinUpdate();
            upgradeHealthPrice += 3;
            PlayerHealth();
        }
        else
        {
            AudioManager.Instance.PlaySFX("BuyFailed");
            buyFailedPanel.SetActive(true);
            isBuyFaildPanelActive = true;
        }

    }

    public void OnClickOkayButton()
    {
        if(isBuyFaildPanelActive)
        {
            buyFailedPanel.SetActive(false);
            isBuyFaildPanelActive = false;
        }
    }

    public void OnClickUpgradeAttackDamage()
    {
        if (CheckEnoughCoins(upgradeATDPrice))
        {
            AudioManager.Instance.PlaySFX("BuySuccess");
            playerRealCoins -= upgradeATDPrice;
            PlayerCoinUpdate();
            playerCombat.attackDamage += 2;
            upgradeATDPrice += 2;
            PlayerDamage();
        }
        else
        {
            AudioManager.Instance.PlaySFX("BuyFailed");
            buyFailedPanel.SetActive(true);
            isBuyFaildPanelActive = true;
        }
    }

    public void OnClickUpgradeAttackRange()
    {
        if (CheckEnoughCoins(upgradeATRPrice))
        {
            AudioManager.Instance.PlaySFX("BuySuccess");
            playerRealCoins -= upgradeATRPrice;
            PlayerCoinUpdate();
            playerCombat.attackRange += 0.05f;
            upgradeATRPrice += 2;
            PlayerAttackRange();
        }
        else
        {
            AudioManager.Instance.PlaySFX("BuyFailed");
            buyFailedPanel.SetActive(true);
            isBuyFaildPanelActive = true;
        }
    }

    public void OnClickUpgradeAttackSpeed()
    {
        if (CheckEnoughCoins(upgradeATSPrice))
        {
            AudioManager.Instance.PlaySFX("BuySuccess");
            playerRealCoins -= upgradeATSPrice;
            PlayerCoinUpdate();
            playerCombat.attackSpeed += 0.1f;
            upgradeATSPrice += 2;
            PlayerAttackSpeed();
        }
        else
        {
            AudioManager.Instance.PlaySFX("BuyFailed");
            buyFailedPanel.SetActive(true);
            isBuyFaildPanelActive = true;
        }
    }

    public void OnClickUpgradeMoveSpeed()
    {
        if (CheckEnoughCoins(upgradeMovePrice))
        {
            AudioManager.Instance.PlaySFX("BuySuccess");
            playerRealCoins -= upgradeMovePrice;
            PlayerCoinUpdate();
            playerController.speed += 0.2f;
            upgradeMovePrice += 2;
            PlayerMoveSpeed();
        }
        else
        {
            AudioManager.Instance.PlaySFX("BuyFailed");
            buyFailedPanel.SetActive(true);
            isBuyFaildPanelActive = true;
        }
    }

    public void OnClickResumeButton()
    {
        if (isUpgradePanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            upgradePanel.SetActive(false);
            isUpgradePanelActive = false;
            if(endLevel.isVictoryPanelActive == false)
            {
                inGameUI.ResumeGame();
            }
        }
    }

    #endregion

    //Player Stats
    #region Show player stats

    public void PlayerHealth()
    {
        playerHealth.text = playerCombat.playerHealth.ToString();
    }

    public void PlayerDamage()
    {
        playerAttackDamage.text = playerCombat.attackDamage.ToString();
    }

    public void PlayerAttackSpeed()
    {
        playerAttackSpeed.text = playerCombat.attackSpeed.ToString();
    }

    public void PlayerAttackRange()
    {
        playerAttackRange.text = playerCombat.attackRange.ToString();
    }

    public void PlayerMoveSpeed()
    {
        playerMoveSpeed.text = playerController.speed.ToString();
    }

    #endregion

}
