using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// ============================================================
// Archivo: OperationResult.cs
// Proyecto: TicketApp.Application (Controller layer)
// Objetivo: Representar el resultado de una operación del Controller
//           sin depender de UI (sin MessageBox, sin controles, etc.)
// ===========================

namespace Controller
{

    /// <summary>
    /// Resultado genérico de una operación del Controller.
    /// - Success: indica si se ejecutó correctamente
    /// - Message: mensaje amigable para que la View lo muestre si quiere
    /// - Data: datos opcionales que la View necesita para refrescar UI
    /// </summary>
    public class OperationResult<T>
    {
        /// <summary>
        /// Indica si la operación se completó correctamente.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Mensaje asociado (éxito o error). La View decide si lo muestra.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Datos que devuelve la operación (por ejemplo, lista de tickets).
        /// Puede ser null si no hay datos que retornar.
        /// </summary>
        public T? Data { get; private set; }

        /// <summary>
        /// Constructor privado para obligar a usar los métodos estáticos.
        /// Esto asegura consistencia en cómo se crean resultados.
        /// </summary>
        private OperationResult(bool success, string message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Crea un resultado exitoso.
        /// </summary>
        public static OperationResult<T> Ok(T? data, string message = "Operación realizada correctamente.")
        {
            return new OperationResult<T>(true, message, data);
        }

        /// <summary>
        /// Crea un resultado fallido.
        /// </summary>
        public static OperationResult<T> Fail(string message)
        {
            return new OperationResult<T>(false, message, default);
        }
    }
}
