using System.ComponentModel;

namespace Feedback_Flow.Helpers;

/// <summary>
/// A BindingList that supports automatic sorting by a specified property.
/// This enables DataGridView to display items in alphabetical order automatically.
/// </summary>
/// <typeparam name="T">The type of elements in the list</typeparam>
public class SortableBindingList<T> : BindingList<T>
{
    private PropertyDescriptor? _sortProperty;
    private ListSortDirection _sortDirection;
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
            ApplySortCore(_sortProperty!, _sortDirection);
        }
    }

    /// <summary>
    /// Core implementation of sorting.
    /// </summary>
    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
        _sortProperty = prop;
        _sortDirection = direction;

        var items = this.ToList();

        if (direction == ListSortDirection.Ascending)
        {
            items = items.OrderBy(x => prop.GetValue(x)).ToList();
        }
        else
        {
            items = items.OrderByDescending(x => prop.GetValue(x)).ToList();
        }

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
            _isSorted = true;
            RaiseListChangedEvents = true;
            ResetBindings();
        }
    }

    protected override void RemoveSortCore()
    {
        _isSorted = false;
        _sortProperty = null;
    }

    /// <summary>
    /// Manually triggers a re-sort of the list.
    /// Useful when an item's sort property has been modified externally.
    /// </summary>
    public void Sort()
    {
        if (_isSorted && _sortProperty != null)
        {
            ApplySortCore(_sortProperty, _sortDirection);
        }
    }

    protected override bool SupportsSortingCore => true;

    protected override bool IsSortedCore => _isSorted;

    protected override ListSortDirection SortDirectionCore => _sortDirection;

    protected override PropertyDescriptor? SortPropertyCore => _sortProperty;
}
