using Microsoft.Extensions.Options;

namespace ZS.MultiSocialPost;

public class TikTokPublish : IMultiSocialPost
{
    public TikTokPublish(IOptions<TikTokOptions> options)
    {
        Options = options;
    }

    public IOptions<TikTokOptions> Options { get; }

    // Step 5: Post the video on TikTok
    public async Task PostVideo(string videoFilePath, string accessToken)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://open-api.tiktok.com/post?access_token={accessToken}";

            // Read the video file as bytes
            byte[] videoBytes = File.ReadAllBytes(videoFilePath);

            // Prepare multipart form data for video post
            var formData = new MultipartFormDataContent();
            formData.Add(new ByteArrayContent(videoBytes), "video", "video.mp4");

            // Send video post request
            var response = await client.PostAsync(url, formData);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Process the response to check if the video was successfully posted
            dynamic jsonResponse = new { };//JsonConvert.DeserializeObject(responseContent);
            bool isVideoPosted = jsonResponse.data.is_posted;

            if (!isVideoPosted)
            {
                string error = jsonResponse.data.description;
                throw new Exception($"Failed to post video: {error}");
            }
        }
    }
}
