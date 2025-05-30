namespace EmployeeManagementAPI.Models
{
    public class EmployeeDTO
    {
        #region property
        public long Id { get; set; } = 0;
        public Guid? Guid { get; set; }
        public string? Name { get; set; }
        public long Designation { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? Gender { get; set; }
        public string? ProfileImage { get; set; }
        #endregion
    }
}
