// ʾ������ҿ�������
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Character playerCharacter;
    public Character enemyCharacter; // ��ǰ����


    private void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();

    }
    void Update()
    {
        if (playerCharacter.canAct)
        {
            TimeManager.Instance.PauseGame(); // ��ͣ��Ϸ���ȴ��������

            if (Input.GetMouseButtonDown(0)) // ���������������Կ�ʼ��ק����
            {
                // ����Ӧ��ʵ�ֿ��Ƶ���ק�߼�������ѡ�������еĿ��Ʋ���ʾ����קЧ��
                Debug.Log("Card is being dragged");
            }
            else if (Input.GetMouseButtonUp(0)) // �������ɿ����ͷſ���
            {
                // ����Ӧ��ʵ�ֿ����ͷŵ��߼�������ȷ�����Ƶ�Ŀ�겢ִ�п���Ч��
                Debug.Log("Card released");
            }

            TimeManager.Instance.ResumeGame(); // ��Ҳ�����ɺ�ָ���Ϸ
        }
    }
}