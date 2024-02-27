using EmployeeAtestation.Models;
using EmployeeAtestation.Pages;
using EmployeeAtestation.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAtestation.Data
{
    public static class EmployeeRepository
    {
        public static async Task<Employee?> Get(string code, string password)
        {
            try
            {
                string data = await Drive.ReadAllFromFile($"{code}-{password.ToSha256Hash()}.json", "Employees");
                return string.IsNullOrEmpty(data) ? null : data.FromJson<Employee>();
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static async Task<Employee> Create(Employee employee)
        {
            var files = await Drive.GetAllFiles(DriveConfig.EmployeesFolder);

            if (files is null || files.Count == 0)
            {
                employee.Code = 1.ToString("000000");
            }
            else
            {
                int max = files.Select(f => Convert.ToInt32(f.Name.Split("-").First())).Max();
                employee.Code = (max + 1).ToString("000000");
            }

            await Drive.CreateFile($"{employee.Code}-{employee.PasswordHash}.json", DriveConfig.EmployeesFolder, employee.ToJson());

            return employee;
        }
    }
}
