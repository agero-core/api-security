using Agero.Core.Checker;
using System;

namespace Agero.Core.Api.Security.Tests.Helpers
{
    public static class ExceptionHelper
    {
        public static void Assert<TException>(Action action, Func<TException, bool> isValid = null)
            where TException : Exception
        {
            Check.ArgumentIsNull(action, "action");

            try
            {
                action();
            }
            catch (TException ex)
            {
                if (isValid != null && !isValid(ex))
                    throw;
            }
        }
    }
}
