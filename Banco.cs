using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientesMysql
{
    public class Banco
    {
        // Servidor MySQL do XAMPP: localhost, porta 3306, usuário root, sem senha
        private static string connectionString =
            "Server=localhost;Port=3308;Database=cadastro;Uid=root;Pwd=;";

        // Executa comandos sem retorno (INSERT, UPDATE, DELETE)
        public static void ExecutarComando(string sql, Dictionary<string, object> parametros)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    foreach (var p in parametros)
                        cmd.Parameters.AddWithValue(p.Key, p.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Retorna dados (SELECT) usando um MySqlDataAdapter
        public static DataTable Consultar(string sql, Dictionary<string, object> parametros = null)
        {
            var tabela = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parametros != null)
                        foreach (var p in parametros)
                            cmd.Parameters.AddWithValue(p.Key, p.Value);

                    using (var adapter = new MySqlDataAdapter(cmd))
                        adapter.Fill(tabela);
                }
            }
            return tabela;
        }

        // Executa um comando e devolve o Id gerado (útil no INSERT)
        public static int ExecutarEObterId(string sql, Dictionary<string, object> parametros)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", parametros["@Nome"]);
                    foreach (var p in parametros)
                        cmd.Parameters.AddWithValue(p.Key, p.Value);
                    cmd.ExecuteNonQuery();
                    return (int)cmd.LastInsertedId;
                }
            }
        }
    }
}
