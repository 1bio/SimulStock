using UnityEngine;

public class VideoCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.instance.videoUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.instance.PauseVideo();
            UIManager.instance.videoUI.gameObject.SetActive(false);
        }
    }
}
