using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos.Auth
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Rol { get; set; }
        public string Message { get; set; }

        public string Token { get; set; }
    }
}
