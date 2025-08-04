namespace MyWebAPI.Services
{
    public class SomeService
    {
        string[] Id = {"A01", "A02", "A03"};

        string[] Name = { "Apple", "Banana", "Cherry" };

        int[] Age = { 14, 15, 16 };

        public string[] getAllStudents()
        {
            string[] students = new string[Id.Length];
            for (int i=0; i<Id.Length; i++)
            {
                students[i] = $"{Id[i]} - {Name[i]} - {Age[i]}";
            }
            return students;
        }

        public string getStudent(string id)
        {
            int i = Array.IndexOf(Id, id);

            if (i < 0 || i >= Id.Length)
            {
                return "沒有找到任何資料";
            }


            return $"{Id[i]} - {Name[i]} - {Age[i]}";
        }
    }
}
