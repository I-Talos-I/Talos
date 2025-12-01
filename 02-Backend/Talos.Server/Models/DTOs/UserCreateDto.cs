namespace Talos.Server.Models.Dtos
{
    public class UserCreateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Tier { get; set; }
        public int PrivateTemplateLimit { get; set; }
    }
}