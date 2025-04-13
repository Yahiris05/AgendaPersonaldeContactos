using AgendaContactos.Data;
using AgendaContactos.Models;
using System.Windows;
using System.Windows.Controls;

namespace AgendaContactos
{
    public partial class MainWindow : Window
    {
        private readonly ContactoRepository _repository = new ContactoRepository();

        public MainWindow()
        {
            InitializeComponent();
            CargarContactos();
        }

        private void CargarContactos()
        {
            dgContactos.ItemsSource = _repository.GetContactos();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var nuevoContacto = new Contacto
            {
                Nombre = txtNombre.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };

            _repository.AddContacto(nuevoContacto);
            CargarContactos();
            LimpiarCampos();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                EliminarContacto(id);
            }
        }

        private void BtnEliminarSeleccionado_Click(object sender, RoutedEventArgs e)
        {
            if (dgContactos.SelectedItem is Contacto contacto)
            {
                EliminarContacto(contacto.Id);
            }
            else
            {
                MessageBox.Show("Por favor selecciona un contacto primero",
                              "Advertencia",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
        }

        private void MenuItemEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgContactos.SelectedItem is Contacto contacto)
            {
                EliminarContacto(contacto.Id);
            }
        }

        private void EliminarContacto(int id)
        {
            var contacto = _repository.GetContactos().FirstOrDefault(c => c.Id == id);
            if (contacto != null)
            {
                var result = MessageBox.Show($"¿Estás seguro de eliminar a {contacto.Nombre}?",
                                           "Confirmar eliminación",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _repository.DeleteContacto(id);
                    CargarContactos();
                    MessageBox.Show("Contacto eliminado correctamente", "Éxito",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
        }
    }
}