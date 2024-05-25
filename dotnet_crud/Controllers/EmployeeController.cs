using dotnet_crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic; // Added for Enumerable<T>
using System.Threading.Tasks; // Added for Task and ActionResult

namespace dotnet_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployeeController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee() 
        {
            var employees = await _applicationDbContext.Employees.ToListAsync(); 
            if (employees == null || employees.Count == 0) 
            {
                return NotFound();
            }

            return employees; // Returning the list of employees
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _applicationDbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }



        [HttpPost]

        public async Task<ActionResult<IEnumerable<Employee>>> PostEmployee(Employee employee)
        {

            _applicationDbContext.Employees.Add(employee);
            await _applicationDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest("ID mismatch between URL and employee object.");
            }

            var existingEmployee = await _applicationDbContext.Employees.FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound("Employee not found.");
            }

            _applicationDbContext.Entry(existingEmployee).CurrentValues.SetValues(employee);

            try
            {
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency conflict occurred. Please try again.");
            }

            return NoContent(); // Indicates successful update with no content to return.
        }



        [HttpDelete]

        public async Task<ActionResult<IEnumerable<Employee>>> DeleteEmployee(int id)
        {
            if (_applicationDbContext.Employees ==null)
            {
                return BadRequest();
            }


            var employees = _applicationDbContext.Employees.Find(id);

            if (employees == null)
            { 
            
            return BadRequest();
            
            }

            _applicationDbContext.Employees.Remove(employees);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
            

        
        }
        }




    }
    

