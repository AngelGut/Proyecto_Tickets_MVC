using TicketApp.Model.Enums;
using TicketApp.Model.Common;
using TicketApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// ============================================================
// Archivo: TicketController.cs
// Proyecto: TicketApp.Application (Controller layer)
// Objetivo: Implementar los casos de uso que la View ejecuta:
//           - Crear ticket
//           - Listar tickets
//           - Cambiar estado
// Regla: NO dibuja UI. Devuelve datos y resultados.
// ============================================================

namespace Controller
{
    /// <summary>
    /// Controller principal para gestionar tickets.
    /// En MVC, este Controller:
    /// - recibe eventos/datos de la View
    /// - ejecuta la lógica de flujo (casos de uso)
    /// - coordina el Model (incluyendo el Store)
    /// - devuelve resultados a la View para que actualice la UI
    /// </summary>
    public class TicketController
    {
        // ============================================================
        // Store único en memoria.
        // Debe existir UNA sola instancia durante la ejecución,
        // para que la lista no se "reinicie" en cada operación.
        // ============================================================
        private readonly TicketStore _store;

        /// <summary>
        /// Constructor que recibe el store.
        /// Esto permite:
        /// - mantener una instancia única
        /// - facilitar pruebas
        /// - desacoplar la creación del store
        /// </summary>
        public TicketController(TicketStore store)
        {
            // Validación defensiva: si store viene null, explota temprano.
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        // ============================================================
        // CASO DE USO 1: Crear Ticket
        // ============================================================
        /// <summary>
        /// Crea un ticket con título y descripción, lo guarda en el store,
        /// y devuelve la lista actualizada para que la View refresque.
        /// </summary>
        public OperationResult<List<Ticket>> CreateTicket(string titulo, string descripcion)
        {
            // -------------------------
            // Validaciones de flujo
            // -------------------------

            // Validación de título: obligatorio para evitar tickets vacíos.
            if (string.IsNullOrWhiteSpace(titulo))
                return OperationResult<List<Ticket>>.Fail("El título es obligatorio.");

            // Descripción puede ser opcional, pero si quieren forzarla:
            // if (string.IsNullOrWhiteSpace(descripcion))
            //     return OperationResult<List<Ticket>>.Fail("La descripción es obligatoria.");

            // Normalizamos (limpiamos) espacios.
            titulo = titulo.Trim();
            descripcion = (descripcion ?? string.Empty).Trim();

            // -------------------------
            // Ejecución del caso de uso
            // -------------------------

            // Creamos la entidad Ticket.
            // Nota: si su Ticket tiene otro constructor, ajusta aquí.
            var ticket = new Ticket
            {
                // Id normalmente lo asigna el Store (autoincrement).
                Titulo = titulo,
                Descripcion = descripcion,
                Estado = TicketStatus.Abierto,
                FechaCreacion = DateTime.Now
            };

            // Guardamos en memoria.
            _store.Add(ticket);

            // Devolvemos la lista actualizada para que la View la pinte.
            var ticketsActualizados = _store.GetAll();

            return OperationResult<List<Ticket>>.Ok(
                ticketsActualizados,
                "Ticket creado correctamente."
            );
        }

        // ============================================================
        // CASO DE USO 2: Obtener tickets
        // ============================================================
        /// <summary>
        /// Devuelve todos los tickets actuales en memoria.
        /// La View lo usará para cargar/refrescar el DataGrid o ListView.
        /// </summary>
        public OperationResult<List<Ticket>> GetTickets()
        {
            var tickets = _store.GetAll();

            // Aunque esté vacío, sigue siendo un éxito (no es un error).
            return OperationResult<List<Ticket>>.Ok(tickets, "Listado cargado.");
        }

        // ============================================================
        // CASO DE USO 3: Cambiar estado del ticket
        // ============================================================
        /// <summary>
        /// Cambia el estado de un ticket (Abierto/Cerrado) por Id,
        /// y devuelve la lista actualizada.
        /// </summary>
        public OperationResult<List<Ticket>> ChangeStatus(int id, TicketStatus status)
        {
            // -------------------------
            // Validaciones de flujo
            // -------------------------

            if (id <= 0)
                return OperationResult<List<Ticket>>.Fail("Debe seleccionar un ticket válido.");

            // -------------------------
            // Ejecución del caso de uso
            // -------------------------

            // Pedimos al store actualizar.
            bool updated = _store.UpdateStatus(id, status);

            if (!updated)
                return OperationResult<List<Ticket>>.Fail("No se encontró el ticket a modificar.");

            // Lista actualizada para refrescar UI.
            var ticketsActualizados = _store.GetAll();

            return OperationResult<List<Ticket>>.Ok(
                ticketsActualizados,
                "Estado del ticket actualizado."
            );
        }
    }
}
