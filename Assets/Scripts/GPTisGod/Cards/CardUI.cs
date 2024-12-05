using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardData cardData; // 关联的卡牌数据

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    public static bool isCardEffectActive = false; // 用于判断卡牌效果是否正在进行
    public Character playerCharacter;
    public Character enemyCharacter;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.position;

        // 设置卡牌的UI显示
        UpdateCardVisual();
        // 获取玩家和敌人的 Character 实例
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }

    public void UpdateCardVisual()
    {
        // 更新卡牌名称
        Text nameText = transform.GetChild(3).GetComponent<Text>();
        if (nameText != null)
        {
            nameText.text = cardData.cardName;
        }
        //描述
        Text desText = transform.GetChild(1).GetComponent<Text>();
        if (desText != null)
        {
            //desText.text = cardData.cardDescription;
        }
        //图片
        Image cardImage = transform.GetChild(0).GetComponent<Image>();
        if (cardImage != null && cardData.cardImage != null)
        {
            //cardImage.sprite = cardData.cardImage;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (playerCharacter.currentState != CharacterState.Idle)
        {
            return; //不为Idle不能打出卡牌，后面如果有取消什么的，得改
        }
        if (isCardEffectActive)
        {
            return; // 正在执行卡牌效果时不允许开始拖动
        }
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCardEffectActive) return; // 正在执行卡牌效果时不允许拖动
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isCardEffectActive) return; // 正在执行卡牌效果时不允许打出卡牌

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // 判断卡牌是否释放在有效区域
        if (!IsValidDropPosition(eventData.position))
        {
            rectTransform.position = originalPosition;
        }
        else
        {
            // 标记卡牌效果正在进行
            isCardEffectActive = true;
            playerCharacter.ExecuteCard(cardData, enemyCharacter);

            // 从手牌移除卡牌并添加到弃牌堆
            Deck deck = playerCharacter.deck;
            if (deck != null)
            {
                deck.PlayCard(cardData);
            }

            // 移除卡牌 UI 以模拟卡牌被打出的效果
            Destroy(gameObject);
        }
    }

    public static void CardEffectComplete()
    {
        // 标记卡牌效果已经完成
        isCardEffectActive = false;
    }

    private bool IsValidDropPosition(Vector2 position)
    {
        // 判断释放位置是否有效
        return true;
    }
}
