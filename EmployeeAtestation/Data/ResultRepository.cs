using EmployeeAtestation.Models;
using EmployeeAtestation.Utils;

namespace EmployeeAtestation.Data
{
    public static class ResultRepository
    {
        public static async Task<bool> HasResult(string code)
        {
            return (await Drive.GetFile($"{code}.json", DriveConfig.ResultsFolder)) is not null;
        }

        public static async Task SendResult(string code, TestResult result)
        {
            await Drive.CreateFile($"{code}.json", DriveConfig.ResultsFolder, result.ToJson());
        }

        public static async Task<IList<TestResult>> GetBetween(DateTime startDate, DateTime endDate)
        {
            var files = await Drive.GetFilesBetween(DriveConfig.ResultsFolder, startDate, endDate);
            var tasks = files.Select(f => Drive.ReadAllFromFile(f.Id));
            var jsons = await Task.WhenAll(tasks);
            var result = jsons.Select(j => j.FromJson<TestResult>()).ToList();

            return result;
        }
    }
}
