using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    internal static class IdentityErrors
    {
        internal const string UserOrPasswordInvalid = "Usuario y/o contraseña incorrecta.";
        internal const string NoRol = "Usuario no tiene rol asignado.";
    }
}
