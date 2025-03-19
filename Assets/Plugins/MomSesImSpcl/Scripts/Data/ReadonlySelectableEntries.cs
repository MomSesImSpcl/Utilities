#if ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Provides a read-only collection of selectable entries with values of type <typeparamref name="V"/>.
    /// </summary>
    /// <typeparam name="V">The type of the values contained within the selectable entries.</typeparam>
    [Serializable]
    public sealed class ReadonlySelectableEntries<V>
    {
        #region Inspector Fields
        [Tooltip("Collection of selectable entries containing values of type V.")]
        [PropertyOrder(2)][ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        [SerializeField] private List<ReadonlySelectableEntry<V>> entries;
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets a read-only collection of selectable entries containing values of type <typeparamref name="V"/>.
        /// </summary>
        /// <value>A <see cref="ReadOnlyCollection{T}"/> of <see cref="ReadonlySelectableEntry{V}"/>.</value>
        public ReadOnlyCollection<ReadonlySelectableEntry<V>> Entries => this.entries.AsReadOnly();
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ReadonlySelectableEntries{V}"/>.
        /// </summary>
        public ReadonlySelectableEntries()
        {
            this.entries = new List<ReadonlySelectableEntry<V>>();
        }
        
        /// <summary>
        /// <see cref="ReadonlySelectableEntries{V}"/>.
        /// </summary>
        /// <param name="_Entries"><see cref="entries"/>.</param>
        public ReadonlySelectableEntries(IEnumerable<ReadonlySelectableEntry<V>> _Entries)
        {
            this.entries = new List<ReadonlySelectableEntry<V>>(_Entries);
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Selects all entries in the <see cref="ReadonlySelectableEntries{V}"/> collection.
        /// </summary>
        [ButtonGroup, PropertyOrder(1)]
        private void SelectAll()
        {
            this.SetKeys(true);
        }

        /// <summary>
        /// Deselects all entries in the collection by setting their keys to false.
        /// </summary>
        [ButtonGroup, PropertyOrder(1)]
        private void DeselectAll()
        {
            this.SetKeys(false);
        }

        /// <summary>
        /// Toggles the key value for all entries in the collection.
        /// </summary>
        /// <param name="_Key">The new key value to be set for each entry.</param>
        private void SetKeys(bool _Key)
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < this.entries.Count; i++)
            {
                this.entries[i] = new ReadonlySelectableEntry<V>(_Key, this.entries[i].Entry.Value);
            }
        }

        /// <summary>
        /// Populates the read-only collection of selectable entries with the provided values.
        /// </summary>
        /// <param name="_Values">The collection of values to populate the entries with.</param>
        public void Populate(IEnumerable<V> _Values)
        {
            var _selected = this.GetSelected().ToArray();
            
            this.entries.Clear();
            
            foreach (var _value in _Values)
            {
                this.entries.Add(_selected.Contains(_value)
                    ? new ReadonlySelectableEntry<V>(true, _value)
                    : new ReadonlySelectableEntry<V>(false, _value));
            }
        }

        /// <summary>
        /// Retrieves an array of selected entries.
        /// </summary>
        /// <param name="_PrintWarning">Indicates whether to print a warning if no entries are selected. Defaults to false.</param>
        /// <returns>An array containing the values of the selected entries.</returns>
        public V[] GetSelected(bool _PrintWarning = false)
        {
            var _selected = this.entries.Where(_ReadonlySelectableEntry => _ReadonlySelectableEntry.Entry.Key).Select(_ReadonlySelectableEntry => _ReadonlySelectableEntry.Entry.Value).ToArray();

#if UNITY_EDITOR
            if (_PrintWarning && _selected.Length == 0)
            {
                Debug.LogWarning("No entries selected.");
            }
#endif
            return _selected;
        }
        #endregion
    } 
}
#endif
