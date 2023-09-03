using System.ComponentModel.DataAnnotations;

namespace EFCoreEncryptSample
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //[Encrypted]
        //[Encrypted(StorageFormat.Binary)]
        public string Email { get; set; }

        //[Encrypted]
       // [Encrypted(StorageFormat.Base64)]
        public string Phone { get; set; }
    }
}
