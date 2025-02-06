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
        finalScore.text = $"최종 점수\n{GamePlayManager.instance.score:D6}";

        for(int i=0; i<rankUIParents.Count; i++)
        {
            //0,1,2번째 자식을 각각 순위, 이름, 점수 순으로 해놓았기 때문에 아래와 같이 작성함
            Text rank = rankUIParents[i].GetChild(0).GetComponent<Text>();
            Text name = rankUIParents[i].GetChild(1).GetComponent<Text>();
            Text score = rankUIParents[i].GetChild(2).GetComponent<Text>();

            rank.text = $"{i+1:D2}";

            //해당 순위에 랭킹 정보가 없다면
            if(RankingManager.instance.rankData.Count <= i)
            {
                name.text = "---";
                score.text = "------";
            }
            else //랭킹 정보가 있다면
            {
                RankData currentData = RankingManager.instance.rankData[i];

                name.text = currentData.name;
                score.text = currentData.score.ToString("D6");
            }
        }
    }

    public void OnClickRegisterBtn()
    {
        //랭킹 등록
        if (RankingManager.instance.Register(nameInput.text))
        {
            //랭킹 등록 성공시 상호작용이 다시 불가능하게함
            inputGroup.interactable = false;
        }
        else
        {
            //랭킹 등록 실패 시 UI를 통해 알림
            alertText.text = "이름을 확인해주세요.";
        }
    }
}
