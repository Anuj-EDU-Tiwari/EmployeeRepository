using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        #region constructor
        public EmployeeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion
        #region property
        private readonly AppDbContext _appDbContext;
        #endregion
        #region methods
        [HttpGet]
        public async Task<IActionResult> GetAllUsingSP()
        {
            try
            {
                var employees = await _appDbContext.Employees.FromSqlRaw("EXEC spGetAllEmployees1").ToListAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with error message
                return StatusCode(500, new { message = "An error occurred while retrieving employees.", error = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var param = new SqlParameter("@Id", id);
                var employees = await _appDbContext.Employees.FromSqlRaw("EXEC spGetEmployeeById1 @Id", param).ToListAsync();
                var employee = employees.FirstOrDefault();
                if (employee == null)
                    return NotFound();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating employees.", error = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EmployeeDTO emp)
        {
            try
            {
                if (emp == null)
                    return BadRequest(new { message = "Employee is null." });

                var parameters = new[]
                {
                new SqlParameter("@Id", emp.Id),
                new SqlParameter("@Name", emp.Name ?? (object)DBNull.Value),
                new SqlParameter("@Designation", emp.Designation),
                new SqlParameter("@IsActive", emp.IsActive),
                new SqlParameter("@CreatedDate", emp.CreatedDate),
                new SqlParameter("@CreatedBy", emp.CreatedBy ?? (object)DBNull.Value),
                new SqlParameter("@ModifiedDate", emp.ModifiedDate),
                new SqlParameter("@ModifiedBy", emp.ModifiedBy ?? (object)DBNull.Value),
                new SqlParameter("@Gender", emp.Gender ?? (object)DBNull.Value),
                new SqlParameter("@ProfileImage", emp.ProfileImage ?? (object)DBNull.Value)
        };

                await _appDbContext.Database.ExecuteSqlRawAsync(
                    "EXEC spInsertOrUpdateEmployee @Id, @Name, @Designation, @IsActive, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy, @Gender, @ProfileImage",
                    parameters
                );

                return Ok(new { success = true, message = "Employee saved successfully via SP." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving employees.", error = ex.Message });
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                
                var param = new SqlParameter("@Id", id);
                await _appDbContext.Database.ExecuteSqlRawAsync("EXEC spDeleteEmployeeById @Id", param);

                return Ok(new { success = true, message = "Employee deleted successfully via SP." });
            
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting employees.", error = ex.Message });
            }

        }
        #endregion
    }
}
