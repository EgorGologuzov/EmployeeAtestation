using EmployeeAtestation.Models;
using EmployeeAtestation.Utils;

namespace EmployeeAtestation.Data
{
    public static class TestRepository
    {
        public static async Task<Test> Get(string fileName)
        {
            string data = await Drive.ReadAllFromFile(fileName, "Tests");

            return data.FromJson<Test>();
        }
    }
}
