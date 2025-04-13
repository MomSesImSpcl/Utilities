#if ODIN_INSPECTOR && UNITY_EDITOR
using System;
using System.Text.RegularExpressions;
using MomSesImSpcl.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Contains inspector buttons for: <br/>
    /// <see cref="StringExtensions.ExtractBefore(string, string, bool, int, uint)"/> <br/>
    /// <see cref="StringExtensions.ExtractBetween(string, string, string, bool, uint)"/> <br/>
    /// <see cref="StringExtensions.ExtractAfter(string, string, bool, int, uint)"/>
    /// </summary>
    [Serializable, HideLabel]
    public sealed class RegexTester
    {
        #region Fields
        // ReSharper disable once UnusedMember.Local
        [SerializeField] private Title regexTester = new(nameof(regexTester).CamelCaseSpacing().ConvertToTitleCase());
        #endregion
        
        #region Methods
        /// <summary>
        /// Extracts the given pattern before the given <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_Pattern">The <see cref="Regex"/> pattern to extract.</param>
        /// <param name="_RightToLeft">Set to <c>true</c> to use <see cref="RegexOptions.RightToLeft"/>.</param>
        /// <param name="_Length">
        /// The max amount of <see cref="char"/> to extract. <br/>
        /// <i>Leave at <c>0</c> to extract the entire match.</i>
        /// </param>
        /// <param name="_Occurence">
        /// Determines which occurence to extract, if there are multiple matches. <br/>
        /// <i>Set to <c>&lt; 0</c> to get the last occurence.</i>
        /// </param>
        [Button(ButtonStyle.FoldoutButton), Tooltip("Extracts the given pattern before the given string.")]
        public void ExtractBefore(string _String, string _Pattern, bool _RightToLeft = false, int _Length = 0, uint _Occurence = 1)
        {
            Debug.Log(_String.ExtractBefore(_Pattern, _RightToLeft, _Length, _Occurence));
        }

        /// <summary>
        /// Extracts a <see cref="string"/> between the given patterns.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_StartPattern">
        /// The pattern from where to start matching. <br/>
        /// <i>Pattern will not be included in the extracted <see cref="string"/>.</i>
        /// </param>
        /// <param name="_EndPattern">
        /// The pattern where to end matching. <br/>
        /// <i>Pattern will not be included in the extracted <see cref="string"/>.</i>
        /// </param>
        /// <param name="_RightToLeft">Set to <c>true</c> to use <see cref="RegexOptions.RightToLeft"/>.</param>
        /// <param name="_Occurence">
        /// Determines which occurence to extract, if there are multiple matches. <br/>
        /// <i>Set to <c>&lt; 0</c> to get the last occurence.</i>
        /// </param>
        [Button(ButtonStyle.FoldoutButton), Tooltip("Extracts a string between the given patterns.")]
        public void ExtractBetween(string _String, string _StartPattern, string _EndPattern, bool _RightToLeft = false, uint _Occurence = 1)
        {
            Debug.Log(_String.ExtractBetween(_StartPattern, _EndPattern, _RightToLeft, _Occurence));
        }
        
        /// <summary>
        /// Extracts the given pattern after the given <see cref="string"/>.
        /// </summary>
        /// <param name="_String">The <see cref="string"/> to extract from.</param>
        /// <param name="_Pattern">The <see cref="Regex"/> pattern to extract.</param>
        /// <param name="_RightToLeft">Set to <c>true</c> to use <see cref="RegexOptions.RightToLeft"/>.</param>
        /// <param name="_Length">
        /// The max amount of <see cref="char"/> to extract. <br/>
        /// <i>Leave at <c>0</c> to extract the entire match.</i>
        /// </param>
        /// <param name="_Occurence">
        /// Determines which occurence to extract, if there are multiple matches. <br/>
        /// <i>Set to <c>&lt; 0</c> to get the last occurence.</i>
        /// </param>
        [Button(ButtonStyle.FoldoutButton), Tooltip("Extracts the given pattern after the given string.")]
        public void ExtractAfter(string _String, string _Pattern, bool _RightToLeft = false, int _Length = 0, uint _Occurence = 1)
        {
            Debug.Log(_String.ExtractAfter(_Pattern, _RightToLeft, _Length, _Occurence));
        }
        #endregion
    }
}
#endif