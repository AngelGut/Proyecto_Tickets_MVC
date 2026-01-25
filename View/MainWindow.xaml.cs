using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Input;
using Controller;
using TicketApp.Application;
using TicketApp.Application.Controllers;
using TicketApp.Model;

namespace TicketApp
{
    public partial class MainWindow : Window
    {
        private readonly TicketController _controller;

        // Guardará el ticket seleccionado al hacer clic en una tarjeta
        private Ticket _ticketSeleccionado;

        public MainWindow()
        {
            InitializeComponent();

            var store = new TicketStore();
            _controller = new TicketController(store);

            RefrescarLista();
        }

        private void BtnCrear_Click(object sender, RoutedEventArgs e)
        {
            string titulo = TxtTitulo.Text;
            string descripcion = TxtDescripcion.Text;

            OperationResult<List<Ticket>> result = _controller.CreateTicket(titulo, descripcion);

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

            TicketStatus nuevoEstado = _ticketSeleccionado.Estado == TicketStatus.Abierto
                ? TicketStatus.Cerrado
                : TicketStatus.Abierto;

            var result = _controller.ChangeStatus(_ticketSeleccionado.Id, nuevoEstado);

            TxtMensaje.Text = result.Message;

            if (result.Success)
            {
                IcTickets.ItemsSource = result.Data;
                _ticketSeleccionado = null;
            }
        }

        private void RefrescarLista()
        {
            var result = _controller.GetTickets();
            if (result.Success)
                IcTickets.ItemsSource = result.Data;
        }

        // Se ejecuta cuando el usuario hace clic en una tarjeta
        private void Ticket_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Ticket ticket)
            {
                _ticketSeleccionado = ticket;
                TxtMensaje.Text = $"Ticket #{ticket.Id} seleccionado";
            }
        }
    }
}
