using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ChartManager : MonoBehaviour
{
     
     
     public GameObject contentsprefab; // 자식
     public GameObject scroll_panel; // 부모
     
     public GameObject deTailPanel;
     public Text name;

    [System.Serializable]
    public class Result
    {
        public EtfItemList[] etfItemList;
    }
   
    [System.Serializable]
    public class EtfItemList  
    {
        public string itemname; // 종목 이름 
        public int nowVal; // 현재가
        public int quant; // 거래량
        public int amonut; // 거래 대금
        public int marketSum; // 시가 총액
    }

    [System.Serializable]
    public class Root
    {
        public string resultCode;
        public Result result;
    }
    private void Start()
    { 
        StartCoroutine(Chart());
    }

    IEnumerator Chart()
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get("https://finance.naver.com/api/sise/etfItemList.nhn"))
        {
            yield return request.SendWebRequest();

            if (request.isDone)
            {
               string data = "";
               data = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data); 
                var _etfItemLists = JsonUtility.FromJson<Root>(data);

                int i = 0;
                foreach (var item in _etfItemLists.result.etfItemList) // List에 있는 아이템들 다 가져옴
                {
                    // contents의 자식 프리팹을 생성
                    GameObject contents = Instantiate(contentsprefab) as GameObject;
                    contents.transform.SetParent(scroll_panel.transform, false);
                    // contents의 프리팹의 텍스트 컴포넌틀르 가져와서 수정

                    contents.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        name.text = item.itemname;

                        deTailPanel.SetActive(true);

                    });
                    contents.GetComponentInChildren<Text>().text = item.itemname + "                  " + item.nowVal + "                  " + item.quant + "                " + item.amonut + "                  " + item.marketSum;
                    // _etfItemLists.result.etfItemList를 10개 가져옴
                    i++;
                    if (i > 9)
                        break;

                    // ChartImageManager chartImageManager = GameObject.Find("StockChart_Image").GetComponent<ChartImageManager>();


                }
            }
        }
    }
    
    public void OnClick(GameObject clickedobj)
    {
        name.text = clickedobj.GetComponentInChildren<Text>().text;
        deTailPanel.SetActive(true);

    }

  }




