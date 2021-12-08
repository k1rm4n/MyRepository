using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace MyLibrarySchool
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilderLibrary = null;

        private SqlDataAdapter sqlDataAdapterLibrary = null;

        private DataSet dataSetLibrary = null;


        private SqlCommandBuilder sqlBuilderStudents = null;

        private SqlDataAdapter sqlDataAdapterStudents = null;

        private DataSet dataSetStudents = null;

        private bool newRowAdding = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData_Students()
        {
            try
            {
                sqlDataAdapterStudents = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM Students", sqlConnection);

                sqlBuilderStudents = new SqlCommandBuilder(sqlDataAdapterStudents);

                sqlBuilderStudents.GetInsertCommand();
                sqlBuilderStudents.GetUpdateCommand();
                sqlBuilderStudents.GetDeleteCommand();

                dataSetStudents = new DataSet();

                sqlDataAdapterStudents.Fill(dataSetStudents, "Students");

                dataGridView1.DataSource = dataSetStudents.Tables["Students"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[5, i] = linkCell;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData_Library()
        {
            try
            {
                sqlDataAdapterLibrary = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM Library", sqlConnection);

                sqlBuilderLibrary = new SqlCommandBuilder(sqlDataAdapterLibrary);

                sqlBuilderLibrary.GetInsertCommand();
                sqlBuilderLibrary.GetUpdateCommand();
                sqlBuilderLibrary.GetDeleteCommand();

                dataSetLibrary = new DataSet();

                sqlDataAdapterLibrary.Fill(dataSetLibrary, "Library");

                dataGridView2.DataSource = dataSetLibrary.Tables["Library"];

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView2[3, i] = linkCell;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData_Students()
        {
            try
            {
                dataSetStudents.Tables["Students"].Clear();

                sqlDataAdapterStudents.Fill(dataSetStudents, "Students");

                dataGridView1.DataSource = dataSetStudents.Tables["Students"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[5, i] = linkCell;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ReloadData_Library()
        {
            try
            {
                dataSetLibrary.Tables["Library"].Clear();

                sqlDataAdapterLibrary.Fill(dataSetLibrary, "Library");

                dataGridView2.DataSource = dataSetLibrary.Tables["Library"];

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView2[3, i] = linkCell;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);

            sqlConnection.Open();

            LoadData_Students();
            LoadData_Library();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSetStudents.Tables["Students"].Rows[rowIndex].Delete();

                            sqlDataAdapterStudents.Update(dataSetStudents, "Students");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSetStudents.Tables["Students"].NewRow();
                        row["name"] = dataGridView1.Rows[rowIndex].Cells["name"].Value;
                        row["surname"] = dataGridView1.Rows[rowIndex].Cells["surname"].Value;
                        row["class"] = dataGridView1.Rows[rowIndex].Cells["class"].Value;
                        row["id_book"] = dataGridView1.Rows[rowIndex].Cells["id_book"].Value;
                       

                        dataSetStudents.Tables["Students"].Rows.Add(row);

                        dataSetStudents.Tables["Students"].Rows.RemoveAt(dataSetStudents.Tables["Students"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = "Delete";

                        sqlDataAdapterStudents.Update(dataSetStudents, "Students");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;
                        dataSetStudents.Tables["Students"].Rows[r]["name"] = dataGridView1.Rows[r].Cells["name"].Value;
                        dataSetStudents.Tables["Students"].Rows[r]["surname"] = dataGridView1.Rows[r].Cells["surname"].Value;
                        dataSetStudents.Tables["Students"].Rows[r]["class"] = dataGridView1.Rows[r].Cells["class"].Value;
                        dataSetStudents.Tables["Students"].Rows[r]["id_book"] = dataGridView1.Rows[r].Cells["id_book"].Value;

                        sqlDataAdapterStudents.Update(dataSetStudents, "Students");

                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = "Delete";

                    }
                }
                ReloadData_Students();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[5, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[5, rowIndex] = linkCell;

                    editingRow.Cells["Delete"].Value = "Update";
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    string task = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView2.Rows.RemoveAt(rowIndex);

                            dataSetLibrary.Tables["Library"].Rows[rowIndex].Delete();

                            sqlDataAdapterLibrary.Update(dataSetLibrary, "Library");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView2.Rows.Count - 2;

                        DataRow row = dataSetLibrary.Tables["Library"].NewRow();

                        row["Id"] = dataGridView2.Rows[rowIndex].Cells["Id"].Value;
                        row["name_book"] = dataGridView2.Rows[rowIndex].Cells["name_book"].Value;
                        row["author"] = dataGridView2.Rows[rowIndex].Cells["author"].Value;


                        dataSetLibrary.Tables["Library"].Rows.Add(row);

                        dataSetLibrary.Tables["Library"].Rows.RemoveAt(dataSetLibrary.Tables["Library"].Rows.Count - 1);

                        dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 2);

                        dataGridView2.Rows[e.RowIndex].Cells[3].Value = "Delete";

                        sqlDataAdapterLibrary.Update(dataSetLibrary, "Library");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSetLibrary.Tables["Library"].Rows[r]["Id"] = dataGridView2.Rows[r].Cells["Id"].Value;
                        dataSetLibrary.Tables["Library"].Rows[r]["name_book"] = dataGridView2.Rows[r].Cells["name_book"].Value;
                        dataSetLibrary.Tables["Library"].Rows[r]["author"] = dataGridView2.Rows[r].Cells["author"].Value;

                        sqlDataAdapterLibrary.Update(dataSetStudents, "Library");

                        dataGridView2.Rows[e.RowIndex].Cells[3].Value = "Delete";

                    }

                    ReloadData_Library();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView2.Rows.Count - 2;

                    DataGridViewRow row = dataGridView2.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView2[3, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView2.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView2.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView2[3, rowIndex] = linkCell;

                    editingRow.Cells["Delete"].Value = "Update";
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData_Students();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ReloadData_Library();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                            $"SELECT Students.name, Students.surname, Students.class, Library.name_book, Library.author FROM Students JOIN Library ON Students.id_book = Library.Id WHERE Students.id_book = {textBox1.Text}", sqlConnection);

            DataSet dataSet = new DataSet();

            sqlDataAdapter.Fill(dataSet);

            dataGridView3.DataSource = dataSet.Tables[0].DefaultView;
        }
    }
}
