using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos.Auth;

public class IdentityResponse
{
    internal IdentityResponse(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public static IdentityResponse Success()
    {
        return new IdentityResponse(true, Array.Empty<string>());
    }

    public static IdentityResponse Failure(IEnumerable<string> errors)
    {
        return new IdentityResponse(false, errors);
    }
}


