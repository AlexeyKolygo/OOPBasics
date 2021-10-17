using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Basics.Models
{
    public class User
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string InvertUserName(string UserName)
        {
            StringBuilder invertion = new StringBuilder();
            for (int i = 0; i < UserName.Length; i++)
            {
                var inversionchar = UserName[UserName.Length - i-1];
                invertion.Append(inversionchar);
            }
            var result = invertion.ToString();
            return result;
        }

        public List<User> UploadUsersFromFile()
        {
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "users.txt");
            var read = File.ReadAllLines(file);
            var userlist = new List<User>();
            foreach(var entry in read)
            {
                var line = entry.Split('&',StringSplitOptions.RemoveEmptyEntries);
                var user = new User {Name = line[0].Trim(), Email = line[1].Trim()};
                userlist.Add(user);
            }
            GenerateFileWithEmails(userlist);
            return userlist;

        }

        private void GenerateFileWithEmails(List<User> userlist)
        {

            string output = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "emails.txt");

            //if (File.Exists(output)) {File.Create(output);}
            
            foreach (var user in userlist)
            {
                using FileStream fs = new FileStream(output, FileMode.Append, FileAccess.Write);
                using StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(user.Email);
            }
            
        }
    }
}
