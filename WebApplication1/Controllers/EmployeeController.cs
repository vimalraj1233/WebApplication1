using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DatabaseContext;
using WebApplication1.Entities;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase

    {
        private readonly IWebAppDBContext _context;
        public EmployeeController(IWebAppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody]Customer emp)
        {
            emp.Id = Guid.NewGuid();
            await _context.Employees.AddAsync(emp);
            await _context.SaveChangesAsync();
            return Ok(emp);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute]Guid id, [FromBody]Customer emp)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(employee != null)
            {
                employee.Name = emp.Name;
                employee.MobileNo = emp.MobileNo;
                employee.EmailID = emp.EmailID;

                 _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return Ok(emp);
            }
            else
            {
                return NotFound("Employee not found");
            }
           
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute]Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return Ok("Employee Deleted Sucessfully");
            }
            else
            {
                return NotFound("Employee not found");
            }

        }
    }
}