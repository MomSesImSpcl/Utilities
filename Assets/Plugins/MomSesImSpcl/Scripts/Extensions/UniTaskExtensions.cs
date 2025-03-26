// using System;
// using Cysharp.Threading.Tasks;
//
// namespace MomSesImSpcl.Extensions
// {
//     /// <summary>
//     /// Contains extension methods for <see cref="UniTask"/>.
//     /// </summary>
//     public static class UniTaskExtensions
//     {
//         #region Methods
//         /// <summary>
//         /// Returns the result of a <see cref="UniTask{T}"/> without awaiting it.
//         /// </summary>
//         /// <param name="_UniTask">The <see cref="UniTask{T}"/> to get the result from.</param>
//         /// <typeparam name="T">The <see cref="Type"/> of the result.</typeparam>
//         /// <returns>The result of this <see cref="UniTask{T}"/>.</returns>
//         public static T GetResult<T>(this UniTask<T> _UniTask)
//         {
//             return _UniTask.GetAwaiter().GetResult();
//         }
//         #endregion
//     }
// }