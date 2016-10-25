using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Visitantes_Novo
{
    public partial class FrmCadAluno : Form
    {
        private MySqlConnection mConn;
        public FrmCadAluno()
        {
            InitializeComponent();
        }

        private void encheDataGridView()
        {
            try
            {
                MySqlConnection mConn2 = new MySqlConnection("Persist Security Info=False; server=192.168.6.100;database=intencoes_matricula;uid=admin;password=yb5731");
                // Abre a conexão
                mConn2.Open();

                //Query SQL
                MySqlCommand command = new MySqlCommand("SELECT id_aluno as Codigo, nome_aluno as 'Nome do Aluno', "
                                                       +"dataNasc_aluno as 'Data de Nascimento', escola_origem as 'Escola de Origem' "
                                                       +"FROM ALUNO", mConn2);
                //obtem um datareader
                MySqlDataReader dr = command.ExecuteReader();

                //Obtem o número de colunas
                int nColunas = dr.FieldCount;

                //percorre as colunas obtendo o seu nome e incluindo no DataGridView
                for (int i = 0; i < nColunas; i++)
                {
                    dataGridView1.Columns.Add(dr.GetName(i).ToString(), dr.GetName(i).ToString());
                }
                //define um array de strings com nCOlunas
                string[] linhaDados = new string[nColunas];
                //percorre o DataRead
                while (dr.Read())
                {
                    //percorre cada uma das colunas
                    for (int a = 0; a < nColunas; a++)
                    {
                        //verifica o tipo de dados da coluna
                        if (dr.GetFieldType(a).ToString() == "System.Int32")
                        {
                            linhaDados[a] = dr.GetInt32(a).ToString();
                        }
                        if (dr.GetFieldType(a).ToString() == "System.String")
                        {
                            linhaDados[a] = dr.GetString(a).ToString();
                        }
                        if (dr.GetFieldType(a).ToString() == "System.DateTime")
                        {
                            linhaDados[a] = dr.GetDateTime(a).ToString();
                        }
                    }
                    //atribui a linha ao datagridview
                    dataGridView1.Rows.Add(linhaDados);
                }
            }
            catch (MySqlException e)
            {
                MessageBox.Show(""+e);
            }
        }
        
        private void FrmCadAluno_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            encheDataGridView();
        }

        private void limpaDataGridView()
        {
            dataGridView1.Columns.Clear(); //Remover as colunas
            dataGridView1.Rows.Clear();    //Remover as linhas
            dataGridView1.Refresh();    //Para a grid se actualizar
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtNomeAluno.Text = "";
            txtDataNasc.Text = "";
            txtEscolaOrigem.Text = "";
            txtIdade.Text = "";
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            txtNomeAluno.CharacterCasing = CharacterCasing.Upper;
            txtEscolaOrigem.CharacterCasing = CharacterCasing.Upper;

            string dataBancoMysql = Convert.ToDateTime(txtDataNasc.Text).ToString("yyyy-MM-dd");

            try
            {
                mConn = new MySqlConnection("Persist Security Info=False; server=192.168.6.100;database=intencoes_matricula;uid=admin;password=yb5731");
                // Abre a conexão
                mConn.Open();

                //Query SQL
                MySqlCommand command = new MySqlCommand("INSERT INTO aluno (nome_aluno, dataNasc_aluno, escola_origem) "
                                                       +"VALUES('"+txtNomeAluno.Text+"', '"+dataBancoMysql+"', '"+txtEscolaOrigem.Text+"')", mConn);

                //Executa a Query SQL
                command.ExecuteNonQuery();

                // Fecha a conexão
                mConn.Close();

                //Mensagem de Sucesso
                MessageBox.Show("Gravado com Sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLimpar_Click(sender, e);

                limpaDataGridView();
                encheDataGridView();
            }
            catch (MySqlException msqle)
            {
                MessageBox.Show("Erro ao gravar! " + msqle.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}