namespace ZS.MultiSocialPost
{
    public interface IMultiSocialPost
    {
        public Task PostVideo(string videoFilePath, string accessToken);

    }
}