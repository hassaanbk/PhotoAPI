﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace PhotoAPI.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : Controller
    {
        protected readonly ILogger<T> _logger;
        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}

