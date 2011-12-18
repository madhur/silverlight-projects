using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OrgChart
{
    public class Person
    {
        public string ID;
        public string Name;
        public string ManagerID;
        public string Title;
        public string Department;
        public string Extension;
        public string Email;
        public string Field1;
        public string Field2;
        public string Field3;
        public string Field4;
        public string Field5;
        public string dotted;

        #region More Fileds
        public int Level = 1;
        public int SubNodes = 0;
        public int HiddenSubNodes = 0;
        public int NodeOrder = 1;
        public double MinChildWidth;
        public double X;
        public double StartX;
        public Boolean Opened = true;        
        public Boolean Collapsed = false;
        #endregion

        public static Person GetPerson(string id, string name, string managerID, string title, string department, string extension, string email)
        {
            Person p = new Person();
            p.ID = id;
            p.Name = name;
            p.ManagerID = managerID;
            p.Title = title;
            p.Department = department;
            p.Extension = extension;
            p.Email = email;
            return p;
        }

        
    }
}
