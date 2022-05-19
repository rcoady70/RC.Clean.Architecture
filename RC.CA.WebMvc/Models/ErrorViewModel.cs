namespace RC.CA.WebUiMvc.Models
{
    public class ErrorViewModel
    {

        public string? RequestId { get; set; } = default!;
        public string? UserName { get; set; } = default!;
        public string? ApiRoute { get; set; } = default!;
        public string? ApiStatus { get; set; } = default!;
        public string CorrelationId { get; set; } // exception shielding from server to client
        public string DelevoperMessage { get; set; }
        public string DelevoperStackTrace { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
