using System.ComponentModel;

namespace Feedback_Flow.Helpers;

/// <summary>
/// A BindingList that supports automatic sorting by a specified property.
/// This enables DataGridView to display items in alphabetical order automatically.
/// </summary>
/// <typeparam name="T">The type of elements in the list</typeparam>
public class SortableBindingList<T> : BindingList<T>
{
    private readonly PropertyDescriptor? _sortProperty;
    private readonly ListSortDirection _sortDirection;
    private bool _isSorted;

    /// <summary>
    /// Initializes a new instance with optional sorting configuration.
    /// </summary>
    /// <param name="sortPropertyName">The property name to sort by (e.g., "FullName")</param>
    /// <param name="sortDirection">The sort direction (Ascending or Descending)</param>
    public SortableBindingList(string? sortPropertyName = null, ListSortDirection sortDirection = ListSortDirection.Ascending)
    {
        // Get the property descriptor for sorting if specified
        if (!string.IsNullOrEmpty(sortPropertyName))
        {
            _sortProperty = TypeDescriptor.GetProperties(typeof(T))[sortPropertyName];
            _sortDirection = sortDirection;
            _isSorted = _sortProperty != null;
        }
    }

    /// <summary>
    /// Applies sorting to the list when an item is added.
    /// </summary>
    protected override void OnListChanged(ListChangedEventArgs e)
    {
        base.OnListChanged(e);

        // Re-sort when items are added or changed
        if (_isSorted && (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemChanged))
        {
            ApplySort();
        }
    }

    /// <summary>
    /// Applies the configured sort to all items in the list.
    /// </summary>
    private void ApplySort()
    {
        if (_sortProperty == null) return;

        var items = this.ToList();
        
        // Sort based on direction
        if (_sortDirection == ListSortDirection.Ascending)
        {
            items = items.OrderBy(x => _sortProperty.GetValue(x)).ToList();
        }
        else
        {
            items = items.OrderByDescending(x => _sortProperty.GetValue(x)).ToList();
        }

        // Clear and re-add in sorted order
        // Temporarily disable notifications to avoid multiple refresh events
        RaiseListChangedEvents = false;
        try
        {
            Clear();
            foreach (var item in items)
            {
                Add(item);
            }
        }
        finally
        {
            RaiseListChangedEvents = true;
            ResetBindings();
        }
    }

    /// <summary>
    /// Manually triggers a re-sort of the list.
    /// Useful when an item's sort property has been modified externally.
    /// </summary>
    public void Sort()
    {
        if (_isSorted)
        {
            ApplySort();
        }
    }

    /// <summary>
    /// Gets whether the list is currently sorted.
    /// </summary>
    protected override bool SupportsSortingCore => true;

    /// <summary>
    /// Gets whether the list is sorted.
    /// </summary>
    protected override bool IsSortedCore => _isSorted;

    /// <summary>
    /// Gets the current sort direction.
    /// </summary>
    protected override ListSortDirection SortDirectionCore => _sortDirection;

    /// <summary>
    /// Gets the property descriptor used for sorting.
    /// </summary>
    protected override PropertyDescriptor? SortPropertyCore => _sortProperty;

    /// <summary>
    /// Removes sorting from the list.
    /// </summary>
    protected override void RemoveSortCore()
    {
        _isSorted = false;
    }
}
