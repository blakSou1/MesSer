namespace MesengerServer
{
    public class Message
    {
        public string messag { get; set; }
        public User player { get; set; }
        public DateTime Time { get; set; }

        public Message(string text, User pl)
        {
            Time = DateTime.Now;
            player = pl;
            messag = text;
        }
    }
}
