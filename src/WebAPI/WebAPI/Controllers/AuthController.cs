using Application.Common.Dtos.Auth;
using Application.Common.Interfaces;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Auth([FromBody] AuthRequest request)
    {
        return Ok(await _identityService.Authenticate(request));
    }
}
