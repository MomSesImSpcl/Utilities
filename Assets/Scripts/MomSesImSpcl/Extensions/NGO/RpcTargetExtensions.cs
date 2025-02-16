// using MomSesImSpcl.Utilities.Pooling;
// using RogueDeck.Networking;
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
//         /// <param name="_RPCParams">The <see cref="RpcParams"/> of the sender, whose <see cref="NetworkBehaviour.OwnerClientId"/> to exclude.</param>
//         /// <param name="_ClientIdWrapper">
//         /// <see cref="ClientIdWrapper"/>. <br/>
//         /// <i>Should be returned to the <see cref="ObjectPool{T}"/> when no longer needed.</i>
//         /// </param>
//         /// <returns>A <see cref="BaseRpcTarget"/> from <see cref="RpcTarget.Not(ulong[],RpcTargetUse)"/>, with the excluded <see cref="NetworkBehaviour.OwnerClientId"/>s.</returns>
//         public static BaseRpcTarget ExcludeServerAndSender(this RpcTarget _RpcTarget, RpcParams _RPCParams, out ClientIdWrapper _ClientIdWrapper)
//         {
//             _ClientIdWrapper = CustomNetworkManager.ClientIdPool.Get();
//             
//             return _RpcTarget.Not(_ClientIdWrapper.SetId(_RPCParams.Receive.SenderClientId), RpcTargetUse.Temp);
//         }
//         #endregion
//     }
// }
