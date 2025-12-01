using System;
using System.Collections.Generic;

namespace Talos.Server.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Tier { get; set; }
        public int PrivateTemplateLimit { get; set; }
        public DateTime CreateAt { get; set; }

        // Lista opcional de templates resumidos
        public List<TemplateDto> Templates { get; set; }
    }
}