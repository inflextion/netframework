using atf.Core.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace atf.Core.Utils;

public class FileUtils
{
    protected TestLogger TestLogger { get; private set; }
    
    public FileUtils(TestLogger testLogger)
    {
        TestLogger = testLogger;
    }
    /// <summary>
    ///  Reads a file and parses its content as a JToken.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public JToken? GetFileContentAsJToken(string filename)
    {
        var path = Path.Combine(Environment.CurrentDirectory, filename);
        if (!File.Exists(path))
        {
            TestLogger.Error("File '{FileName}' does not exist.", filename);
            return null;
        }
        var content = File.ReadAllText(path);
        if (string.IsNullOrEmpty(content))
        {
            TestLogger.Error("File '{FileName}' is empty.", filename);
            return null;
        }
        try
        {
            return JToken.Parse(content);
        }
        catch (Exception e)
        {
            TestLogger.Error("Failed to parse JSON from file '{FileName}': {ErrorMessage}", filename, e.Message);
            return null;
        }
    }
    /// <summary>
    /// Asynchronously reads a file and parses its content as a JToken.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public async Task<JToken?> GetFileContentAsJTokenAsync(string filename)
    {
        var path = Path.Combine(Environment.CurrentDirectory, filename);
        if (!File.Exists(path))
        {
            TestLogger.Error("File '{FileName}' does not exist.", filename);
            return null;
        }
        var content = await File.ReadAllTextAsync(path);
        if (string.IsNullOrEmpty(content))
        {
            TestLogger.Error("File '{FileName}' is empty.", filename);
            return null;
        }
        try
        {
            return JToken.Parse(content);
        }
        catch (Exception e)
        {
            TestLogger.Error("Failed to parse JSON from file '{FileName}': {ErrorMessage}", filename, e.Message);
            return null;
        }
    }
}