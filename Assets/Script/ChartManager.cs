using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChartManager : MonoBehaviour
{
    public static ChartManager instance;

    public GameObject content;
    public GameObject crawling; 

    public GameObject purchaseWindow;
    public Text stockname;


    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
    }

    private void Start() {
        StartCoroutine(Chart());
    }


    // 웹 크롤링
    IEnumerator Chart()
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get("https://finance.naver.com/api/sise/etfItemList.nhn")) 
        {
            yield return request.SendWebRequest(); // 원격 서버와 통신 시작

            if (request.result == UnityWebRequest.Result.Success)
            {
                // UTF8로 인코딩 시 오류, Encoding.GetEncoding("EUC-KR") 사용 시 한글 깨짐 해결
                // EUC-KR 인코딩은 빌드 시 cotents 생성 안됨
                string _data = "";
                _data = Encoding.UTF8.GetString(request.downloadHandler.data); 
                Root _etfItemLists = JsonUtility.FromJson<Root>(_data);
           
                int index = 0;
                foreach (EtfItemList item in _etfItemLists.result.etfItemList) // 아이템 가져오기
                {
                    // 스크롤뷰 컨텐츠 안에 오브젝트 생성
                    GameObject stock = Instantiate(crawling);
                    stock.transform.SetParent(content.transform, false);
                    
                    // 종목 버튼 클릭 이벤트
                    stock.GetComponent<Button>().onClick.AddListener(() => 
                    {
                        stockname.text = item.itemname;
                        purchaseWindow.SetActive(true);
                    });

                    stock.GetComponentInChildren<Text>().text = string.Format("{0}   {1}   {2}   {3}   {4}", 
                    item.itemname, 
                    item.nowVal, 
                    item.quant, 
                    item.amonut, 
                    item.marketSum);

                    index++;
                    if (index > 9)
                        break;
                }
            }
            else{
                Debug.Log($"Error: {request.error}");
            }
        }
    }


    [System.Serializable]
    public class Root
    {
        public string resultCode;
        public Result result;
    }

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
}




