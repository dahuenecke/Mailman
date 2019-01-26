using System;
using System.Collections.Generic;

namespace Mailman
{
    public interface IEmail
    {
       IEnumerable<string> Recipients { get; set; } 
       string Subject { get; set; }
       string Message { get; set; }
    }
}