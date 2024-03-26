using MEDITAP_CRUD.Data;
using MEDITAP_CRUD.Models;
using MEDITAP_CRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MEDITAP_CRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDbContext mVCDbContext;
        public EmployeesController(MVCDbContext mVCDbContext)
        {
            this.mVCDbContext = mVCDbContext;
        }

        // Get All List of Employee (view)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mVCDbContext.Employees.ToListAsync();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Add New Employee
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            // Iterate Model
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department
            };

            // Handle entity framework untuk add & save data ke database
            await mVCDbContext.Employees.AddAsync(employee);
            await mVCDbContext.SaveChangesAsync();
            // If add sukses, balik ke page Index (show list employees)
            return RedirectToAction("Index");
        }

        // View Detail Employee
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            // Get employee by Id
            var employee = await mVCDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };

                return await Task.Run(() => View("VIew", viewModel));
            }

            return RedirectToAction("Index");
        }

        // Update Employee based on ID
        [HttpPost]
        public  async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            // get Employee ID on DB
            var employee = await mVCDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                // Update the employee data based on ID
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                // Save yang udah di Update
                await mVCDbContext.SaveChangesAsync();

                // Return user ke page Employee
                return RedirectToAction("Index");
            };

            return RedirectToAction("Index");
        }

        // Delete Employee based on ID
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mVCDbContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                // Delete employee jika tidak null
                mVCDbContext.Employees.Remove(employee);

                // Save yang sudah di delete
                await mVCDbContext.SaveChangesAsync();

                // Return user ke page Employee
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
