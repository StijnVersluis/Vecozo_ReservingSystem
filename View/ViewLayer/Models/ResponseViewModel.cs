namespace ViewLayer.Models
{
    public class ResponseViewModel
    {
        public bool Success { get; private set; }

        public string Name { get; private set; }

        public string Message { get; private set; }

        public ResponseViewModel(bool success, string name, string message)
        {
            Success = success;
            Name = name;
            Message = message;
        }
    }
}
