using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardData cardData; // �����Ŀ�������

    private Canvas canvas;
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    public static bool isCardEffectActive = false; // �����жϿ���Ч���Ƿ����ڽ���
    public Character playerCharacter;
    public Character enemyCharacter;

    public bool IsMoving;
    public Vector3 MoveTarget;
    public bool MovingEndDestroy;
    public float MoveSpeed;
    public float TotalTime;
    public float CurTime;
    public Vector3 OriginPosition;
    public bool IsDiscard;
    public bool IsSrc;

    public int ToBagIndex;
    public bool CanInteractive = true;


    void Start()
    {

    }

    public void UpdateCardVisual()
    {
        IsMoving = false;
        MoveSpeed = 3000.0f;
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        // ��ȡ��Һ͵��˵� Character ʵ��
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
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

    void Update(){
        Move();
    }

    private void Move(){
        if(!IsMoving) return;
        rectTransform.anchoredPosition = Vector3.Lerp(OriginPosition, MoveTarget, CurTime/TotalTime);
        CurTime += Time.deltaTime;
        if(CurTime > TotalTime){
            IsMoving = false;
            // �������Ƴ����Ʋ���ӵ����ƶ�
            Deck deck = playerCharacter.deck;
            if (deck != null)
            {
                if(IsDiscard) deck.PlayCard(cardData);
                else if(IsSrc){
                    deck.drawPile.Add(cardData);
                    if(ToBagIndex != -1){
                        deck.InFresh = false;
                        deck.DrawCard(ToBagIndex);
                    }
                }
                if(MovingEndDestroy) Destroy(this.gameObject);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!CanInteractive) return;
        if (playerCharacter.currentState != CharacterState.Idle)
        {
            return; //��ΪIdle���ܴ�����ƣ����������ȡ��ʲô�ģ��ø�
        }
        Deck deck = playerCharacter.deck;
        if (isCardEffectActive || deck.InFresh)
        {
            return; // ����ִ�п���Ч��ʱ������ʼ�϶�
        }
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!CanInteractive) return;
        if (isCardEffectActive || IsMoving) return; // ����ִ�п���Ч��ʱ�������϶�
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!CanInteractive) return;
        if (isCardEffectActive || IsMoving) return; // ����ִ�п���Ч��ʱ������������

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // �жϿ����Ƿ��ͷ�����Ч����
        if (!IsValidDropPosition(eventData.position))
        {
            MoveTo(originalPosition, false, false, false);
        }
        else
        {
            // ��ǿ���Ч�����ڽ���
            isCardEffectActive = true;
            playerCharacter.ExecuteCard(cardData, enemyCharacter);

            Vector2 deckPos = playerCharacter.deck.discard.GetComponent<RectTransform>().anchoredPosition;
            MoveTo(deckPos, true, true, false);
        }
    }

    public void MoveTo(Vector2 Target, bool SelfDestroy, bool isDiscard, bool isSrc){
        IsDiscard = isDiscard;
        IsSrc = isSrc;
        IsMoving = true;
        MovingEndDestroy = SelfDestroy;
        MoveTarget = Target;
        OriginPosition = rectTransform.anchoredPosition;
        TotalTime = Math.Abs((OriginPosition - MoveTarget).magnitude / MoveSpeed);
        CurTime = 0.0f;
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
