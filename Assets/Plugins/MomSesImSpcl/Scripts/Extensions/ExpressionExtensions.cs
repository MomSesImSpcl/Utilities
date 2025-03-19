using System;
using System.Linq.Expressions;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Expression{TDelegate}"/>.
    /// </summary>
    public static class ExpressionExtensions
    {
        #region Methods
        /// <summary>
        /// Gets the name of a member inside <see cref="Type"/> <c>T</c>.
        /// </summary>
        /// <param name="_Member">Must be a field of property.</param>
        /// <typeparam name="T">The <see cref="Type"/> that holds the member of <see cref="Type"/> <c>V</c>.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the member to get the name of.</typeparam>
        /// <returns>The name of the member inside <see cref="Type"/> <c>T</c>.</returns>
        /// <exception cref="ArgumentException">When the member of <see cref="Type"/> <c>V</c> is not a field or property.</exception>
        public static string GetMemberName<T,V>(this Expression<Func<T,V>> _Member) where T : notnull
        {
            if (_Member.Body is MemberExpression _memberExpression)
            {
                return _memberExpression.Member.Name;
            }
            
            throw new ArgumentException($"The given Expression [{_Member.ToString().Bold()}] is not a Field or Property.");
        }
        #endregion
    }
}