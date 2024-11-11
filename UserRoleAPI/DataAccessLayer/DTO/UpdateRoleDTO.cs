namespace UserRoleAPI.DataAccessLayer.DTO
{
    public class UpdateRoleDTO
    {
        public string? RoleName { get; set; }
        public string? CreatedBy { get; set; }
        public bool? ActiveStatus { get; set; } = true;
    }
}