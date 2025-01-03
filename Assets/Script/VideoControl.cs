
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace YoutubePlayer
{
    public class VideoControl : MonoBehaviour
    {
        public YoutubePlayer youtubePlayer;
        VideoPlayer videoPlayer;
        public Button bt_Play;
        public Button bt_Pause;
        public Button bt_Reset;
        
        private void Awake()
        {
            bt_Play.interactable = false;
            bt_Pause.interactable = false;
            bt_Reset.interactable = false;
            videoPlayer = youtubePlayer.GetComponent<VideoPlayer>();
            videoPlayer.prepareCompleted += VideoPlayerPreparedCompleted;
        }

        void VideoPlayerPreparedCompleted( VideoPlayer source)
        {
            bt_Play.interactable = source.isPrepared;
            bt_Pause.interactable = source.isPrepared;
            bt_Reset.interactable = source.isPrepared;
        }
        public async void Prepare()
        {
            print("�ҷ�������...");
            try
            {
                await youtubePlayer.PrepareVideoAsync();
                print("�ҷ����⸦ ����");
            }
            catch
            {
                print("����!����!");
            }

        }
         public void PlayVideo()
        {
            videoPlayer.Play();
        }
        public void PauseVideo()
        {
            videoPlayer.Pause();
        }
        public void ResetVideo()
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
   
        private void OnDestroy()
        {
            videoPlayer.prepareCompleted -= VideoPlayerPreparedCompleted;
        }
    }

}


