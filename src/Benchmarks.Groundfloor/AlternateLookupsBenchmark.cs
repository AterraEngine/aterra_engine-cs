// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Attributes;

namespace Benchmarks.Groundfloor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]
public class AlternateLookupsBenchmark {
    private Dictionary<string, string> _stringDictionary { get; set; }
    private Dictionary<string, string>.AlternateLookup<ReadOnlySpan<char>> _stringDictionaryAltLookupProperty { get; set; }


    [GlobalSetup]
    public void Setup() {
        var dictionary = new Dictionary<string, string>();
        for (int i = 0; i < 1_000_000; i++) {
            dictionary.Add(i.ToString(), i.ToString());
        }

        _stringDictionary = dictionary;
        _stringDictionaryAltLookupProperty = dictionary.GetAlternateLookup<ReadOnlySpan<char>>();
    }
    
    [Benchmark]
    public string StringDictionary() {
        return _stringDictionary["12345"];
    }
    
    [Benchmark]
    public string StringDictionaryAltLookupProperty() {
        return _stringDictionaryAltLookupProperty["12345"];
    }
}
