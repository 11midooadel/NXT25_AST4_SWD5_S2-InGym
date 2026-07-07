using System.ComponentModel.DataAnnotations;
namespace NXT25_AST4_SWD5_S2_InGym.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
