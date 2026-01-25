using System;

namespace TicketApp.Model.Common
{
    /// <summary>
    /// Clase genérica que representa el resultado de una operación.
    /// 
    /// Su objetivo es encapsular:
    /// - Si la operación fue exitosa o no.
    /// - Un mensaje descriptivo para la interfaz de usuario.
    /// - Los datos resultantes de la operación (si existen).
    /// 
    /// Esta clase se utiliza como medio de comunicación entre el
    /// Controller y la View, evitando que la View tenga que
    /// manejar excepciones o lógica de negocio directamente.
    /// 
    /// De esta forma se mantiene el desacoplamiento propio del patrón MVC.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo de datos que devuelve la operación (por ejemplo: una lista de tickets).
    /// </typeparam>
    public class OperationResult<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// 
        /// Tiene setter privado para garantizar que solo la propia clase
        /// pueda modificar su valor, evitando inconsistencias desde fuera.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Mensaje descriptivo asociado al resultado de la operación.
        /// 
        /// Este mensaje está pensado para ser mostrado directamente en la View
        /// (por ejemplo, en un TextBlock o MessageBox).
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Datos resultantes de la operación.
        /// 
        /// Puede ser null en caso de fallo.
        /// Se utiliza un tipo genérico para que la clase sea reutilizable
        /// en distintos casos de uso.
        /// </summary>
        public T? Data { get; private set; }

        /// <summary>
        /// Constructor privado.
        /// 
        /// Se fuerza el uso de los métodos de fábrica (Ok / Fail)
        /// para asegurar que el objeto siempre se cree en un estado válido.
        /// </summary>
        private OperationResult(bool success, string message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Crea un resultado exitoso de una operación.
        /// 
        /// Se utiliza cuando el caso de uso se ejecuta correctamente
        /// y se desea devolver información a la View.
        /// </summary>
        /// <param name="data">Datos resultantes de la operación.</param>
        /// <param name="message">Mensaje opcional de éxito.</param>
        /// <returns>
        /// Instancia de <see cref="OperationResult{T}"/> con Success = true.
        /// </returns>
        public static OperationResult<T> Ok(T data, string message = "Operación exitosa")
        {
            return new OperationResult<T>(true, message, data);
        }

        /// <summary>
        /// Crea un resultado fallido de una operación.
        /// 
        /// Se utiliza cuando ocurre una validación incorrecta,
        /// un error lógico o una condición que impide completar el caso de uso.
        /// </summary>
        /// <param name="message">Mensaje descriptivo del error.</param>
        /// <returns>
        /// Instancia de <see cref="OperationResult{T}"/> con Success = false.
        /// </returns>
        public static OperationResult<T> Fail(string message)
        {
            return new OperationResult<T>(false, message, default);
        }
    }
}
