namespace MesengerServer
{
    public class Groups
    {
        public string groupName { get; set; }
        public string ID { get; set; }
        public string Password { get; set; }
        public List<Chats> chats { get; set; }

        public Groups(string name, string password)
        {
            groupName = name;
            Password = password;
            chats = new List<Chats>();
            ID = GenerateId.GenerateID();
        }
    }
}
