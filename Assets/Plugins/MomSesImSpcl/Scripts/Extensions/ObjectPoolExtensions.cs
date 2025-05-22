using UnityEngine;
using UnityEngine.Pool;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="ObjectPool{T}"/>.
    /// </summary>
    public static class ObjectPoolExtensions
    {
        #region Methods
        public static async
#if UNITASK
            Cysharp.Threading.Tasks.UniTask
#else
            System.Threading.Tasks.Task
#endif
        PreloadAsync<T>(this ObjectPool<T> _ObjectPool, T _Prefab, int _Count, Transform _Parent, Vector3 _Position, Quaternion _Rotation) where T : MonoBehaviour
        {
            foreach (var _card in await Object.InstantiateAsync(_Prefab, _Count, _Parent, _Position, _Rotation))
            {
                _ObjectPool.Release(_card);
            }
        }
        #endregion
    }
}