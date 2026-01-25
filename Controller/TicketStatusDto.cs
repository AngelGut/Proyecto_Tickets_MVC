using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// Estado "para UI". La View usará este enum y NO el del Model.
    /// </summary>
    public enum TicketStatusDto
    {
        Abierto,
        Cerrado
    }
}
