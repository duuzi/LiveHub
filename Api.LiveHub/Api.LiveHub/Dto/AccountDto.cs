using Api.LiveHub.Domain.Models;

namespace Api.LiveHub.Dto
{
    public class AccountDto
    {
        public string OpenId { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string NickName { get; set; }
        public string DepartmentNo { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public AccountStatus Status { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAddress { get; set; }
    }
}
