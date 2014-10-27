﻿using Tokens.Exceptions;
using Tokens.Operators;

namespace Tokens
{
    /// <summary>
    /// Factory class to create <see cref="ITokenOperator"/> objects.
    /// </summary>
    public class TokenOperatorFactory : BaseTokenFactory<ITokenOperator>
    {
        /// <summary>
        /// Performs an operation on the given value.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string PerformOperation(Token token, string value)
        {
            if (string.IsNullOrEmpty(token.Operation))
            {
                return value;
            }

            if (!token.Operation.StartsWith("To"))
            {
                return value;
            }

            var fired = false;

            foreach (var @operator in Items)
            {
                if (!CanPerform(@operator, token)) continue;

                value = @operator.Perform(token, value);
                    
                fired = true;

                break;
            }

            if (!fired)
            {
                var message = "Can't perform operation: " + token.Operation;

                throw new MissingOperationException(message);
            }

            return value;
        }

        private bool CanPerform(ITokenOperator @operator, Token token)
        {
            var name = @operator.GetType().Name.Replace("Operator", string.Empty) + "(";

            return token.Operation.StartsWith(name);
        }
    }
}
