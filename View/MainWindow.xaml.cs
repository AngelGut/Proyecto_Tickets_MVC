using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using Controller; // SOLO Controller. Nada de Model.

namespace TicketApp
{
    public partial class MainWindow : Window
    {
        private readonly TicketController _controller;

        // La View guarda el seleccionado como DTO (no como Model)
        private TicketDto? _ticketSeleccionado;

        public MainWindow()
        {
            InitializeComponent();

            // ✅ La View crea el controller.
            // IMPORTANTÍSIMO: Para crear el Store, la View NO debe conocer Model.
            // Solución limpia: el Controller expone un "factory" o constructor sin parámetros,
            // o inyectas desde App startup.
            //
            // Como tú estás en evaluación, te doy una opción limpia:
            // Crear el store dentro del Controller (ver nota abajo).

            InitializeComponent();

            // ✅ INICIALIZA el controller (ya tienes el constructor sin parámetros)
            _controller = new TicketController();

            // ✅ opcional: cargar lista al iniciar
            RefrescarLista();
        }

        private void BtnCrear_Click(object sender, RoutedEventArgs e)
        {
            string titulo = TxtTitulo.Text;
            string descripcion = TxtDescripcion.Text;

            OperationResult<List<TicketDto>> result = _controller.CreateTicket(titulo, descripcion);

            TxtMensaje.Text = result.Message;

            if (result.Success)
            {
                IcTickets.ItemsSource = result.Data;
                TxtTitulo.Clear();
                TxtDescripcion.Clear();
            }
        }

        private void BtnCambiarEstado_Click(object sender, RoutedEventArgs e)
        {
            if (_ticketSeleccionado == null)
            {
                TxtMensaje.Text = "Selecciona un ticket primero.";
                return;
            }

            TicketStatusDto nuevoEstado = (_ticketSeleccionado.Estado == TicketStatusDto.Abierto)
                ? TicketStatusDto.Cerrado
                : TicketStatusDto.Abierto;

            var result = _controller.ChangeStatus(_ticketSeleccionado.Id, nuevoEstado);

            TxtMensaje.Text = result.Message;

            if (result.Success)
            {
                IcTickets.ItemsSource = result.Data;
                _ticketSeleccionado = null;
            }
        }

        private void Ticket_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TicketDto ticket)
            {
                _ticketSeleccionado = ticket;
                TxtMensaje.Text = $"Ticket #{ticket.Id} seleccionado";
            }
        }

        private void RefrescarLista()
        {
            var result = _controller.GetTickets();
            if (result.Success)
                IcTickets.ItemsSource = result.Data;
        }

    }
}
