using System;
using TicketApp.Model.Enums;

namespace TicketApp.Model.Entities
{

    // Entidad principal del sistema
    //Representa un Ticket de soporte

    public class Ticket
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descripcion { get; private set; }
        public TicketStatus Estado { get; private set; }
        public DateTime FechaCreacion { get; private set; }


        // Constructor: solo el modelo crea tickets

        public Ticket(int id, string titulo, string descripcion)
        {
            Id = id;
            Titulo = titulo;
            Descripcion = descripcion;
            Estado = TicketStatus.Abierto;
            FechaCreacion = DateTime.Now;
        }


        //Cambia el estado del ticket

        public void CambiarEstado(TicketStatus nuevoEstado)
        {
            Estado = nuevoEstado;
        }
    }
}
