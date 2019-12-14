using Api.LiveHub.Domain.Models;
using System;

namespace Api.LiveHub.ViewModel
{
    [Serializable]
    public class AccountViewModel
    {
        public long Id { get; set; }
        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public RoleViewModel Role { get; set; }
        public string OpenId { get; set; }

        public string NickName { get; set; }

        public string AvatarUrl { get; set; }
        public string DepartmentNo { get; set; }

        public string Password { get; set; }

        public AccountStatus Status { get; set; }
        public string RoleName { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        
    }
}
