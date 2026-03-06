using System.ComponentModel;
using Feedback_Flow.Helpers;
using FluentAssertions;
using Xunit;

namespace Feedback_Flow.Tests.Helpers;

public class SortableBindingListTests
{
    private class TestItem
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void AddItem_ListSortsByDefaultProperty()
    {
        var list = new SortableBindingList<TestItem>("Name");

        list.Add(new TestItem { Name = "Charlie" });
        list.Add(new TestItem { Name = "Alice" });
        list.Add(new TestItem { Name = "Bob" });

        list[0].Name.Should().Be("Alice");
        list[1].Name.Should().Be("Bob");
        list[2].Name.Should().Be("Charlie");
    }

    [Fact]
    public void ApplySort_ByProperty_SortsCorrectly()
    {
        var list = new SortableBindingList<TestItem>();
        list.Add(new TestItem { Name = "Charlie" });
        list.Add(new TestItem { Name = "Alice" });

        var prop = TypeDescriptor.GetProperties(typeof(TestItem))["Name"]!;
        ((IBindingList)list).ApplySort(prop, ListSortDirection.Ascending);

        list[0].Name.Should().Be("Alice");
        list[1].Name.Should().Be("Charlie");
    }

    [Fact]
    public void ApplySort_Descending_SortsDescending()
    {
        var list = new SortableBindingList<TestItem>();
        list.Add(new TestItem { Name = "Alice" });
        list.Add(new TestItem { Name = "Charlie" });
        list.Add(new TestItem { Name = "Bob" });

        var prop = TypeDescriptor.GetProperties(typeof(TestItem))["Name"]!;
        ((IBindingList)list).ApplySort(prop, ListSortDirection.Descending);

        list[0].Name.Should().Be("Charlie");
        list[1].Name.Should().Be("Bob");
        list[2].Name.Should().Be("Alice");
    }

    [Fact]
    public void SupportsSorting_ReturnsTrue()
    {
        var list = new SortableBindingList<TestItem>();

        ((IBindingList)list).SupportsSorting.Should().BeTrue();
    }
}
