namespace MesengerServer
{
    public static class GenerateId
    {
        public static string GenerateID()
        {
            var guidBytes = Guid.NewGuid().ToByteArray();
            var base64 = Convert.ToBase64String(guidBytes);
            base64 = base64.Replace("/", string.Empty).Replace("+", string.Empty);
            return base64.Substring(0, 8);
        }
    }
}
