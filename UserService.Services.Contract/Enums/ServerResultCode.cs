using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services.Contract.Enums
{
    public enum ServerResultCode
    {
        None,
        InvalidEmail,
        InvalidPassword,
        UserNameIsRequired,
        InvalidRoleSelected,
        UserSuccessfullyCreated,
        UserWithSuchEmailExists,
    }
}
