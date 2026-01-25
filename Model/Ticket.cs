using System;
using TicketApp.Model;

namespace Model
{
    /// <summary>
    /// Esta clase contiene los datos principales y las reglas basicas  que definen el comportamiento de un ticket
    /// </summary>
    public class Ticket
    {
        //TODO: Estas son las propiedades basicas que debe tener un ticket
        public int Id { get; private set; }
        public string Titulo { get; private set; }
        public string Descripcion { get; private set; }
        public TicketStatus Estado { get; private set; }
        public DateTime FechaCreacion { get; private set; }

        //TODO: Este Constructor inicializa un ticket con los datos basicos y establece el estado inicial
        public Ticket(int id, string titulo, string descripcion)
        {
            //TODO: Esta es un Validacion basica encargada de asegurar que el titulo no este vacio
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título no puede estar vacío");

            Id = id;
            Titulo = titulo;
            Descripcion = descripcion;
            Estado = TicketStatus.Abierto;
            FechaCreacion = DateTime.Now;
        }

        //TODO: Este metodo permite cerrar un ticket, cambiando su estado a Cerrado
        public void Cerrar()
        {
            Estado = TicketStatus.Cerrado;
        }
    }
}
