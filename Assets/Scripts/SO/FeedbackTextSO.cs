using UnityEngine;

[CreateAssetMenu(fileName = "Text", menuName = "Text Feed back")]
public class FeedbackTextSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string deliveryTableText;

    [TextArea(3, 10)]
    public string changeSceneConditionText;

    [TextArea(3, 10)]
    public string playerHoldSomethingText;
    
    [TextArea(3, 10)]
    public string deliverySuccessFeedback;
    [TextArea(3, 10)]
    public string wrongDeliveryFeedback;

    [TextArea(3, 10)]
    public string alreadyDeliverObjectFeedback;

    [TextArea(3, 10)]
    [Tooltip("Player can't bring order when delivery")]
    public string finishTheOrderFeedback;
}