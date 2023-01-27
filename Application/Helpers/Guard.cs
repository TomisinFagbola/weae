using Domain.Entities.Identities;
using Domain.Enums;
using Application.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class Guard
    {
        public static void AgainstNull<T>(T value, HttpStatusCode code = HttpStatusCode.NotFound, string message = null)
        {
            if (value == null)
                throw new RestException(code, $"{typeof(T).Name} not found", message);
        }
        public static void AgainstDuplicate<T>(T value, string message = null, HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            if (value != null)
                throw new RestException(code, $"Duplicate {typeof(T).Name}", message);
        }

        public static void AgainstFailedTransaction(bool value, HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
            if (value == false)
                throw new RestException(code, "Internal server error.");
        }
    }
}