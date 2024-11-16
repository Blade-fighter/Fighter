using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardData cardData; // �����Ŀ�������

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.position;

        // ���ÿ��Ƶ�UI��ʾ
        UpdateCardVisual();
    }

    void UpdateCardVisual()
    {
        // ���¿������ƺ�ͼƬ
        Text nameText = transform.GetChild(0).GetComponent<Text>(); 
        if (nameText != null)
        {
            nameText.text = cardData.cardName;
        }

        Image cardImage = transform.GetChild(3).GetComponent<Image>();
        if (cardImage != null && cardData.cardImage != null)
        {
            cardImage.sprite = cardData.cardImage;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // �жϿ����Ƿ��ͷ�����Ч����
        if (!IsValidDropPosition(eventData.position))
        {
            rectTransform.position = originalPosition;
        }
        else
        {
            // ִ�п��Ƶ�Ч��
            PlayerController playerController = FindObjectOfType<PlayerController>();
            Character enemy = playerController.enemyCharacter;
            playerController.playerCharacter.ExecuteCard(cardData, enemy);
        }
    }

    private bool IsValidDropPosition(Vector2 position)
    {
        // �ж��ͷ�λ���Ƿ���Ч
        return true;
    }
}
