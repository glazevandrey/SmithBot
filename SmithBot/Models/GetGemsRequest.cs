namespace SmithBot.Models
{
    public class GetGemsRequest
    {
        public string operationName { get; set; }
        public Variables variables { get; set; }
        public string query { get; set; }
    }

    public class Variables
    {
        public string query { get; set; }
        public object attributes { get; set; }
        public string sort { get; set; }
        public int count { get; set; }
        public string cursor { get; set; }

    }


}
