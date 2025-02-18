// using Unity.Netcode;
//
// // Can be uncommented when an Assembly Definition file is referenced, that includes the Unity.Netcode namespace.
// namespace RogueDeck.MomSesImSpcl.Extensions.NGO
// {
//     /// <summary>
//     /// Contains extension methods for <see cref="Unity.Netcode.RpcTarget"/>.
//     /// </summary>
//     public static class RpcTargetExtensions
//     {
//         #region Methods
//         /// <summary>
//         /// Excludes the <see cref="NetworkBehaviour.OwnerClientId"/> of the server <c>(0)</c>, and the <see cref="NetworkBehaviour.OwnerClientId"/> of the sender of the given <see cref="RpcParams"/>.
//         /// </summary>
//         /// <param name="_RpcTarget"><see cref="RpcTarget"/>.</param>
//         /// <param name="_ExcludedClientIds">The <see cref="NetworkBehaviour.OwnerClientId"/>s to exclude from the <see cref="RpcTarget"/>.</param>
//         /// <returns>A <see cref="BaseRpcTarget"/> from <see cref="RpcTarget.Not(ulong[],RpcTargetUse)"/>, with the excluded <see cref="NetworkBehaviour.OwnerClientId"/>s.</returns>
//         public static BaseRpcTarget ExcludeServerAndSender(this RpcTarget _RpcTarget, ulong[] _ExcludedClientIds)
//         {
//             return _RpcTarget.Not(_ExcludedClientIds, RpcTargetUse.Temp);
//         }
//         #endregion
//     }
// }
