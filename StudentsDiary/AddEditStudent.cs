using System.Data;

namespace StudentsDiary
{

    public partial class AddEditStudent : Form
    {

      

        int _studentId;
        Student _student;

        FileHelper<List<Student>> _fileHelper =
             new FileHelper<List<Student>>(Program.FilePath);

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();

            _studentId = id;
            GetStudentData();
            tbFirstName.Select();
        }

        void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(s => s.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak uzytkownika o podanym Id");

                FillTextBoxes();
            }
        }

        void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technilogy;
            tbPhysics.Text = _student.Physics;
            rtbComents.Text = _student.Comments;
            tbPolishLang.Text = _student.PolishLang;
            tbForeingLang.Text = _student.ForeingLang;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
                AssignIdToNewStudent(students);

            AddNewUserToList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Math = tbMath.Text,
                Technilogy = tbTechnology.Text,
                Physics = tbPhysics.Text,
                Comments = rtbComents.Text,
                PolishLang = tbPolishLang.Text,
                ForeingLang = tbForeingLang.Text,
            };
            students.Add(student);
        }

        void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighterId = students
                    .OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighterId == null ?
               1 : studentWithHighterId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
