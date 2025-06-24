using System;
using System.Windows.Forms;
using TaskSystemCatalogAdmin.Models;

namespace TaskSystemCatalogAdmin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Users
        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            var user = new UserModel
            {
                Username = txtUserName.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                PasswordHash = txtPassword.Text
            };

            var success = await ApiService.CreateUserAsync(user);

            if (success)
                MessageBox.Show("Usuario agregado correctamente.");
            else
                MessageBox.Show("Error al agregar el usuario.");
        }
        private async void btnLoadUsers_Click(object sender, EventArgs e)
        {
            var users = await ApiService.GetUsersAsync();
            dgvUsers.DataSource = users;
        }
        private async void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is UserModel selected)
            {
                string newFullName = Microsoft.VisualBasic.Interaction.InputBox("Nuevo nombre completo:", "Editar Usuario", selected.FullName); // JAMC Ejercicio básico, para no entrar tanto a detalles
                if (!string.IsNullOrWhiteSpace(newFullName))
                {
                    selected.FullName = newFullName;
                    var success = await ApiService.UpdateUserAsync(selected);
                    if (success) btnLoadUsers_Click(null, null);
                    else MessageBox.Show("Error al editar.");
                }
            }
        }
        private async void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is UserModel selected)
            {
                var confirm = MessageBox.Show($"¿Eliminar usuario '{selected.Username}'?", "Confirmar", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    var success = await ApiService.DeleteUserAsync(selected.Id);
                    if (success)
                    {
                        MessageBox.Show("Dato actualizado.");
                        btnLoadUsers_Click(null, null);
                    }
                    else 
                        MessageBox.Show("Error al eliminar.");
                }
            }
        }
        #endregion

        #region Priorities
        private async void btnAddPriority_ClickAsync(object sender, EventArgs e)
        {
            var priority = new PriorityModel
            {
                Name = txtPriorityName.Text.Trim(),
                Order = (int)numPriorityOrder.Value
            };

            var success = await ApiService.CreatePriorityAsync(priority);

            if (success)
                MessageBox.Show("Prioridad agregada correctamente.");
            else
                MessageBox.Show("Error al agregar la prioridad.");
        }
        private async void btnCargar_Click(object sender, EventArgs e)
        {
            var list = await ApiService.GetPrioritiesAsync();
            dgvPriorities.DataSource = list;
        }
        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvPriorities.CurrentRow?.DataBoundItem is PriorityModel selected)
            {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Nuevo nombre:", "Editar Prioridad", selected.Name); // JAMC Ejercicio básico, para no entrar tanto a detalles
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    selected.Name = newName;
                    var success = await ApiService.UpdatePriorityAsync(selected);
                    if (success) btnCargar_Click(null, null);
                    else MessageBox.Show("Error al editar.");
                }
            }
        }
        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPriorities.CurrentRow?.DataBoundItem is PriorityModel selected)
            {
                var confirm = MessageBox.Show($"¿Eliminar prioridad '{selected.Name}'?", "Confirmar", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    var success = await ApiService.DeletePriorityAsync(selected.Id);
                    if (success) btnCargar_Click(null, null);
                    else MessageBox.Show("Error al eliminar.");
                }
            }
        }
        #endregion
    }
}
