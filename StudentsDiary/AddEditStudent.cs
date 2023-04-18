using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{

    public partial class AddEditStudent : Form
    {
        string _filePath =
        Path.Combine(Environment.CurrentDirectory, "students.txt");
        int _studentId;

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();

            _studentId = id;

            if (id != 0)
            {
                var students = DeserializeFromFile();
                var student = students.FirstOrDefault(s => s.Id == id);

                if (student == null)
                {
                    throw new Exception("Brak uzytkownika o podanym Id");
                }
                tbId.Text = student.Id.ToString();
                tbFirstName.Text = student.FirstName;
                tbLastName.Text = student.LastName;
                tbMath.Text = student.Math;
                tbTechnology.Text = student.Technilogy;
                tbPhysics.Text = student.Physics;
                rtbComents.Text = student.Comments;
                tbPolishLang.Text = student.PolishLang;
                tbForeingLang.Text = student.ForeingLang;
            }

            tbFirstName.Select();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
            {
                var studentWithHighterId = students
                    .OrderByDescending(x => x.Id).FirstOrDefault();

                _studentId = studentWithHighterId == null ?
                   1 : studentWithHighterId.Id++;
            }


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

            SerializeToFile(students);

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void SerializeToFile(List<Student> students)
        {
            var serializer = new XmlSerializer(typeof(List<Student>));
            using (var streamWriter = new StreamWriter(_filePath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }
        }

        public List<Student> DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<Student>();

            var serializer = new XmlSerializer(typeof(List<Student>));
            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (List<Student>)serializer.Deserialize(streamReader);
                streamReader.Close();
                return students;
            }
        }
    }
}
