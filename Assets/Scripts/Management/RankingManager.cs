using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RankingManager : Singleton<RankingManager>
{
    //����Ʈ ���� �� �ݵ�� �ʱ�ȭ�������
    public List<RankData> rankData = new();

    private void Start()
    {
        //���� �� ����� ������ �ε���
        Load();
    }

    public bool Register(string name)
    {
        //�г����� 3���� �̻��̾�߸� ����� �� �ֵ��� �ϱ� ����
        if(name.Length < 3)
        {
            Debug.LogError("�г��� ���� ����");
            return false;
        }

        //���ο� �̸��� ������ ���
        rankData.Add(new RankData(name, GamePlayManager.instance.score));

        Sort(); //����
        Save(); //����

        return true; //���������� ��ϵǾ����� �˸�
    }

    public void Load()
    {
        //PlayerPrefs�� ����� ������ �ҷ�����, Ű���� ������ �������. (�����Ͱ� �����Ƿ�)

        //��ŷ ������ �ʱ�ȭ
        rankData.Clear();

        for(int i=0; i<10; i++)
        {
            //�ش� �ε���(����)�� ������ �ִٸ�
            if (PlayerPrefs.HasKey($"RankName{i}"))
            {
                string name = PlayerPrefs.GetString($"RankName{i}");
                int score = PlayerPrefs.GetInt($"RankScore{i}");

                //�ҷ��� ������ ���
                rankData.Add(new RankData(name, score));
            }
            else
            {
                //�ش� ������ ������ ���ٸ�, �� �Ʒ� �������� ������ ���ٴ°��̹Ƿ� ���̻� ã�� ����.
                break; 
            }
        }
    }

    public void Save()
    {
        //PlayerPrefs�� ���� �����Ұ���.

        for(int i=0; i<rankData.Count; i++)
        {
            PlayerPrefs.SetString($"RankName{i}", rankData[i].name);
            PlayerPrefs.SetInt($"RankScore{i}", rankData[i].score);
        }
    }

    public void Sort()
    {
        //System.Linq�� ����Ͽ� ����.
        //������ �������� ������ ���ƾ��ϹǷ� OrderByDescending�� �����.
        //������� ����Ʈ�� �ƴ� �迭�� �����Ƿ�, ToList�� ����ؾ���.
        rankData = rankData.OrderByDescending(a => a.score).ToList();

        //�ִ� 10������� ���� ����ϱ� ����.
        //������� �̹� 10���� ����� ���·�, �Ѹ��� �߰��Ǿ��� ��
        //���� ���� ���� ���� ����� 0~9��° �ε����� ��ġ���� ���� ���̱� ������
        //�Ʒ� �ڵ带 ���� �ڵ����� 10���� �߷����Ե�.
        rankData = rankData.GetRange(0, Mathf.Min(rankData.Count, 10));
    }
}

//��ŷ ������ Ŭ����
//������ ������ �ʿ� �����Ƿ� �̸��� ������ ����
[System.Serializable] //����ȭ�� ���� �ν����ͺ信�� Ȯ���� �� �ֵ��� ��
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
