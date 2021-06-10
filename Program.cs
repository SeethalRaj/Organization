using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = @"..\organization.xml";

            if (File.Exists(fileName))
            {
                XmlDocument obj = new XmlDocument();
                obj.Load(fileName);
                XmlElement root = obj.DocumentElement;
                XmlNodeList nodelist = root.GetElementsByTagName("Unit");

                List<Organization> lstOrganization = getOrganizationList(nodelist);

                Console.WriteLine("Employees List in the ABC Inc.");

                Console.WriteLine(" ");

                foreach (var item in lstOrganization)
                {
                    Console.WriteLine("Unit Name: "+item.UnitName);

                    foreach (var emp in item.Employees)
                    {
                        Console.WriteLine("Employee Name : "+emp.Name + ", Title: " + emp.Title);
                    }

                    Console.WriteLine("------------------------------------------");
                }

                Console.WriteLine("Do you want to generate JSON?(Y/N)");

                string answer = Console.ReadLine();
                if (answer.ToLower() == "y")
                {
                    string json = JsonConvert.SerializeObject(listForJSON(lstOrganization));
                    File.WriteAllText(@"..\Organization.json", json);

                    Console.WriteLine("The JSON file have been created!");
                }
                
                Console.ReadLine();
            }
        }

        static List<Organization> getOrganizationList(XmlNodeList nodelist)
        {
            List<Organization> listOrg = new List<Organization>();

            foreach (XmlNode node in nodelist)
            {
                Organization orgObj = new Organization();
                orgObj.UnitName = node.Attributes["Name"].Value;
                List<Employee> lstEmp = new List<Employee>();
                var tets = node.SelectNodes("Employee");

                if (node.ChildNodes[0] != null)
                {
                    for (int i = 0; i <= node.SelectNodes("Employee").Count - 1; i++)
                    {

                        Employee empObj = new Employee();
                        empObj.Name = node.ChildNodes[i].InnerText;
                        empObj.Title = node.ChildNodes[i].Attributes["Title"].Value;
                        lstEmp.Add(empObj);
                    }

                }
                orgObj.Employees = lstEmp;
                listOrg.Add(orgObj);
            }

            return listOrg;
        }

        static List<Organization> listForJSON(List<Organization> listOrg)
        {
            List<Organization> listForJSON = new List<Organization>();

            foreach (Organization org in listOrg)
            {
                bool isSwitch = false;
                if (org.UnitName.Contains("Platform Team") && isSwitch == false)
                {
                    org.UnitName = "Maintenance Team";
                    isSwitch = true;
                }
                if (org.UnitName.Contains("Maintenance Team") && isSwitch == false)
                {
                    org.UnitName = "Platform Team";
                }
                listForJSON.Add(org);
            }

            return listForJSON;

        }
    }

    public class Organization
    {
        public string UnitName;
        public List<Employee> Employees;
    }

    public class Employee
    {
        public string Name;
        public string Title;
    }
}
