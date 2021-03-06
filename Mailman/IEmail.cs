using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace Mailman.Net.Smtp
{
    public interface IEmail
    {
       IEnumerable<string> Recipients { get; set; } 
       string Subject { get; set; }
       string Message { get; set; }
       bool IsHtml { get; set; }
       ValidationResult Validation { get; }
    }
}