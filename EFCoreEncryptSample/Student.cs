using EntityFrameworkCore.EncryptColumn.Attribute;

namespace EFCoreEncryptSample
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [EncryptColumn]
        public string Email { get; set; }

        [EncryptColumn]
        public string Phone { get; set; }
    }
}
