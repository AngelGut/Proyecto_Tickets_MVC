using System.Collections.Generic;
using System.Linq;
using TicketApp.Model.Entities;
using TicketApp.Model.Enums;

namespace TicketApp.Model.Store
{

    //Almacenamiento en memoria de los tickets
    // Vive SOLO durante la ejecución

    public class TicketStore
    {
        private readonly List<Ticket> _tickets;
        private int _nextId;

        public TicketStore()
        {
            _tickets = new List<Ticket>();
            _nextId = 1;
        }


        // Crea un nuevo ticket y lo guarda en memoria

        public Ticket Add(string titulo, string descripcion)
        {
            var ticket = new Ticket(_nextId++, titulo, descripcion);
            _tickets.Add(ticket);
            return ticket;
        }


        // Devuelve todos los tickets

        public List<Ticket> GetAll()
        {
            // Se devuelve copia para evitar modificaciones externas
            return _tickets.ToList();
        }

        >
        // Busca un ticket por ID
        
        public Ticket? GetById(int id)
        {
            return _tickets.FirstOrDefault(t => t.Id == id);
        }


        // Cambia el estado de un ticket existente

        public bool ChangeStatus(int id, TicketStatus status)
        {
            var ticket = GetById(id);
            if (ticket == null)
                return false;

            ticket.CambiarEstado(status);
            return true;
        }
    }
}
