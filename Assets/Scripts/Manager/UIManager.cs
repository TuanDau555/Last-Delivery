using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Parameter
    [SerializeField] private FeedbackTextSO feedbackTextSO;
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float autoHide;

    private float hideTimer;
    private bool isShowingUI;
    #endregion

    void Start()
    {
        HideFeedback();
    }

    void Update()
    {
        if (isShowingUI)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
            {
                HideFeedback();
            }
        }
    }

    public void ShowDeliveryTableFeedback(string tableName)
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.deliveryTableText + tableName;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    public void ShowChangeConditionFeedback()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.changeSceneConditionText;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    public void ShowPlayerHoldSomethingFeedback()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.playerHoldSomethingText;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    public void ShowDeliverySuccessFeedback(int moneyAdd)
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.deliverySuccessFeedback + "+" + moneyAdd.ToString();

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    public void ShowWrongDeliveryFeedback(int penaltyMoney)
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.wrongDeliveryFeedback + "-" + penaltyMoney.ToString();

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    public void ShowAlreadyDeliverObject()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.alreadyDeliverObjectFeedback;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    
    public void FinishTheOrderFeedback()
    {
         if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.finishTheOrderFeedback;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    
    public void NotEnoughMoney()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.notEnoughMoney;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    public void NextDayWelcome()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.nextDayCongratulation;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    
    private void HideFeedback()
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        isShowingUI = false;
    }

    private void StartCountDown()
    {
        hideTimer = autoHide;
        isShowingUI = true;
    }
}
