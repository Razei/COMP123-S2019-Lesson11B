using System;
using System.IO;
using System.Windows.Forms;

namespace COMP123_S2019_Lesson11B
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// this is the event handler for the MainForm's FormClosing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// This is the event handler for the Exit Menu Item's Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.aboutBox.ShowDialog();
        }

        private void HelpToolStripButton_Click(object sender, EventArgs e)
        {
            Program.aboutBox.ShowDialog();
        }

        private void ExitToolStripMenuButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'testDatabaseDataSet.StudentTable' table. You can move, or remove it, as needed.
            this.studentTableTableAdapter.Fill(this.testDatabaseDataSet.StudentTable);

        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            //display using LINQ
            //var studentList =
            //    from student in this.testDatabaseDataSet.StudentTable
            //    select student;

            //foreach (var student in studentList.ToList())
            //{
            //    Debug.WriteLine("Student Last Name: " + student.LastName);
            //}

            Program.studentInfoForm.Show();
            this.Hide();

        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // configure the file dialog
            StudentSaveFileDialog.FileName = "Student.txt";
            StudentSaveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            StudentSaveFileDialog.Filter = "Text Files (*.txt)|*.txt| All Files (*.*)|*.*";

            //open file dialog - Modal Form
            var result = StudentSaveFileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                //open file to write
                using (StreamWriter outputStream = new StreamWriter(
                    File.Open(StudentSaveFileDialog.FileName, FileMode.Create)))
                {
                    //write stuff to the file
                    outputStream.WriteLine(Program.student.id);
                    outputStream.WriteLine(Program.student.StudentID);
                    outputStream.WriteLine(Program.student.FirstName);
                    outputStream.WriteLine(Program.student.LastName);

                    //close the file
                    outputStream.Close();

                    //dispose of the memory
                    outputStream.Dispose();
                }

                MessageBox.Show("File Saved", "Saving...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //SelectionChanged event works on startup (without clicking)
        private void StudentTableDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //local aliases
            var rowindex = StudentTableDataGridView.CurrentCell.RowIndex;
            var rows = StudentTableDataGridView.Rows;
            var cells = rows[rowindex].Cells;
            var columnCount = StudentTableDataGridView.ColumnCount;


            string outputString = string.Empty;

            StudentTableDataGridView.Rows[rowindex].Selected = true;

            for (int index = 0; index < columnCount; index++)
            {
                outputString += cells[index].Value.ToString() + " ";
            }

            //if (MessageBox.Show(outputString, "Output String", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    //do something
            //}

            //Debug.WriteLine(outputString);

            SelectionLabel.Text = outputString;

            Program.student.id = int.Parse(cells[(int)StudentField.ID].Value.ToString());
            Program.student.StudentID = cells[(int)StudentField.STUDENT_ID].Value.ToString();
            Program.student.FirstName = cells[(int)StudentField.FIRST_NAME].Value.ToString();
            Program.student.LastName = cells[(int)StudentField.LAST_NAME].Value.ToString();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //configure file dialog
            StudentOpenFileDialog.FileName = "Student.txt";
            StudentOpenFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            StudentOpenFileDialog.Filter = "Text Files (*.txt)|*.txt| All Files (*.*)|*.*";

            //open the file dialog
            var result = StudentOpenFileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                try
                {
                    using (StreamReader InputStream = new StreamReader(
                    File.Open(StudentOpenFileDialog.FileName, FileMode.Open)))
                    {
                        //Read stuff into the class
                        Program.student.id = int.Parse(InputStream.ReadLine()); ;
                        Program.student.StudentID = InputStream.ReadLine();
                        Program.student.FirstName = InputStream.ReadLine();
                        Program.student.LastName = InputStream.ReadLine();

                        //cleanup
                        InputStream.Close();
                        InputStream.Dispose();

                        NextButton_Click(sender, e);
                    }
                }
                catch (IOException exception)
                {
                    MessageBox.Show("Error: " + exception.Message, "File I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private void SaveBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // configure the file dialog
            StudentSaveFileDialog.FileName = "Student.dat";
            StudentSaveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            StudentSaveFileDialog.Filter = "Binary Files (*.dat)|*.dat| All Files (*.*)|*.*";

            //open file dialog - Modal Form
            var result = StudentSaveFileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                //open file to write
                using (BinaryWriter outputStream = new BinaryWriter(
                    File.Open(StudentSaveFileDialog.FileName, FileMode.Create)))
                {
                    //write stuff to the file
                    outputStream.Write(Program.student.id.ToString());
                    outputStream.Write(Program.student.StudentID);
                    outputStream.Write(Program.student.FirstName);
                    outputStream.Write(Program.student.LastName);

                    //cleanup
                    outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();
                }

                MessageBox.Show("Binary File Saved", "Saving Binary File...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //configure file dialog
            StudentOpenFileDialog.FileName = "Student.dat";
            StudentOpenFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            StudentOpenFileDialog.Filter = "Binary Files (*.dat)|*.dat| All Files (*.*)|*.*";

            //open the file dialog
            var result = StudentOpenFileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                try
                {
                    using (BinaryReader InputStream = new BinaryReader(
                    File.Open(StudentOpenFileDialog.FileName, FileMode.Open)))
                    {
                        //Read stuff into the class
                        Program.student.id = int.Parse(InputStream.ReadString()); ;
                        Program.student.StudentID = InputStream.ReadString();
                        Program.student.FirstName = InputStream.ReadString();
                        Program.student.LastName = InputStream.ReadString();

                        //cleanup
                        InputStream.Close();
                        InputStream.Dispose();

                        NextButton_Click(sender, e);
                    }
                }
                catch (IOException exception)
                {
                    MessageBox.Show("Error: " + exception.Message, "File I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
    }
}
