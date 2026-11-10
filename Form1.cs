using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CadastroClientesMysql
{
    public partial class Form1 : Form
    {
        private int idSelecionado = 0; // 0 = novo registro

        public Form1()
        {
            InitializeComponent();
            CarregarGrid();//o banco ja existe
        }

        private void CarregarGrid()
        {
            dgvClientes.DataSource =
            Banco.Consultar("SELECT Id, Nome, Email, Telefone, Cidade FROM clientes ORDER BY Nome");
            dgvClientes.Columns["Id"].Visible = false;
            lblTotal.Text = "Total de clientes: " + dgvClientes.Rows.Count;
        }

        private void LimparCampos()
        {
            idSelecionado = 0;
            txtNome.Clear(); txtEmail.Clear();
            txtTelefone.Clear(); txtCidade.Clear();
            txtNome.Focus();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validação simples
            if (txtNome.Text.Trim() == "")
            {
                MessageBox.Show("Informe o nome do cliente!", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            if (idSelecionado == 0)
            {
                // ===== INCLUSÃO =====
                Banco.ExecutarComando(
                    "INSERT INTO clientes (Nome, Email, Telefone, Cidade) " +
                    "VALUES (@Nome, @Email, @Telefone, @Cidade)",
                    new Dictionary<string, object> {
                { "@Nome", txtNome.Text.Trim() },
                { "@Email", txtEmail.Text.Trim() },
                { "@Telefone", txtTelefone.Text.Trim() },
                { "@Cidade", txtCidade.Text.Trim() }
                    });
                MessageBox.Show("Cliente incluído com sucesso!");
            }
            else
            {
                // ===== EDIÇÃO =====
                Banco.ExecutarComando(
                    "UPDATE clientes SET Nome = @Nome, Email = @Email, " +
                    "Telefone = @Telefone, Cidade = @Cidade WHERE Id = @Id",
                    new Dictionary<string, object> {
                { "@Id",   idSelecionado },
                { "@Nome", txtNome.Text.Trim() },
                { "@Email", txtEmail.Text.Trim() },
                { "@Telefone", txtTelefone.Text.Trim() },
                { "@Cidade", txtCidade.Text.Trim() }
                    });
                MessageBox.Show("Cliente atualizado com sucesso!");
            }

            LimparCampos();
            CarregarGrid();
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow linha = dgvClientes.Rows[e.RowIndex];
            idSelecionado = Convert.ToInt32(linha.Cells["Id"].Value);
            txtNome.Text = linha.Cells["Nome"].Value?.ToString();
            txtEmail.Text = linha.Cells["Email"].Value?.ToString();
            txtTelefone.Text = linha.Cells["Telefone"].Value?.ToString();
            txtCidade.Text = linha.Cells["Cidade"].Value?.ToString();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (idSelecionado == 0)
            {
                MessageBox.Show("Selecione um cliente na lista para excluir.");
                return;
            }

            DialogResult resp = MessageBox.Show(
                "Confirma a exclusão do cliente \"" + txtNome.Text + "\"?",
                "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                Banco.ExecutarComando(
                    "DELETE FROM clientes WHERE Id = @Id",
                    new Dictionary<string, object> { { "@Id", idSelecionado } });

                MessageBox.Show("Cliente excluído!");
                LimparCampos();
                CarregarGrid();
            }
        }

        private void button4_Click(object sender, EventArgs e) // btnBuscar
        {
            dgvClientes.DataSource = Banco.Consultar(
                "SELECT Id, Nome, Email, Telefone, Cidade FROM clientes " +
                "WHERE Nome LIKE @Busca ORDER BY Nome",
                new Dictionary<string, object> { { "@Busca", "%" + txtBusca.Text.Trim() + "%" } });
            dgvClientes.Columns["Id"].Visible = false;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            LimparCampos(); // prepara o formulário para uma nova inclusão
        }
    }
}
