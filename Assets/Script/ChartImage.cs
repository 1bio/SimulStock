using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChartImage : MonoBehaviour
{
    public string[] textureUrl;

    // 버튼 이벤트
    public void GetImage(){
        int index = transform.GetSiblingIndex();
        StartCoroutine(DownloadImage(textureUrl[index]));
    }
    
    // 차트 이미지 크롤링 
    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success){

            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite webSprite = SpriteFromTexture2D(webTexture);
            GameObject.Find("ChartImage").GetComponent<Image>().sprite = webSprite;
        }
        else{
            Debug.Log($"Error:{request.error}");
        }
    }
    
    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}

