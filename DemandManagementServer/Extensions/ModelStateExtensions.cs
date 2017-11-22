using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DemandManagementServer.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetErrorMessage(this ModelStateDictionary modelState )
        {
            foreach (var item in modelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    return item.Errors[0].ErrorMessage;
                }
            }
            return "";
        }
    }
}
