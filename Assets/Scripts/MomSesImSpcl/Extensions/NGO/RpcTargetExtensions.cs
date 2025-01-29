using Unity.Netcode;

namespace MomSesImSpcl.Extensions.NGO
{
    /// <summary>
    /// Contains extension methods for <see cref="Unity.Netcode.RpcTarget"/>.
    /// </summary>
    public static class RpcTargetExtensions
    {
        #region Methods
        /// <summary>
        /// Excludes the <see cref="NetworkBehaviour.OwnerClientId"/> of the server <c>(0)</c>, and the <see cref="NetworkBehaviour.OwnerClientId"/> of the sender of the given <see cref="RpcParams"/>.
        /// </summary>
        /// <param name="_RpcTarget"><see cref="RpcTarget"/>.</param>
        /// <param name="_RPCParams">The <see cref="RpcParams"/> of the sender, whose <see cref="NetworkBehaviour.OwnerClientId"/> to exclude.</param>
        /// <returns>A <see cref="BaseRpcTarget"/> from <see cref="RpcTarget.Not(ulong[],RpcTargetUse)"/>, with the excluded <see cref="NetworkBehaviour.OwnerClientId"/>s.</returns>
        public static BaseRpcTarget ExcludeServerAndSender(this RpcTarget _RpcTarget, RpcParams _RPCParams)
        {
            var _excludedOwnerClientIds = new ulong[] { 0, _RPCParams.Receive.SenderClientId };

            return _RpcTarget.Not(_excludedOwnerClientIds, RpcTargetUse.Temp);
        }
        #endregion
    }
}