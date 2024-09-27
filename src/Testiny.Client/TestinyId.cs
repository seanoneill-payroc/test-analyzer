namespace Testiny.Client;

public class TestinyId
{
    public static object Create(string id)
    {
        return id[..3] switch
        {
            "TR-" => new TestRunId(id),
            "TC-" => new TestCaseId(id)
        };
    }
}

public class TestCaseId
{
    private const string _prefix = "TC-";
    private readonly int _id;
    
    public TestCaseId(string value)
    {
        if(!value.StartsWith(_prefix))
            throw new ArgumentException($"Invalid test case id: {value}");
        
        if (!int.TryParse(value[_prefix.Length..], out _id))
            throw new ArgumentException($"Invalid test case id: {value}");
    }

    public TestCaseId(int value) => _id = value;
    
    public static implicit operator TestCaseId(int value) => new(value);
    public static implicit operator TestCaseId(string value) => new(value);
    public static implicit operator string(TestCaseId id) => id.ToString();
    public static implicit operator int(TestCaseId id) => id._id;
    
    public override string ToString() => $"{_prefix}{_id}";
}

public class TestRunId
{
    private const string _prefix = "TR-";
    private readonly int _id;
    
    public TestRunId(string value)
    {
        if(!value.StartsWith(_prefix))
            throw new ArgumentException($"Invalid test case id: {value}");
        
        if (!int.TryParse(value[_prefix.Length..], out _id))
            throw new ArgumentException($"Invalid test case id: {value}");
    }

    public TestRunId(int value) => _id = value;
    
    public static implicit operator TestRunId(int value) => new(value);
    public static implicit operator TestRunId(string value) => new(value);
    public static implicit operator string(TestRunId id) => id.ToString();
    public static implicit operator int(TestRunId id) => id._id;
    
    public override string ToString() => $"{_prefix}{_id}";
}