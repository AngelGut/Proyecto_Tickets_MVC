using System;

namespace Model
{
    /// <summary>
    /// Representara los estados posibles de un Ticket dentro del sistema, digase Cerrado o Abierto
    /// El uso del enum nos dejara controlar y restringir los valores validos que puede tener el estado de un ticket
    /// que puede tener el estado de un ticket.
    /// </summary>
    
    public enum TicketStatus
    {
        //TODO: Abiero representa un ticket que esta en proceso de resolucion, Cerrado representa un ticket que ya fue resuelto
        Abierto,
        Cerrado
    }
}
