using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RankingManager : Singleton<RankingManager>
{
    //리스트 만들 때 반드시 초기화해줘야함
    public List<RankData> rankData = new();

    private void Start()
    {
        //실행 시 저장된 정보를 로드함
        Load();
    }

    public bool Register(string name)
    {
        //닉네임이 3글자 이상이어야만 등록할 수 있도록 하기 위함
        if(name.Length < 3)
        {
            Debug.LogError("닉네임 길이 부족");
            return false;
        }

        //새로운 이름과 점수를 등록
        rankData.Add(new RankData(name, GamePlayManager.instance.score));

        Sort(); //정렬
        Save(); //저장

        return true; //성공적으로 등록되었음을 알림
    }

    public void Load()
    {
        //PlayerPrefs에 저장된 정보를 불러오되, 키값이 없으면 멈춰야함. (데이터가 없으므로)

        //랭킹 데이터 초기화
        rankData.Clear();

        for(int i=0; i<10; i++)
        {
            //해당 인덱스(순위)에 정보가 있다면
            if (PlayerPrefs.HasKey($"RankName{i}"))
            {
                string name = PlayerPrefs.GetString($"RankName{i}");
                int score = PlayerPrefs.GetInt($"RankScore{i}");

                //불러온 데이터 등록
                rankData.Add(new RankData(name, score));
            }
            else
            {
                //해당 순위에 정보가 없다면, 그 아래 순위에도 정보가 없다는것이므로 더이상 찾지 않음.
                break; 
            }
        }
    }

    public void Save()
    {
        //PlayerPrefs를 통해 저장할것임.

        for(int i=0; i<rankData.Count; i++)
        {
            PlayerPrefs.SetString($"RankName{i}", rankData[i].name);
            PlayerPrefs.SetInt($"RankScore{i}", rankData[i].score);
        }
    }

    public void Sort()
    {
        //System.Linq를 사용하여 정렬.
        //점수가 높을수록 순위가 높아야하므로 OrderByDescending을 사용함.
        //결과물이 리스트가 아닌 배열로 나오므로, ToList를 사용해야함.
        rankData = rankData.OrderByDescending(a => a.score).ToList();

        //최대 10명까지의 값만 기록하기 위함.
        //예를들어 이미 10명이 저장된 상태로, 한명이 추가되었을 때
        //가장 낮은 값을 가진 사람은 0~9번째 인덱스이 위치하지 않을 것이기 때문에
        //아래 코드를 통해 자동으로 10명이 추려지게됨.
        rankData = rankData.GetRange(0, Mathf.Min(rankData.Count, 10));
    }
}

//랭킹 데이터 클래스
//순위는 저장할 필요 없으므로 이름과 점수만 저장
[System.Serializable] //직렬화를 통해 인스펙터뷰에서 확인할 수 있도록 함
public class RankData
{
    public string name;
    public int score;

    public RankData(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
