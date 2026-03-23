using HrApp.Data;
using HrApp.Models;
using HrApp.Services.Interfaces;
using HrApp.Services.Results;
using Microsoft.EntityFrameworkCore;

namespace HrApp.Services.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task Add(Employee employee)
        {
            if (employee != null)
            {
                 _context.Employees.Add(employee);
                 _context.SaveChanges();
            }
            return Task.CompletedTask;
        }

        public Task Delete(Employee employee)
        {
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var employees = await _context.Employees.ToListAsync();
            return employees;
        }

        public async Task<Employee?> GetById(int? id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            return employee;
        }

        public  Task Update(Employee employee)
        {
            if (employee != null)
            {
                _context.Employees.Update(employee);
                 _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }
    }
}
