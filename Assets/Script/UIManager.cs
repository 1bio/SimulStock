using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Login")]
    public GameObject startMenu;
    public InputField usernameField;
    public GameObject chatting;

    [Header("Video Player")]
    public Canvas videoUI;
    public VideoPlayer videoPlayer;
    private bool isPlaying = false;

    [Header("NPC Dialogue")]
    public GameObject npcDialogue;
    public GameObject[] npcTexts = new GameObject[4];

    [Header("Stock")]
    public Button stockButton;
    public GameObject stock;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }
    
    // Login 버튼 이벤트 
    public void ConnectToServer(){
        startMenu.SetActive(false);
        chatting.SetActive(true);
        usernameField.interactable = false;
        
        Client.instance.ConnectToServer();
    }

    // 종목 목록 조회 이벤트
    public void CheckStockList(){
        stock.gameObject.SetActive(true);
    }

    // Video 재생 
    public void PlayVideo(){
        isPlaying = true;
        videoPlayer.Play();
    }

    // Video 중지
    public void PauseVideo(){
        isPlaying = false;
        videoPlayer.Stop();
    }
    
    // Video 리셋
    public void ResetVideo(){
        if(isPlaying){
            PauseVideo();
            videoPlayer.time = 0;
        }

        PlayVideo();
    }

    // UI 열기
    public void OpenWindow(GameObject window)
    {
        window.gameObject.SetActive(true);
    }

    // UI 닫기    
    public void CloseWindow(GameObject window){
        window.gameObject.SetActive(false);
    }

    // 종료
    public void Quit(){
        Application.Quit();
    }
}
