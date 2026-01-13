using System.Collections;

namespace EngConnect.BuildingBlock.Application.Params;

public class SortParams : IList<SortParameter>
{
    private readonly List<SortParameter> _sortParameters = [];

    // IList<SortParameter> implementation
    public SortParameter this[int index]
    {
        get => _sortParameters[index];
        set => _sortParameters[index] = value;
    }

    public int Count => _sortParameters.Count;

    public bool IsReadOnly => false;

    public void Add(SortParameter item) => _sortParameters.Add(item);
    public void AddRange(List<SortParameter> sortParameters) => _sortParameters.AddRange(sortParameters);

    public void Clear() => _sortParameters.Clear();

    public bool Contains(SortParameter item) => _sortParameters.Contains(item);

    public void CopyTo(SortParameter[] array, int arrayIndex) => _sortParameters.CopyTo(array, arrayIndex);

    public IEnumerator<SortParameter> GetEnumerator() => _sortParameters.GetEnumerator();

    public int IndexOf(SortParameter item) => _sortParameters.IndexOf(item);

    public void Insert(int index, SortParameter item) => _sortParameters.Insert(index, item);

    public bool Remove(SortParameter item) => _sortParameters.Remove(item);

    public void RemoveAt(int index) => _sortParameters.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => _sortParameters.GetEnumerator();
}

public class SortParameter
{
    public SortParameter(string fieldName, bool isDescending)
    {
        FieldName = fieldName;
        IsDescending = isDescending;
    }
    public string FieldName { get; set; }
    public bool IsDescending { get; set; }
}