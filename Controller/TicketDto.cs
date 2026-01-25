using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// DTO para la View: NO es la entidad del Model.
    /// Solo contiene lo que la UI necesita mostrar/usar.
    /// </summary>
    public class TicketDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = "";

        public string Descripcion { get; set; } = "";

        public TicketStatusDto Estado { get; set; }

        public string FechaCreacionTexto { get; set; } = "";
    }
}
