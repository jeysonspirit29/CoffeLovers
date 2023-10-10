using Application.Common.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AuthResponse> Authenticate(AuthRequest request);
    Task<ICollection<string>> GetUserIdsBySuperior(string chiefUserId);

}
