using HrApp.Models;

namespace HrApp.Services.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee?> GetById(int? id);
        Task Add(Employee employee);
        Task Update(Employee employee);
        Task Delete(Employee employee);
    }
}
