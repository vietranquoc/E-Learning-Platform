using ELearningPlatform_Data.Infracstructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Domain.Handler
{
    public abstract class BaseHandler
    {
        private readonly IHttpContextAccessor _httpContext;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseHandler(IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _httpContext = httpContext;
            _unitOfWork = unitOfWork;
        }
    }
}
