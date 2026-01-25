using System;

namespace TicketApp.Model.Common
{

    // esto lo usamos para  comunicar información entre el Controller y la View
    //sin acoplar la lógica de negocio a la interfaz gráfica.



    public class OperationResult<T>
    {
        //usamos private set para que solo se pueda modificar desde dentro de la clase o en metodos comtrolados 
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T? Data { get; private set; }

        private OperationResult(bool success, string message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static OperationResult<T> Ok(T data, string message = "Operación exitosa")
        {
            return new OperationResult<T>(true, message, data);
        }

        public static OperationResult<T> Fail(string message)
        {
            return new OperationResult<T>(false, message, default);
        }
    }
}