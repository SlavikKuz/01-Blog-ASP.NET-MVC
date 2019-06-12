using DataLibrary.BDAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Logik
{
    public static class UsersDB
    {
        public static void createUser(int _age, string _name, 
            string _lastname, string _email, string _password)
        {
            UsersModel user = new UsersModel
            {
                age = _age,
                name = _name,
                lastname = _lastname,
                email = _email,
                password = _password
            };

            string sql = @"INSERT INTO dbo.Users (age, name, lastname, email, password)
                    VALUES(@age,@name,@lastname,@email,@password)";

            SqlWorkflow.saveData(sql, user);
        }

        public static List<UsersModel> loadUsers()
        {
            string sql = @"SELECT * FROM dbo.Users";
            return SqlWorkflow.loadData<UsersModel>(sql);
        }
    }
}
