using System.Collections.Generic;

namespace BackendSport.Application.DTOs.RolDTOs
{
    public class RolListDto
    {
        public required string Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public List<string> Permisos { get; set; } = new List<string>();
    }
} 