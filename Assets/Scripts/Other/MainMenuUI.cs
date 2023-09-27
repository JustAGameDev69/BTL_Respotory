using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private RawImage[] rawImg;
    [SerializeField] private float _x, _y;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button musicButton;

    private bool isInforPanelActive = false;
    private bool isExitPanelActive = false;
    private bool isButtonClicked = false;


    void Update()
    {
        foreach (var item in rawImg)
        {
            item.uvRect = new Rect(item.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, item.uvRect.size);
        }

    }

    public void OnClickPlayButton()
    {
        if (isButtonClicked == false)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            SceneManager.LoadScene(1);
            AudioManager.Instance.PlayMusic("IngameMusic");
        }
    }

    public void OnClickInfoButton()
    {
        if(isInforPanelActive == false && isButtonClicked == false)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            infoPanel.SetActive(true);
            isInforPanelActive = true;
            isButtonClicked = true;
        }
        else if(isInforPanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            infoPanel.SetActive(false);
            isInforPanelActive = false;
            isButtonClicked = false;
        }
    }

    public void OnCLickExitButton()
    {
        if (isButtonClicked == false && isExitPanelActive == false)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            exitPanel.SetActive(true);
            isExitPanelActive = true;
            isButtonClicked = true;
            exitButton.enabled = false;
        }
        else
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            exitPanel.SetActive(false);
            isExitPanelActive = false;
            isButtonClicked = false;
            exitButton.enabled = true;

        }
    }

    #region Exit Confirm
    public void OnClickYesButton()
    {
        if(isExitPanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            Application.Quit();
        }
    }

    public void OnClickNoButton()
    {
        if(isExitPanelActive == true)
        {
            AudioManager.Instance.PlaySFX("ClickButton");
            exitPanel.SetActive(false);
            isExitPanelActive = false;
            isButtonClicked = false;
            exitButton.enabled = true;
        }
    }
    #endregion          //Yes No Exit Confirm 
}
