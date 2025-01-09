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
    /// Represents a collection of selectable entries, each of type <typeparamref name="V"/>.
    /// </summary>
    /// <typeparam name="V">The type of the value in each selectable entry.</typeparam>
    [Serializable]
    public sealed class SelectableEntries<V>
    {
        #region Inspector Fields
        [Tooltip("Selectable entries with a generic value.")]
        [PropertyOrder(2)][ListDrawerSettings(CustomAddFunction = nameof(OnEntryAdded))]
        [SerializeField] private List<SelectableEntry<V>> entries;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the read-only collection of selectable entries.
        /// </summary>
        /// <value>
        /// A <see cref="ReadOnlyCollection{T}"/> containing the selectable entries of type
        /// <typeparamref name="V"/>.
        /// </value>
        public ReadOnlyCollection<SelectableEntry<V>> Entries => this.entries.AsReadOnly();
        #endregion

        #region Constructors
        /// <summary>
        /// <see cref="ReadonlySelectableEntries{V}"/>.
        /// </summary>
        /// <param name="_Entries"><see cref="entries"/>.</param>
        public SelectableEntries(IEnumerable<SelectableEntry<V>> _Entries)
        {
            this.entries = new List<SelectableEntry<V>>(_Entries);
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Method called when an entry is added to the list of selectable entries.
        /// </summary>
        /// <param name="_SelectableEntry">The list of selectable entries to which a new entry is added.</param>
        private void OnEntryAdded(List<SelectableEntry<V>> _SelectableEntry)
        {
            _SelectableEntry.Add(new SelectableEntry<V>(true, default!));
        }

        /// <summary>
        /// Selects all entries in the collection, marking them as selected.
        /// </summary>
        [ButtonGroup, PropertyOrder(1)]
        private void SelectAll()
        {
            this.SetKeys(true);
        }

        /// <summary>
        /// Deselects all entries in the collection.
        /// </summary>
        [ButtonGroup, PropertyOrder(1)]
        private void DeselectAll()
        {
            this.SetKeys(false);
        }

        /// <summary>
        /// Updates the key for all entries in the collection.
        /// </summary>
        /// <param name="_Key">The new key value to set for each entry.</param>
        private void SetKeys(bool _Key)
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < this.Entries.Count; i++)
            {
                this.entries[i] = new SelectableEntry<V>(_Key, this.Entries[i].Entry.Value);
            }
        }

        /// <summary>
        /// Sets the keys for the entries based on the provided collection of values and a key flag.
        /// </summary>
        /// <param name="_Values">A collection of values used to determine which entries to update.</param>
        /// <param name="_Key">A boolean flag used to set the key for matching entries.</param>
        public void SetKeys(ICollection<V> _Values, bool _Key)
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < this.Entries.Count; i++)
            {
                if (_Values.Contains(this.Entries[i].Entry.Value))
                {
                    this.entries[i] = new SelectableEntry<V>(_Key, this.Entries[i].Entry.Value);
                }
                else
                {
                    this.entries[i] = new SelectableEntry<V>(!_Key, this.Entries[i].Entry.Value);
                }
            }
        }

        /// <summary>
        /// Retrieves the values of all selected entries.
        /// </summary>
        /// <returns>An enumerable collection of selected entry values.</returns>
        public IEnumerable<V> GetSelected()
        {
            return this.Entries.Where(_SelectableEntry => _SelectableEntry.Entry.Key).Select(_SelectableEntry => _SelectableEntry.Entry.Value);
        }

        /// <summary>
        /// Adds a <see cref="SelectableEntry{V}"/> to the collection of entries.
        /// </summary>
        /// <param name="_Entry">The entry to be added to the collection.</param>
        public void Add(SelectableEntry<V> _Entry)
        {
            this.entries.Add(_Entry);
        }

        /// <summary>
        /// Clears all entries from the collection.
        /// </summary>
        public void ClearAll()
        {
            this.entries.Clear();
        }
        #endregion
    } 
}
#endif
