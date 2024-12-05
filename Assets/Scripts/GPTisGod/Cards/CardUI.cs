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
    public static bool isCardEffectActive = false; // �����жϿ���Ч���Ƿ����ڽ���
    public Character playerCharacter;
    public Character enemyCharacter;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.position;

        // ���ÿ��Ƶ�UI��ʾ
        UpdateCardVisual();
        // ��ȡ��Һ͵��˵� Character ʵ��
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }

    public void UpdateCardVisual()
    {
        // ���¿�������
        Text nameText = transform.GetChild(3).GetComponent<Text>();
        if (nameText != null)
        {
            nameText.text = cardData.cardName;
        }
        //����
        Text desText = transform.GetChild(1).GetComponent<Text>();
        if (desText != null)
        {
            //desText.text = cardData.cardDescription;
        }
        //ͼƬ
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
            return; //��ΪIdle���ܴ�����ƣ����������ȡ��ʲô�ģ��ø�
        }
        if (isCardEffectActive)
        {
            return; // ����ִ�п���Ч��ʱ������ʼ�϶�
        }
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCardEffectActive) return; // ����ִ�п���Ч��ʱ�������϶�
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isCardEffectActive) return; // ����ִ�п���Ч��ʱ������������

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // �жϿ����Ƿ��ͷ�����Ч����
        if (!IsValidDropPosition(eventData.position))
        {
            rectTransform.position = originalPosition;
        }
        else
        {
            // ��ǿ���Ч�����ڽ���
            isCardEffectActive = true;
            playerCharacter.ExecuteCard(cardData, enemyCharacter);

            // �������Ƴ����Ʋ���ӵ����ƶ�
            Deck deck = playerCharacter.deck;
            if (deck != null)
            {
                deck.PlayCard(cardData);
            }

            // �Ƴ����� UI ��ģ�⿨�Ʊ������Ч��
            Destroy(gameObject);
        }
    }

    public static void CardEffectComplete()
    {
        // ��ǿ���Ч���Ѿ����
        isCardEffectActive = false;
    }

    private bool IsValidDropPosition(Vector2 position)
    {
        // �ж��ͷ�λ���Ƿ���Ч
        return true;
    }
}
