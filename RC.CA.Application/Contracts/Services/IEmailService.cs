﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Contracts.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync();
    }
}
