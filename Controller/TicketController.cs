using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketApp.Model;               // Enum real del dominio (Model)
using TicketApp.Model.Entities;      // Entidad Ticket (Model)

namespace Controller
{
    /// <summary>
    /// Controlador principal del sistema de tickets.
    /// 
    /// Responsabilidad dentro de MVC:
    /// - Actúa como intermediario entre la View y el Model.
    /// - Recibe eventos y datos provenientes de la View.
    /// - Ejecuta los casos de uso de la aplicación.
    /// - Coordina el acceso al Model.
    /// - Devuelve datos procesados (DTOs) a la View.
    /// 
    /// IMPORTANTE:
    /// - La View NO conoce el Model.
    /// - El Controller SÍ conoce el Model (esto es correcto en MVC).
    /// - La View trabaja exclusivamente con DTOs definidos en Controller.
    /// </summary>
    public class TicketController
    {
        /// <summary>
        /// Store en memoria que gestiona la colección de tickets.
        /// 
        /// Vive en el Model y simula una capa de persistencia.
        /// El Controller lo utiliza para ejecutar los casos de uso.
        /// </summary>
        private readonly TicketStore _store;

        /// <summary>
        /// Constructor sin parámetros.
        /// 
        /// Se utiliza cuando la aplicación se ejecuta normalmente.
        /// Permite que la View cree el Controller SIN conocer el Model,
        /// ya que el Controller se encarga internamente de instanciar el Store.
        /// 
        /// Este enfoque mantiene el desacoplamiento View ↔ Model.
        /// </summary>
        public TicketController()
        {
            _store = new TicketStore();
        }

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// 
        /// Se utiliza principalmente para:
        /// - Pruebas unitarias.
        /// - Simular distintos Stores.
        /// - Mayor flexibilidad en escenarios avanzados.
        /// </summary>
        public TicketController(TicketStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        /// <summary>
        /// Mapea una entidad del Model (Ticket) a un DTO del Controller (TicketDto).
        /// 
        /// Este método:
        /// - Evita que la View acceda directamente al Model.
        /// - Traduce enums del Model a enums del Controller.
        /// - Formatea datos para presentación en la UI.
        /// 
        /// Es un punto clave para mantener MVC limpio.
        /// </summary>
        private static TicketDto MapToDto(Ticket t)
        {
            return new TicketDto
            {
                // Identificador único del ticket
                Id = t.Id,

                // Datos básicos del ticket
                Titulo = t.Titulo,
                Descripcion = t.Descripcion,

                // Conversión de estado del Model al estado usado por la View
                Estado = (t.Estado == TicketStatus.Abierto)
                    ? TicketStatusDto.Abierto
                    : TicketStatusDto.Cerrado,

                // Fecha convertida a texto legible para la interfaz gráfica
                FechaCreacionTexto = t.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
            };
        }

        /// <summary>
        /// Caso de uso: Crear un nuevo ticket.
        /// 
        /// Flujo:
        /// 1. Valida los datos recibidos desde la View.
        /// 2. Solicita al Store la creación del ticket (Model).
        /// 3. Obtiene la lista actualizada de tickets.
        /// 4. Convierte los tickets a DTOs.
        /// 5. Devuelve el resultado a la View.
        /// </summary>
        public OperationResult<List<TicketDto>> CreateTicket(string titulo, string descripcion)
        {
            // Validación de negocio básica
            if (string.IsNullOrWhiteSpace(titulo))
                return OperationResult<List<TicketDto>>
                    .Fail("El título es obligatorio.");

            // Normalización de texto
            titulo = titulo.Trim();
            descripcion = (descripcion ?? string.Empty).Trim();

            // El Store se encarga de crear y almacenar el ticket
            _store.Add(titulo, descripcion);

            // Conversión del Model a DTO para la View
            var dtos = _store.GetAll()
                             .Select(MapToDto)
                             .ToList();

            return OperationResult<List<TicketDto>>
                .Ok(dtos, "Ticket creado correctamente.");
        }

        /// <summary>
        /// Caso de uso: Obtener todos los tickets.
        /// 
        /// La View utiliza este método para:
        /// - Cargar la lista inicial.
        /// - Refrescar la interfaz.
        /// </summary>
        public OperationResult<List<TicketDto>> GetTickets()
        {
            var dtos = _store.GetAll()
                             .Select(MapToDto)
                             .ToList();

            return OperationResult<List<TicketDto>>
                .Ok(dtos, "Listado cargado.");
        }

        /// <summary>
        /// Caso de uso: Cambiar el estado de un ticket.
        /// 
        /// Flujo:
        /// 1. Recibe el estado desde la View (DTO).
        /// 2. Convierte el estado DTO al enum del Model.
        /// 3. Solicita al Store el cambio de estado.
        /// 4. Devuelve la lista actualizada en forma de DTOs.
        /// </summary>
        public OperationResult<List<TicketDto>> ChangeStatus(int id, TicketStatusDto statusDto)
        {
            // Validación del identificador
            if (id <= 0)
                return OperationResult<List<TicketDto>>
                    .Fail("Debe seleccionar un ticket válido.");

            // Traducción del estado DTO al estado del Model
            TicketStatus statusModel =
                (statusDto == TicketStatusDto.Abierto)
                    ? TicketStatus.Abierto
                    : TicketStatus.Cerrado;

            // Solicitud de cambio al Model
            bool updated = _store.ChangeStatus(id, statusModel);

            if (!updated)
                return OperationResult<List<TicketDto>>
                    .Fail("No se encontró el ticket a modificar.");

            // Retorno de la lista actualizada
            var dtos = _store.GetAll()
                             .Select(MapToDto)
                             .ToList();

            return OperationResult<List<TicketDto>>
                .Ok(dtos, "Estado actualizado.");
        }
    }
}
