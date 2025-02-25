#if UNITY_EDITOR
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MomSesImSpcl.Extensions;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Contains helper methods to check members for <c>null</c>.
    /// </summary>
    public static class NullChecker
    {
        /// <summary>
        /// Checks every member in the given <see cref="Expression{TDelegate}"/> for <c>null</c> and prints the name to the console.
        /// </summary>
        /// <param name="_Expression">Each member will be checked if it is <c>null</c>.</param>
        /// <param name="_TraverseInnerLambdas">Set this to <c>false</c> to not traverse the objects of any inner lambda expression.</param>
        /// <typeparam name="T">Can be any <see cref="Type"/>.</typeparam>
        /// <returns>The evaluated result of the <see cref="Expression{TDelegate}"/>.</returns>
        public static T Traverse<T>(Expression<Func<T>> _Expression, bool _TraverseInnerLambdas = true)
        {
            return (T)Traverse(_Expression.Body, _TraverseInnerLambdas);
        }

        /// <summary>
        /// Checks every member in the given <see cref="Expression{TDelegate}"/> for <c>null</c> and prints the name to the console.
        /// </summary>
        /// <param name="_Expression">Each member will be checked if it is <c>null</c>.</param>
        /// <param name="_TraverseInnerLambdas">Set this to <c>false</c> to not traverse the objects of any inner lambda expression.</param>
        public static void Traverse(Expression<Action> _Expression, bool _TraverseInnerLambdas = true)
        {
            Traverse(_Expression.Body, _TraverseInnerLambdas);
        }
        
        /// <summary>
        /// Traverses every member in the given <see cref="Expression"/> and checks it for <c>null</c>.
        /// </summary>
        /// <param name="_Expression">The <see cref="Expression"/> to traverse and check.</param>
        /// <param name="_TraverseInnerLambdas">Set this to <c>false</c> to not traverse the objects of any inner lambda expression.</param>
        /// <returns>The evaluated result of the <see cref="Expression"/>.</returns>
        /// <exception cref="NullReferenceException">When any member in the <see cref="Expression"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">When the <see cref="Expression"/> contains an unsupported <see cref="Type"/>.</exception>
        private static object Traverse(Expression _Expression, bool _TraverseInnerLambdas)
        {
            while (true)
            {
                switch (_Expression)
                {
                    case MemberExpression _memberExpression:
                    {
                        if (Traverse(_memberExpression.Expression, _TraverseInnerLambdas) is {} _member)
                        {
                            var _memberValue = _memberExpression.Member switch
                            {
                                PropertyInfo _propertyInfo => _propertyInfo.GetValue(_member),
                                FieldInfo _fieldInfo => _fieldInfo.GetValue(_member),
                                _ => _member
                            };

                            if (_memberValue is not null)
                            {
                                return _memberValue;
                            }
                        }

                        throw new NullReferenceException($"{_memberExpression.Member.Name.Bold()} is null.");
                    }
                    case MethodCallExpression _methodCallExpression:
                        if (_methodCallExpression.Object != null && Traverse(_methodCallExpression.Object, _TraverseInnerLambdas) is {} _object)
                        {
                            var _arguments = _methodCallExpression.Arguments.Select(_Argument => Traverse(_Argument, _TraverseInnerLambdas)).ToArray();

                            return _methodCallExpression.Method.Invoke(_object, _arguments);   
                        }
                        
                        throw new NullReferenceException($"{ExtractMethodInstanceName(_methodCallExpression).Bold()} is null.");
                        
                    case BinaryExpression _binaryExpression:
                        if (Traverse(_binaryExpression.Left, _TraverseInnerLambdas) is null)
                        {
                            throw new NullReferenceException($"The left operand is null: {_binaryExpression.Left.ToString().Bold()}");
                        }
                        if (Traverse(_binaryExpression.Right, _TraverseInnerLambdas) is null)
                        {
                            throw new NullReferenceException($"The right operand is null: {_binaryExpression.Right.ToString().Bold()}");
                        }

                        return Expression.Lambda<Func<object>>(Expression.Convert(_binaryExpression, typeof(object))).Compile().Invoke();

                    case LambdaExpression _lambdaExpression:
                        if (_TraverseInnerLambdas)
                        {
                            return Traverse(_lambdaExpression.Body, true);
                        }
                        
                        return _lambdaExpression.Compile();
                    
                    case ParameterExpression _parameterExpression:
                        try
                        {
                            return Expression.Lambda(_parameterExpression).Compile().DynamicInvoke();
                        }
                        catch
                        {
                            throw new NullReferenceException($"{_parameterExpression.Name.Bold()} is null.");
                        }
                        
                    case UnaryExpression _unaryExpression:
                        _Expression = _unaryExpression.Operand;
                        continue;

                    case ConstantExpression _constantExpression:
                        return _constantExpression.Value;
                        
                    default:
                        throw new InvalidOperationException($"Unsupported expression type: {_Expression.NodeType.ToString().Bold()}.");
                }
            }
        }

        /// <summary>
        /// Extract the name of the instance of an <see cref="object"/> from which the <see cref="MethodCallExpression"/> was called.
        /// </summary>
        /// <param name="_MethodCallExpression">The <see cref="MethodCallExpression"/> that was called.</param>
        /// <returns>The name of the variable from which the method was called, or <c>this</c> if the method was called from <c>this</c>.</returns>
        private static string ExtractMethodInstanceName(MethodCallExpression _MethodCallExpression)
        {
            /* This will be how the expression will look when the method instance was "this". */
            // GameObjectName (Namespace.Class).Method()
            // value(Namespace.Class).Method()
            
            /* This will be how the expression will look when the method instance was a variable. */
            // value(Namespace.Class+<>c__DisplayClass3_0).MethodInstance.Method()
            
            var _expression = _MethodCallExpression.ToString();
            var _methodName = _MethodCallExpression.Method.Name;

            // Extract after (Namespace.Class) and before .Method
            var _methodInstance = _expression.ExtractBetween("\\)", $".{_methodName}");

            if (!string.IsNullOrEmpty(_methodInstance))
            {
                // At this point the string should look like this: .MethodInstance
                return _methodInstance[1..]; // Removes the '.' before the name.
            }
            
            return "this";
        }
    }
}
#endif