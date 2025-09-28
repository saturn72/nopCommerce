using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace KedemMarket.Domain.Ordering;
public class OrderItemsStatus : BaseEntity
{
    private string _statuses;
    private IReadOnlyDictionary<string, IEnumerable<int>> _statusDictionary;
    private static readonly JsonSerializerOptions _jso = new()
    {
        AllowTrailingCommas = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.All),
    };

    public int OrderId { get; set; }
    public string? Statuses
    {
        get => _statuses;
        set
        {
            _statuses = value;
            _statusDictionary = null;
        }
    }
    public IReadOnlyDictionary<string, IEnumerable<int>> StatusDictionary
    {
        get => GetStatusDictionary();
        set
        {
            _statusDictionary = value;
            _statuses = JsonSerializer.Serialize(value, _jso);
        }
    }

    private IReadOnlyDictionary<string, IEnumerable<int>> GetStatusDictionary()
    {
        if (_statusDictionary != null)
            return _statusDictionary;

        if (_statuses == null)
        {
            _statusDictionary = new Dictionary<string, IEnumerable<int>>(StringComparer.OrdinalIgnoreCase);
            return _statusDictionary;
        }

        var d = JsonSerializer.Deserialize<Dictionary<string, IEnumerable<int>>>(_statuses);
        return (_statusDictionary = new Dictionary<string, IEnumerable<int>>(d, StringComparer.OrdinalIgnoreCase));
    }
}
