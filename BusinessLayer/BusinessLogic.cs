﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DataAccessLayer;

namespace BusinessLayer
{
    public class BusinessLogic
    {
        private DAL db = new DAL();
        private string query;
        SqlDataReader sdr;

        public string SignUp(User i)
        {
            try
            {
                if (checkAccount(i))
                {
                    query = @"Insert into Users Values('" + i.Name + "','" + i.Email + "','" + i.Password + "')";
                    db.IDU(query);
                    return "Successful";
                }
                else
                {
                    throw new Exception("Unsuccessful");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        private bool checkAccount(User i)   
        {
            try
            {
                var UserList = new List<User>();
                query = @"Select * from Users where Email='" + i.Email + "'";
                sdr = db.GetReader(query);
                // Storing Users in UserList
                while (sdr.Read())
                {
                    User user = new User();
                    user.ID = int.Parse(sdr[0].ToString());
                    user.Name = sdr[1].ToString();
                    user.Email = sdr[2].ToString();
                    user.Password = sdr[3].ToString();
                    UserList.Add(user);
                }
                // Checking any Matching Emails
                foreach (var u in UserList)
                {
                    if (i.Email.Equals(u.Email))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        public User SignIn(User i)
        {
            User user;
            query = @"Select * from Users where Email='" + i.Email + "' AND Password='" + i.Password + "'";

            sdr = db.GetReader(query);

            if (sdr.Read())
            {
                user = new User();
                user.ID = int.Parse(sdr[0].ToString());
                user.Name = sdr[1].ToString();
                user.Email = sdr[2].ToString();
                user.Password = sdr[3].ToString();
                sdr.Close();
                db.CloseConnection();
                return user;
            }

            return null;
        }

        // For Closing the Reader & Connection after SignIn (Ignore this for now)
        public void CloseReader()
        {
            sdr.Close();
            db.CloseConnection();
        }
    }
}
