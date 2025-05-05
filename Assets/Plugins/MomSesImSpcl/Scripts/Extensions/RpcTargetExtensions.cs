#if NETCODE
using System;
using MomSesImSpcl.Utilities.Pooling;
using Unity.Netcode;

// Can be uncommented when an Assembly Definition file is referenced, that includes the Unity.Netcode namespace.
namespace MomSesImSpcl.Extensions
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
        /// <param name="_ExcludedClientIds">
        /// The <see cref="Array"/> that was used to exclude the <see cref="NetworkBehaviour.OwnerClientId"/>s. <br/>
        /// <i>Should be returned back to the <see cref="ArrayPool{T}"/>.</i>
        /// </param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayPool{T}.ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="ArrayPool{T}.LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="ArrayPool{T}.LogArrayBuckets"/> is set to <c>true</c>.</param>
        /// <returns>A <see cref="BaseRpcTarget"/> from <see cref="RpcTarget.Not(ulong[],RpcTargetUse)"/>, with the excluded <see cref="NetworkBehaviour.OwnerClientId"/>s.</returns>
        public static BaseRpcTarget ExcludeServerAndSender(this RpcTarget _RpcTarget, RpcParams _RPCParams, out ulong[] _ExcludedClientIds, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            _ExcludedClientIds = ArrayPool<ulong>.Get(2, 0, _RPCParams.Receive.SenderClientId, _LogArrayBucket, _ForceStopLogging);
            return _RpcTarget.Not(_ExcludedClientIds, RpcTargetUse.Temp);
        }
        #endregion
    }
}
#endif