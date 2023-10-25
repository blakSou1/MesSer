namespace MesengerServer
{
    public class User
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Groups> Group { get; set; }
        public List<Groups> winGroup { get; set; }
        public User(string id, string name, string password)
        {
            ID = id;
            UserName = name;
            Password = password;
            Group = new List<Groups>();
            winGroup = new List<Groups>();
        }
    }
}
