using OONLFWK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OONLFWK.Infrastructure
{
    public interface ICurrentUser
    {
        ApplicationUser User { get; }
    }
}