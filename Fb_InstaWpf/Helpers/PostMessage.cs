namespace Fb_InstaWpf
{
    public class PostMessage
    {
        public string FromUserId { get; set; }

        public string Message { get; set; }

        public MessageType MessageType { get; set; }

        public string ImagePath { get; set; }
        public string ToUserId { get; internal set; }
        public int Status { get; set; }
        public string ToUrl { get; set; }
       

        
    }
}
