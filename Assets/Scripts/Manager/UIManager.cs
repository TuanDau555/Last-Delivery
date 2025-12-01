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

    /// <summary>
    /// Hàm này thể hiện vị trí món nên được giao đến (Container nào ?)
    /// </summary>
    /// <param name="tableName">Là địa điểm được giao dến</param>
    public void ShowDeliveryTableFeedback(string tableName)
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.deliveryTableText + tableName;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    /// <summary>
    /// Cửa để qua lV2 
    /// Chỉ khi đạt được từ 7 ngày thì mới mở của
    /// </summary>
    public void ShowChangeConditionFeedback()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.changeSceneConditionText;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    /// <summary>
    /// Nếu player cầm gì đó rồi thì không cầm được thêm nữa
    /// </summary>
    public void ShowPlayerHoldSomethingFeedback()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.playerHoldSomethingText;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    /// <summary>
    /// Khi giao hàng thành công thì hiện feedback
    /// </summary>
    /// <param name="moneyAdd">Số tiền được thưởng khi hoàn thành</param>
    public void ShowDeliverySuccessFeedback(int moneyAdd)
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.deliverySuccessFeedback + "+" + moneyAdd.ToString();

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    /// <summary>
    /// Khi giao hành thất bại thì hiện feedback
    /// </summary>
    /// <param name="penaltyMoney">Số tiền bị phạt khi không hoàn thành</param>
    public void ShowWrongDeliveryFeedback(int penaltyMoney)
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.wrongDeliveryFeedback + "-" + penaltyMoney.ToString();

        feedbackPanel.SetActive(true);

        StartCountDown();
    }

    /// <summary>
    /// Feedback khi món hàng này đã được giao hoặc đang giao
    /// </summary>
    public void ShowAlreadyDeliverObject()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.alreadyDeliverObjectFeedback;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    
    /// <summary>
    /// Feedback này khi người chơi cố gắng đem hàng qua một địa điểm khác mà không giao
    /// </summary>
    public void FinishTheOrderFeedback()
    {
         if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.finishTheOrderFeedback;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    
    /// <summary>
    /// Feedback hiện lên khi player không đủ tiền mua đồ
    /// </summary>
    public void NotEnoughMoney()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.notEnoughMoney;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    
    /// <summary>
    /// Feedback hiện lên khi player kết thúc một ngày
    /// </summary>
    public void NextDayWelcome()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.nextDayCongratulation;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    
    public void LowMoodWarning()
    {
        if (feedbackPanel == null || feedbackText == null) return;

        feedbackText.text = feedbackTextSO.lowMoodWarning;

        feedbackPanel.SetActive(true);

        StartCountDown();
    }
    
    private void HideFeedback()
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        isShowingUI = false;
    }

    /// <summary>
    /// Đếm ngược trước khi tắt feedback
    /// </summary>
    private void StartCountDown()
    {
        hideTimer = autoHide;
        isShowingUI = true;
    }
}
