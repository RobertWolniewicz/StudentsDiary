using StudentsDiary.Properties;

namespace StudentsDiary;

public partial class Main : Form
{
    FileHelper<List<Student>> _fileHelper =
        new FileHelper<List<Student>>(Program.FilePath);

    public bool IsMaximize
    {
        get
        {
            return Settings.Default.IsMaximalize;
        }
        set
        {
            Settings.Default.IsMaximalize = value;
        }
    }

    public Main()
    {
        InitializeComponent();
        SetComboBoxItems();
        RefreshDiary();
        SetColumnsHeader();

        dgvDiary.Columns[nameof(Student.Group)].Visible = false;

        if (IsMaximize)
        {
            WindowState = FormWindowState.Maximized;
        }
    }

    void RefreshDiary()
    {
        var students = _fileHelper.DeserializeFromFile();
        if (combGroup.SelectedItem.ToString() != "Wszyscy")
            students = students.Where(x => x.Group.ToString() == combGroup.SelectedItem.ToString()).ToList();

        students = students.OrderBy(x => x.Id).ToList();
        dgvDiary.DataSource = students;
    }
    void SetColumnsHeader()
    {
        dgvDiary.Columns[0].HeaderText = "Numer";
        dgvDiary.Columns[1].HeaderText = "Imie";
        dgvDiary.Columns[2].HeaderText = "Nazwisko";
        dgvDiary.Columns[3].HeaderText = "Uwagi";
        dgvDiary.Columns[4].HeaderText = "Matematyka";
        dgvDiary.Columns[5].HeaderText = "Technologia";
        dgvDiary.Columns[6].HeaderText = "Fizyka";
        dgvDiary.Columns[7].HeaderText = "Jêzyk polski";
        dgvDiary.Columns[8].HeaderText = "Jêzyk obcy";
        dgvDiary.Columns[9].HeaderText = "Zajêcia dodatkowe";
    }
    void SetComboBoxItems()
    {
        combGroup.Items.Add("Wszyscy");
        combGroup.Items.AddRange(Enum.GetValues(typeof(Group)).Cast<object>().ToArray());
        combGroup.SelectedIndex = 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        var addEditStudent = new AddEditStudent();
        addEditStudent.FormClosing += AddEditStudent_FormClosing;
        addEditStudent.ShowDialog();
    }

    private void AddEditStudent_FormClosing(object? sender, FormClosingEventArgs e)
    {
        RefreshDiary();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
        if (dgvDiary.SelectedRows.Count == 0)
        {
            MessageBox.Show("Proszê zaznacz ucznia, którego dane chcesz edytowaæ");
            return;
        }

        var addEditStudent = new AddEditStudent(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
        addEditStudent.FormClosing += AddEditStudent_FormClosing;
        addEditStudent.ShowDialog();

    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (dgvDiary.SelectedRows.Count == 0)
        {
            MessageBox.Show("Proszê zaznacz ucznia, którego dane chcesz usun¹æ");
            return;
        }

        var selectedStudent = dgvDiary.SelectedRows[0];

        var confirmDelete = MessageBox.Show($"Czy napewno chcesz usun¹æ ucznia " +
             $"{(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()}",
             "Usuwanie Ucznia",
             MessageBoxButtons.OKCancel);
        if (confirmDelete == DialogResult.OK)
        {
            DeleteStudent(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            RefreshDiary();
        }
    }

    void DeleteStudent(int id)
    {
        var students = _fileHelper.DeserializeFromFile();
        students.RemoveAll(x => x.Id == id);
        _fileHelper.SerializeToFile(students);
    }

    private void btnRefreshe_Click(object sender, EventArgs e)
    {
        RefreshDiary();
    }

    private void Main_FormClosed(object sender, FormClosedEventArgs e)
    {
        if (WindowState == FormWindowState.Maximized)
        {
            IsMaximize = true;
        }
        else
            IsMaximize = false;

        Settings.Default.Save();
    }

    private void combGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshDiary();
    }
}