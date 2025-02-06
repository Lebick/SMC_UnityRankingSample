using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    public List<Transform> rankUIParents = new();

    public Text finalScore;

    public Text alertText;

    public CanvasGroup inputGroup;
    public InputField nameInput;

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        finalScore.text = $"���� ����\n{GamePlayManager.instance.score:D6}";

        for(int i=0; i<rankUIParents.Count; i++)
        {
            //0,1,2��° �ڽ��� ���� ����, �̸�, ���� ������ �س��ұ� ������ �Ʒ��� ���� �ۼ���
            Text rank = rankUIParents[i].GetChild(0).GetComponent<Text>();
            Text name = rankUIParents[i].GetChild(1).GetComponent<Text>();
            Text score = rankUIParents[i].GetChild(2).GetComponent<Text>();

            rank.text = $"{i+1:D2}";

            //�ش� ������ ��ŷ ������ ���ٸ�
            if(RankingManager.instance.rankData.Count <= i)
            {
                name.text = "---";
                score.text = "------";
            }
            else //��ŷ ������ �ִٸ�
            {
                RankData currentData = RankingManager.instance.rankData[i];

                name.text = currentData.name;
                score.text = currentData.score.ToString("D6");
            }
        }
    }

    public void OnClickRegisterBtn()
    {
        //��ŷ ���
        if (RankingManager.instance.Register(nameInput.text))
        {
            //��ŷ ��� ������ ��ȣ�ۿ��� �ٽ� �Ұ����ϰ���
            inputGroup.interactable = false;
        }
        else
        {
            //��ŷ ��� ���� �� UI�� ���� �˸�
            alertText.text = "�̸��� Ȯ�����ּ���.";
        }
    }
}
