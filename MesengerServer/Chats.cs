namespace MesengerServer
{
    public class Chats
    {
        public List<Message> message { get; set; }
        public string Name { get; set; }

        public Chats(string name)
        {
            Name = name;
            message = new List<Message>();
        }
    }
}
