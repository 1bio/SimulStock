using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace YoutubePlayer
{
   
    [RequireComponent(typeof(VideoPlayer))]
    public class YoutubePlayer : MonoBehaviour
    {
       
        static readonly string[] k_PlayFields = { "url" };
        static readonly string[] k_DownloadFields = { "title", "_filename" , "ext", "url" };
        public string youtubeUrl;
        public bool is360Video;
        public VideoPlayer VideoPlayer { get; private set; }

        void Awake()
        {
            VideoPlayer = GetComponent<VideoPlayer>();
            youtubeUrl = "https://www.youtube.com/watch?v=YZtorB71H_M";

        }
      

        async void OnEnable()
        {
            if (VideoPlayer.playOnAwake)
                await PlayVideoAsync();
        }

       
        public static async Task<string> GetRawVideoUrlAsync(string videoUrl, YoutubeDlOptions options = null, CancellationToken cancellationToken = default)
        {
            options = options ?? YoutubeDlOptions.Default;
            var metaData = await YoutubeDl.GetVideoMetaDataAsync<YoutubeVideoMetaData>(videoUrl, options, k_PlayFields, cancellationToken);
            return metaData.Url;
        }

       
        public async Task PrepareVideoAsync(string videoUrl = null, YoutubeDlOptions options = null, CancellationToken cancellationToken = default)
        {
            videoUrl = videoUrl ?? youtubeUrl;
            options = options ?? (is360Video ? YoutubeDlOptions.Three60 : YoutubeDlOptions.Default);
            var rawUrl = await GetRawVideoUrlAsync(videoUrl, options, cancellationToken);

            VideoPlayer.source = VideoSource.Url;

      
            if (VideoPlayer.url != rawUrl)
                VideoPlayer.url = rawUrl;

            youtubeUrl = videoUrl;

            await VideoPlayer.PrepareAsync(cancellationToken);
        }

      
        public async Task PlayVideoAsync(string videoUrl = null, YoutubeDlOptions options = null, CancellationToken cancellationToken = default)
        {
            options = options ?? (is360Video ? YoutubeDlOptions.Three60 : YoutubeDlOptions.Default);
            await PrepareVideoAsync(videoUrl, options, cancellationToken);
       
        }

        
        public async Task<string> DownloadVideoAsync(string destinationFolder = null, string videoUrl = null, CancellationToken cancellationToken = default)
        {
            videoUrl = videoUrl ?? youtubeUrl;

            var video = await YoutubeDl.GetVideoMetaDataAsync<YoutubeVideoMetaData>(videoUrl, k_DownloadFields, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var fileName = GetVideoFileName(video);

            var filePath = fileName;
            if (!string.IsNullOrEmpty(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
                filePath = Path.Combine(destinationFolder, fileName);
            }

            await YoutubeDownloader.DownloadAsync(video, filePath, cancellationToken);
            return filePath;
        }

        static string GetVideoFileName(YoutubeVideoMetaData video)
        {
            if (!string.IsNullOrWhiteSpace(video.FileName))
            {
                return video.FileName;
            }

            var fileName = $"{video.Title}.{video.Extension}";

            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar.ToString(), "_");
            }

            return fileName;
        }
    }
}
