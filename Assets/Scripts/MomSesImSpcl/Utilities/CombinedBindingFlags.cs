using System;
using System.Reflection;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Combines multiple <see cref="BindingFlags"/>.
    /// </summary>
    [Flags]
    public enum CombinedBindingFlags
    {
        /// <summary>
        /// <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.Instance"/>.
        /// </summary>
        PublicInstance = BindingFlags.Public | BindingFlags.Instance,
        /// <summary>
        /// <see cref="PublicInstance"/> | <see cref="BindingFlags.IgnoreCase"/>.
        /// </summary>
        PublicInstanceIgnoreCase = PublicInstance | BindingFlags.IgnoreCase,
        /// <summary>
        /// <see cref="PublicInstance"/> | <see cref="BindingFlags.DeclaredOnly"/>.
        /// </summary>
        PublicInstanceDeclared = PublicInstance | BindingFlags.DeclaredOnly,
        /// <summary>
        /// <see cref="PublicInstanceDeclared"/> | <see cref="PublicInstanceIgnoreCase"/>.
        /// </summary>
        PublicInstanceDeclaredIgnoreCase = PublicInstanceDeclared | PublicInstanceIgnoreCase,
        /// <summary>
        /// <see cref="PublicInstance"/> | <see cref="BindingFlags.FlattenHierarchy"/>.
        /// </summary>
        PublicInstanceFlattened = PublicInstance | BindingFlags.FlattenHierarchy,
        /// <summary>
        /// <see cref="PublicInstanceFlattened"/> | <see cref="PublicInstanceIgnoreCase"/>.
        /// </summary>
        PublicInstanceFlattenedIgnoreCase = PublicInstanceFlattened | PublicInstanceIgnoreCase,
        /// <summary>
        /// <see cref="BindingFlags.NonPublic"/> | <see cref="BindingFlags.Instance"/>.
        /// </summary>
        NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance,
        /// <summary>
        /// <see cref="NonPublicInstance"/> | <see cref="BindingFlags.IgnoreCase"/>.
        /// </summary>
        NonPublicInstanceIgnoreCase = NonPublicInstance | BindingFlags.IgnoreCase,
        /// <summary>
        /// <see cref="NonPublicInstance"/> | <see cref="BindingFlags.DeclaredOnly"/>.
        /// </summary>
        NonPublicInstanceDeclared = NonPublicInstance | BindingFlags.DeclaredOnly,
        /// <summary>
        /// <see cref="NonPublicInstanceDeclared"/> | <see cref="NonPublicInstanceIgnoreCase"/>.
        /// </summary>
        NonPublicInstanceDeclaredIgnoreCase = NonPublicInstanceDeclared | NonPublicInstanceIgnoreCase,
        /// <summary>
        /// <see cref="NonPublicInstance"/> | <see cref="BindingFlags.FlattenHierarchy"/>.
        /// </summary>
        NonPublicInstanceFlattened = NonPublicInstance | BindingFlags.FlattenHierarchy,
        /// <summary>
        /// <see cref="NonPublicInstance"/> | <see cref="NonPublicInstanceIgnoreCase"/>.
        /// </summary>
        NonPublicInstanceFlattenedIgnoreCase = NonPublicInstance | NonPublicInstanceIgnoreCase,
        /// <summary>
        /// <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.Static"/>.
        /// </summary>
        PublicStatic = BindingFlags.Public | BindingFlags.Static,
        /// <summary>
        /// <see cref="PublicStatic"/> | <see cref="BindingFlags.IgnoreCase"/>.
        /// </summary>
        PublicStaticIgnoreCase = PublicStatic | BindingFlags.IgnoreCase,
        /// <summary>
        /// <see cref="PublicStatic"/> | <see cref="BindingFlags.DeclaredOnly"/>.
        /// </summary>
        PublicStaticDeclared = PublicStatic | BindingFlags.DeclaredOnly,
        /// <summary>
        /// <see cref="PublicStaticDeclared"/> | <see cref="PublicStaticIgnoreCase"/>.
        /// </summary>
        PublicStaticDeclaredIgnoreCase = PublicStaticDeclared | PublicStaticIgnoreCase,
        /// <summary>
        /// <see cref="PublicStatic"/> | <see cref="BindingFlags.FlattenHierarchy"/>.
        /// </summary>
        PublicStaticFlattened = PublicStatic | BindingFlags.FlattenHierarchy,
        /// <summary>
        /// <see cref="PublicStaticFlattened"/> | <see cref="PublicStaticIgnoreCase"/>.
        /// </summary>
        PublicStaticFlattenedIgnoreCase = PublicStaticFlattened | PublicStaticIgnoreCase,
        /// <summary>
        /// <see cref="BindingFlags.NonPublic"/> | <see cref="BindingFlags.Static"/>.
        /// </summary>
        NonPublicStatic = BindingFlags.NonPublic | BindingFlags.Static,
        /// <summary>
        /// <see cref="NonPublicStatic"/> | <see cref="BindingFlags.IgnoreCase"/>.
        /// </summary>
        NonPublicStaticIgnoreCase = NonPublicStatic | BindingFlags.IgnoreCase,
        /// <summary>
        /// <see cref="NonPublicStatic"/> | <see cref="BindingFlags.DeclaredOnly"/>.
        /// </summary>
        NonPublicStaticDeclared = NonPublicStatic | BindingFlags.DeclaredOnly,
        /// <summary>
        /// <see cref="NonPublicStaticDeclared"/> | <see cref="NonPublicStaticIgnoreCase"/>.
        /// </summary>
        NonPublicStaticDeclaredIgnoreCase = NonPublicStaticDeclared | NonPublicStaticIgnoreCase,
        /// <summary>
        /// <see cref="NonPublicStatic"/> | <see cref="BindingFlags.FlattenHierarchy"/>.
        /// </summary>
        NonPublicStaticFlattened = NonPublicStatic | BindingFlags.FlattenHierarchy,
        /// <summary>
        /// <see cref="NonPublicStaticFlattened"/> | <see cref="NonPublicStaticIgnoreCase"/>.
        /// </summary>
        NonPublicStaticFlattenedIgnoreCase = NonPublicStaticFlattened | NonPublicStaticIgnoreCase,
        /// <summary>
        /// <see cref="PublicInstance"/> | <see cref="NonPublicInstance"/>.
        /// </summary>
        AllInstance = PublicInstance | NonPublicInstance,
        /// <summary>
        /// <see cref="PublicInstanceIgnoreCase"/> | <see cref="NonPublicInstanceIgnoreCase"/>.
        /// </summary>
        AllInstanceIgnoreCase = PublicInstanceIgnoreCase | NonPublicInstanceIgnoreCase,
        /// <summary>
        /// <see cref="PublicInstanceDeclared"/> | <see cref="NonPublicInstanceDeclared"/>.
        /// </summary>
        AllInstanceDeclared = PublicInstanceDeclared | NonPublicInstanceDeclared,
        /// <summary>
        /// <see cref="PublicInstanceDeclaredIgnoreCase"/> | <see cref="NonPublicInstanceDeclaredIgnoreCase"/>.
        /// </summary>
        AllInstanceDeclaredIgnoreCase = PublicInstanceDeclaredIgnoreCase | NonPublicInstanceDeclaredIgnoreCase,
        /// <summary>
        /// <see cref="PublicInstanceFlattened"/> | <see cref="NonPublicInstanceFlattened"/>.
        /// </summary>
        AllInstanceFlattened = PublicInstanceFlattened | NonPublicInstanceFlattened,
        /// <summary>
        /// <see cref="PublicInstanceFlattenedIgnoreCase"/> | <see cref="NonPublicInstanceFlattenedIgnoreCase"/>.
        /// </summary>
        AllInstanceFlattenedIgnoreCase = PublicInstanceFlattenedIgnoreCase | NonPublicInstanceFlattenedIgnoreCase,
        /// <summary>
        /// <see cref="PublicStatic"/> | <see cref="NonPublicStatic"/>.
        /// </summary>
        AllStatic = PublicStatic | NonPublicStatic,
        /// <summary>
        /// <see cref="PublicStaticIgnoreCase"/> | <see cref="NonPublicStaticIgnoreCase"/>.
        /// </summary>
        AllStaticIgnoreCase = PublicStaticIgnoreCase | NonPublicStaticIgnoreCase,
        /// <summary>
        /// <see cref="PublicStaticDeclared"/> | <see cref="NonPublicStaticDeclared"/>.
        /// </summary>
        AllStaticDeclared = PublicStaticDeclared | NonPublicStaticDeclared,
        /// <summary>
        /// <see cref="PublicStaticDeclaredIgnoreCase"/> | <see cref="NonPublicStaticDeclaredIgnoreCase"/>.
        /// </summary>
        AllStaticDeclaredIgnoreCase = PublicStaticDeclaredIgnoreCase | NonPublicStaticDeclaredIgnoreCase,
        /// <summary>
        /// <see cref="PublicStaticFlattened"/> | <see cref="NonPublicStaticFlattened"/>.
        /// </summary>
        AllStaticFlattened = PublicStaticFlattened | NonPublicStaticFlattened,
        /// <summary>
        /// <see cref="PublicStaticFlattenedIgnoreCase"/> | <see cref="NonPublicStaticFlattenedIgnoreCase"/>.
        /// </summary>
        AllStaticFlattenedIgnoreCase = PublicStaticFlattenedIgnoreCase | NonPublicStaticFlattenedIgnoreCase,
        /// <summary>
        /// <see cref="AllInstanceDeclared"/> | <see cref="AllStaticDeclared"/>.
        /// </summary>
        AllDeclared = AllInstanceDeclared | AllStaticDeclared,
        /// <summary>
        /// <see cref="AllInstanceDeclaredIgnoreCase"/> | <see cref="AllStaticDeclaredIgnoreCase"/>.
        /// </summary>
        AllInstDeclaredIgnoreCase = AllInstanceDeclaredIgnoreCase | AllStaticDeclaredIgnoreCase,
        /// <summary>
        /// <see cref="AllInstanceFlattened"/> | <see cref="AllStaticFlattened"/>.
        /// </summary>
        All = AllInstanceFlattened | AllStaticFlattened,
        /// <summary>
        /// <see cref="AllInstanceIgnoreCase"/> | <see cref="AllStaticFlattenedIgnoreCase"/>.
        /// </summary>
        AllIgnoreCase = AllInstanceIgnoreCase | AllStaticFlattenedIgnoreCase
    }
}
