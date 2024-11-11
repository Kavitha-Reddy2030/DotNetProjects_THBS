namespace UserRoleAPI.DataAccessLayer.DTO
{
    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public bool ActiveStatus { get; set; } = true;
    }
}