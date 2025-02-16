#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MomSesImSpcl.Utilities.Logging;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods
        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text bold tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in a bold tag.</returns>
        public static string Bold(this object? _Object)
        {
            return $"<b>{_Object.OrNull()}</b>";
        }
        
        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text color tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <param name="_Color">The <see cref="RichTextColor"/> to wrap the <see cref="object.ToString"/>-output with.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in the specified color tag.</returns>
        public static string Color(this object? _Object, RichTextColor _Color)
        {
            return $"<color={_Color}>{_Object.OrNull()}</color>";
        }

        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text italic tag.
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in an italic tag.</returns>
        public static string Italic(this object? _Object)
        {
            return $"<i>{_Object.OrNull()}</i>";
        }

        /// <summary>
        /// Returns the string representation of the object, or "null" if the object is null.
        /// </summary>
        /// <param name="_Object">The object to get the string representation of.</param>
        /// <returns>A string representing the object, or "null" if the object is null.</returns>
        public static string OrNull(this object? _Object)
        {
            return _Object?.ToString() ?? "null";
        }

        /// <summary>
        /// Sets the value of a Field through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Field.</param>
        /// <param name="_Field">The Field whose value to set.</param>
        /// <param name="_Value">The value to set to the Field.</param>
        /// <param name="_BindingFlags">Optional <see cref="BindingFlags"/> to find the Field.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/> that holds the Field.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the Field.</typeparam>
        public static void SetFieldValue<T,V>(this T _Instance, Expression<Func<T,V>> _Field, V _Value, BindingFlags _BindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
        {
            _Instance.SetMemberValue(_Field, (_Type, _FieldName) => _Type.GetField(_FieldName, _BindingFlags), _FieldInfo =>
            {
                _FieldInfo.SetValue(_Instance, _Value);
                
                // ReSharper disable once VariableHidesOuterVariable
            }, (_Type, _BindingFlags) => _Type.GetFields(_BindingFlags));
        }
        
        /// <summary>
        /// Sets the value of a Property through reflection.
        /// </summary>
        /// <param name="_Instance">The instance of the <see cref="object"/> that holds the Property.</param>
        /// <param name="_Property">The Property whose value to set.</param>
        /// <param name="_Value">The value to set to the Property.</param>
        /// <param name="_BindingFlags">Optional <see cref="BindingFlags"/> to find the Property.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/> that holds the Property.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the Property.</typeparam>
        public static void SetPropertyValue<T,V>(this T _Instance, Expression<Func<T,V>> _Property, V _Value, BindingFlags _BindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
        {
            _Instance.SetMemberValue(_Property, (_Type, _PropertyName) => _Type.GetProperty(_PropertyName, _BindingFlags), _PropertyInfo =>
            {
                _PropertyInfo.SetValue(_Instance, _Value);
                
                // ReSharper disable once VariableHidesOuterVariable
            }, (_Type, _BindingFlags) => _Type.GetProperties(_BindingFlags));
        }
        
        /// <summary>
        /// Sets the value of a member through reflection.
        /// </summary>
        /// <param name="_">The instance of the <see cref="object"/> that holds the member.</param>
        /// <param name="_Member">The member whose value to set.</param>
        /// <param name="_GetMethod">Should be <see cref="Type.GetField(string,BindingFlags)"/> or <see cref="Type.GetProperty(string,BindingFlags)"/>.</param>
        /// <param name="_SetMethod">Should be <see cref="FieldInfo"/>.<see cref="FieldInfo.SetValue(object,object)"/> or <see cref="PropertyInfo"/>.<see cref="PropertyInfo.SetValue(object,object)"/>.</param>
        /// <param name="_FallbackMembers">
        /// In case the Field/Property could not be found, this will print all Fields/Properties of the given <see cref="Type"/>. <br/>
        /// <i>Should be <see cref="Type.GetFields(BindingFlags)"/> or <see cref="Type.GetProperties(BindingFlags)"/>.</i>
        /// </param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="object"/> that holds the member.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the member.</typeparam>
        /// <typeparam name="I">The concrete <see cref="Type"/> of the <see cref="MemberInfo"/>.</typeparam>
        private static void SetMemberValue<T,V,I>(this T _, Expression<Func<T,V>> _Member, Func<Type,string,I> _GetMethod, Action<I> _SetMethod, Func<Type,BindingFlags,I[]> _FallbackMembers) where I : MemberInfo
        {
            if (_Member.Body is MemberExpression _memberExpression)
            {
                var _type = typeof(T);
                
                if (_GetMethod(_type, _memberExpression.Member.Name) is {} _memberInfo)
                {
                    _SetMethod(_memberInfo);
                }
                else
                {
                    // ReSharper disable once VariableHidesOuterVariable
                    var _members = _FallbackMembers(_type, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Select(_MemberInfo => _MemberInfo.Name);
                    var _memberType = nameof(I).Replace("Info", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                    var _memberName = _memberExpression.Member.Name.Bold();
                    var _className = nameof(T).Bold();
                    
                    Debug.LogError($"Could not find the {_memberType} [{_memberName}] in Class [{_className}].{Environment.NewLine}Here is every {_memberType} in Class [{_className}]:{Environment.NewLine}[{string.Join(Environment.NewLine, _members)}]");
                }
            }
            else
            {
                Debug.LogError($"The given Expression [{_Member.Bold()}] is not a Field or Property.");
            }
        }
        
        /// <summary>
        /// Wraps this object's <see cref="object.ToString"/>-output in a Rich Text underline tag. <br/>
        /// <i>Doesn't work in the default console, but works with TextMeshPro.</i>
        /// </summary>
        /// <param name="_Object">The <see cref="object"/> to get the <see cref="object.ToString"/>-output of.</param>
        /// <returns>A <see cref="string"/> containing this object's <see cref="object.ToString"/>-output, wrapped in an underline tag.</returns>
        public static string Underline(this object? _Object)
        {
            return $"<u>{_Object.OrNull()}</u>";
        }
        
        /// <summary>
        /// Converts the string representation of the specified object to a MemoryStream, using the provided encoding.
        /// </summary>
        /// <param name="_Object">The object whose string representation will be converted to a MemoryStream.</param>
        /// <param name="_Encoding">The encoding to use for the conversion.</param>
        /// <returns>A MemoryStream containing the encoded string representation of the specified object.</returns>
        public static MemoryStream ToMemoryStream(this object? _Object, Encoding _Encoding)
        {
            return new MemoryStream(_Encoding.GetBytes(_Object?.ToString()?.OrNull()!));
        }
        #endregion
    }
}
