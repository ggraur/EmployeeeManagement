using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeeManagement.Models
{
   public interface IEmployeeRepository
    {
        Employee GetEmployee(int ID);
        IEnumerable<Employee> GetAllEmployee();
        //object GetAllEmployee();

        Employee Add(Employee employee);
        //object Count();
        Employee Update(Employee employeeChanges);
        Employee Delete(int id);

    }
}
