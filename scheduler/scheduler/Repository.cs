using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using utilities.data;

namespace scheduler.data
{
    public class Repository : IRepository
    {
        private IDbConnection Connection { get; set; }

        private Repository() { }

        public static Repository Create(IDbConnection cn)
        {
            return new Repository() { Connection = cn };
        }

        private List<IAssignment> assignments;
        public List<IAssignment> Assignments
        {
            get { return assignments; }
            set { assignments = value; }
        }

        private List<IEmployee> employees;
        public List<IEmployee> Employees
        {
            get { return employees; }
            set { employees = value; }
        }

        private void EmptyEmployeesTable()
        {
            const string deleteCommand = "TRUNCATE TABLE EMPLOYEES";
            DataInteraction.ExecuteNonQuery(deleteCommand, Connection);
        }

        private void EmptyContactsTable()
        {
            const string deleteCommand = "TRUNCATE TABLE CONTACTS";
            DataInteraction.ExecuteNonQuery(deleteCommand, Connection);
        }

        private void EmptyAssignmentsTable()
        {
            const string deleteCommand = "TRUNCATE TABLE ASSIGNMENTS";
            DataInteraction.ExecuteNonQuery(deleteCommand, Connection);
        }

        private List<SqlParameter> GetContactParameters(IEmployee employee)
        {
            var parameters = new List<SqlParameter>
                {
                    DataInteraction.CreateSqlParameter("@phone", employee.Contact.Phone),
                    DataInteraction.CreateSqlParameter("@email", employee.Contact.Email),
                    DataInteraction.CreateSqlParameter("@address", employee.Contact.Address)
                };
            return parameters;
        }

        private void InsertContact(List<SqlParameter> parameters)
        {
            const string insertContactCommand = "INSERT INTO CONTACTS (phone, email, address) VALUES (@phone,@email,@address)";
            DataInteraction.ExecuteNonQuery(insertContactCommand, parameters, Connection);
        }

        private List<SqlParameter> GetEmployeeParameters(IEmployee employee)
        {
            var parameters = new List<SqlParameter>
                {
                    DataInteraction.CreateSqlParameter("@first", employee.First),
                    DataInteraction.CreateSqlParameter("@last", employee.Last),
                    DataInteraction.CreateSqlParameter("@initials", employee.Initials),
                    DataInteraction.CreateSqlParameter("@start", employee.Start),
                    DataInteraction.CreateSqlParameter("@contactId", employee.Contact.Id)
                };
            return parameters;
        }

        private void InsertEmployee(List<SqlParameter> parameters)
        {
            const string insertEmployeeCommand = "INSERT INTO EMPLOYEES (First, Last, Initials, Start, ContactId) VALUES (@first,@last,@initials,@start,@contactId)";
            DataInteraction.ExecuteNonQuery(insertEmployeeCommand, parameters, Connection);
        }

        private int GetContactIdForCurrentEmployee(IEmployee employee)
        {
            var phone = DataInteraction.CreateSqlParameter("@phone", employee.Contact.Phone);
            var contactDt = DataInteraction.CreateDataTable("SELECT * FROM CONTACTS WHERE Phone=@phone", new List<SqlParameter> { phone }, Connection);
            var contactId = (int)contactDt.Rows[0]["Id"];
            return contactId;
        }

        private List<SqlParameter> GetAssignmentParameters(IAssignment assignment)
        {
            var parameters = new List<SqlParameter>
                {
                    DataInteraction.CreateSqlParameter("@first", assignment.Date),
                    DataInteraction.CreateSqlParameter("@last", assignment.Employee.Id),
                    DataInteraction.CreateSqlParameter("@role", assignment.Role)
                };
            return parameters;
        }

        private void InsertAssignment(List<SqlParameter> parameters)
        {
            
        }

        public void SaveEmployees()
        {
            EmptyEmployeesTable();
            EmptyContactsTable();

            foreach (var employee in Employees)
            {
                var contactParameters = GetContactParameters(employee);
                InsertContact(contactParameters);

                employee.Contact.Id = GetContactIdForCurrentEmployee(employee);

                var employeeParameters = GetEmployeeParameters(employee);
                InsertEmployee(employeeParameters);
            }
        }

        public List<IEmployee> GetEmployees()
        {
            DataInteraction.CreateDataTable("SELECT * FROM Employees", Connection);
            return null;
        }

        public void SaveAssignments()
        {
            EmptyAssignmentsTable();

            foreach (var assignment in Assignments        )
            {
                var assignmentParameters = GetAssignmentParameters(assignment);
                InsertAssignment(assignmentParameters);
            }
        }

        public List<IAssignment> GetAssignments()
        {
            throw new NotImplementedException();
        }

        public void Seed()
        {
            Employees = new List<IEmployee>();

            var SA = Employee.Create("Shane", "Adams", "SA", DateTime.Now, "417-499-3116", "", ""); Employees.Add(SA);
            var StB = Employee.Create("Steve", "Boeger", "StB", DateTime.Now, "417-624-3192", "", ""); Employees.Add(StB);
            var SB = Employee.Create("Susie", "Boeger", "SB", DateTime.Now, "417-624-3192", "", ""); Employees.Add(SB);
            var RoB = Employee.Create("Robin", "Boyd", "RoB", DateTime.Now, "417-624-2949", "", ""); Employees.Add(RoB);
            var RB = Employee.Create("Rick", "Brown", "RB", DateTime.Now, "417-496-3793", "", ""); Employees.Add(RB);
            var CC = Employee.Create("Chris", "Crabtree", "CC", DateTime.Now, "417-830-6644", "", ""); Employees.Add(CC);
            var CE = Employee.Create("Cary", "Edwards", "CE", DateTime.Now, "417-619-5745", "", ""); Employees.Add(CE);
            var RE = Employee.Create("Ray", "Eisenmann", "RE", DateTime.Now, "417-425-7644", "", ""); Employees.Add(RE);
            var NF = Employee.Create("Nancy", "Fiscus", "NF", DateTime.Now, "417-781-4152", "", ""); Employees.Add(NF);
            var CG = Employee.Create("Chris", "Garde", "CG", DateTime.Now, "417-782-7869", "", ""); Employees.Add(CG);
            var JH = Employee.Create("John", "Howard", "JH", DateTime.Now, "417-317-0777", "", ""); Employees.Add(JH);
            var CH = Employee.Create("Cole", "Hughes", "CH", DateTime.Now, "417-728-4793", "", ""); Employees.Add(CH);
            var SM = Employee.Create("Shelly", "Mabe", "SM", DateTime.Now, "417-766-6916", "", ""); Employees.Add(SM);
            var EM = Employee.Create("Ed", "Messer", "EM", DateTime.Now, "417-381-6317", "", ""); Employees.Add(EM);
            var LS = Employee.Create("Leanna", "Swager", "LS", DateTime.Now, "417-358-4426", "", ""); Employees.Add(LS);
            var YS = Employee.Create("Yvonne", "Stanke", "YS", DateTime.Now, "417-862-9887", "", ""); Employees.Add(YS);
            var JT = Employee.Create("Jim", "Tyler", "JT", DateTime.Now, "417-206-8005", "", ""); Employees.Add(JT);
            var CW = Employee.Create("Curt", "Williams", "CW", DateTime.Now, "417-833-9007", "", ""); Employees.Add(CW);

            Assignments = new List<IAssignment>
            {
                //March 1st
                Assignment.Create(Role.PM, SA,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,1)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,1)),
                Assignment.Create(Role.AM, CE,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,3,1)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,1)),
                 //March 2nd
                Assignment.Create(Role.PM, SA,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num04, StB,new DateTime(2015,3,2)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,2)),
                Assignment.Create(Role.PC, RoB,new DateTime(2015,3,2)),
                Assignment.Create(Role.V, RB,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num08, CC,new DateTime(2015,3,2)),
                Assignment.Create(Role.PC, CE,new DateTime(2015,3,2)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,3,2)),
                Assignment.Create(Role.C2, NF,new DateTime(2015,3,2)),
                Assignment.Create(Role.SevenToTen, CG,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num07, JH,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num01, CH,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num09, SM,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num02, EM,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num05, LS,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num06, YS,new DateTime(2015,3,2)),
                Assignment.Create(Role.Num03, JT,new DateTime(2015,3,2)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,3,2)),
                 //March 3rd
                Assignment.Create(Role.PM, SA,new DateTime(2015,3,3)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,3)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num07, RoB,new DateTime(2015,3,3)),
                Assignment.Create(Role.V, RB,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num04, CC,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num013, CE,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num01, NF,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num09, CG,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num05, JH,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num012, CH,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num03, SM,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num010, EM,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num08, LS,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num06, YS,new DateTime(2015,3,3)),
                Assignment.Create(Role.Num011, JT,new DateTime(2015,3,3)),
                Assignment.Create(Role.C2, CW,new DateTime(2015,3,3)),
                 //March 4th
                Assignment.Create(Role.PM, SA,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,3,4)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num04, RoB,new DateTime(2015,3,4)),
                Assignment.Create(Role.V, RB,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num09, CC,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num03, CE,new DateTime(2015,3,4)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,3,4)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num08, CG,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num01, JH,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num07, CH,new DateTime(2015,3,4)),
                Assignment.Create(Role.C2, SM,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num05, EM,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num06, LS,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num010, YS,new DateTime(2015,3,4)),
                Assignment.Create(Role.Num011, JT,new DateTime(2015,3,4)),
                Assignment.Create(Role.SevenToOne, CW,new DateTime(2015,3,4)),
                 //March 5th
                Assignment.Create(Role.PM, SA,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num04, StB,new DateTime(2015,3,5)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,5)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,3,5)),
                Assignment.Create(Role.V, RB,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num07, CC,new DateTime(2015,3,5)),
                Assignment.Create(Role.C2, CE,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num012, NF,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num06, CG,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num011, JH,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num09, CH,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num01, SM,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num010, EM,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num08, LS,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num03, YS,new DateTime(2015,3,5)),
                Assignment.Create(Role.Num05, JT,new DateTime(2015,3,5)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,5)),
                 //March 6th
                Assignment.Create(Role.PC, SA,new DateTime(2015,3,6)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,6)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,3,6)),
                Assignment.Create(Role.V, RB,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num04, CC,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num01, CE,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num07, RE,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num08, NF,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num05, CG,new DateTime(2015,3,6)),
                Assignment.Create(Role.PM, JH,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num03, CH,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num011, SM,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num09, EM,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num06, LS,new DateTime(2015,3,6)),
                Assignment.Create(Role.C2, YS,new DateTime(2015,3,6)),
                Assignment.Create(Role.Num010, JT,new DateTime(2015,3,6)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,6)),
                 //March 7th
                Assignment.Create(Role.Off, SA,new DateTime(2015,3,7)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,3,7)),
                Assignment.Create(Role.SevenToThree, NF,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,7)),
                Assignment.Create(Role.PM, JH,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,3,7)),
                Assignment.Create(Role.AM, YS,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,3,7)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,7)),
                 //March 8th
                Assignment.Create(Role.Off, SA,new DateTime(2015,3,8)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,8)),
                Assignment.Create(Role.PM, JH,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,3,8)),
                Assignment.Create(Role.AM, YS,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,3,8)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,8)),
                 //March 9th
                Assignment.Create(Role.Num07, SA,new DateTime(2015,3,9)),
                Assignment.Create(Role.PC, StB,new DateTime(2015,3,9)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,9)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,3,9)),
                Assignment.Create(Role.Num01, RB,new DateTime(2015,3,9)),
                Assignment.Create(Role.C2, CC,new DateTime(2015,3,9)),
                Assignment.Create(Role.V, CE,new DateTime(2015,3,9)),
                Assignment.Create(Role.Num04, RE,new DateTime(2015,3,9)),
                Assignment.Create(Role.Num03, NF,new DateTime(2015,3,9)),
                Assignment.Create(Role.Num05, CG,new DateTime(2015,3,9)),
                Assignment.Create(Role.PM, JH,new DateTime(2015,3,9)),
                Assignment.Create(Role.Num06, CH,new DateTime(2015,3,9)),
                Assignment.Create(Role.Num02, SM,new DateTime(2015,3,9)),
                Assignment.Create(Role.Num08, EM,new DateTime(2015,3,9)),
                Assignment.Create(Role.V, LS,new DateTime(2015,3,9)),
                Assignment.Create(Role.PC, YS,new DateTime(2015,3,9)),
                Assignment.Create(Role.V, JT,new DateTime(2015,3,9)),
                Assignment.Create(Role.V, CW,new DateTime(2015,3,9)),
                 //March 10th
                Assignment.Create(Role.Num05, SA,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num07, StB,new DateTime(2015,3,10)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num06, RB,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num01, CC,new DateTime(2015,3,10)),
                Assignment.Create(Role.V, CE,new DateTime(2015,3,10)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,3,10)),
                Assignment.Create(Role.C2, NF,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num03, CG,new DateTime(2015,3,10)),
                Assignment.Create(Role.PM, JH,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num08, CH,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num09, SM,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num04, EM,new DateTime(2015,3,10)),
                Assignment.Create(Role.V, LS,new DateTime(2015,3,10)),
                Assignment.Create(Role.Num010, YS,new DateTime(2015,3,10)),
                Assignment.Create(Role.V, JT,new DateTime(2015,3,10)),
                Assignment.Create(Role.V, CW,new DateTime(2015,3,10)),
                 //March 11th
                Assignment.Create(Role.Num05, SA,new DateTime(2015,3,11)),
                Assignment.Create(Role.Num04, StB,new DateTime(2015,3,11)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,11)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,3,11)),
                Assignment.Create(Role.Num07, RB,new DateTime(2015,3,11)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,11)),
                Assignment.Create(Role.V, CE,new DateTime(2015,3,11)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,3,11)),
                Assignment.Create(Role.Num01, NF,new DateTime(2015,3,11)),
                Assignment.Create(Role.C2, CG,new DateTime(2015,3,11)),
                Assignment.Create(Role.PM, JH,new DateTime(2015,3,11)),
                Assignment.Create(Role.Num06, CH,new DateTime(2015,3,11)),
                Assignment.Create(Role.Num08, SM,new DateTime(2015,3,11)),
                Assignment.Create(Role.Num09, EM,new DateTime(2015,3,11)),
                Assignment.Create(Role.V, LS,new DateTime(2015,3,11)),
                Assignment.Create(Role.Num03, YS,new DateTime(2015,3,11)),
                Assignment.Create(Role.V, JT,new DateTime(2015,3,11)),
                Assignment.Create(Role.V, CW,new DateTime(2015,3,11)),
                 //March 12th
                Assignment.Create(Role.Num07, SA,new DateTime(2015,3,12)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,12)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num05, RB,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num09, CC,new DateTime(2015,3,12)),
                Assignment.Create(Role.V, CE,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num06, RE,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num010, NF,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num01, CG,new DateTime(2015,3,12)),
                Assignment.Create(Role.PM, JH,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num08, CH,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num04, SM,new DateTime(2015,3,12)),
                Assignment.Create(Role.Num03, EM,new DateTime(2015,3,12)),
                Assignment.Create(Role.V, LS,new DateTime(2015,3,12)),
                Assignment.Create(Role.C2, YS,new DateTime(2015,3,12)),
                Assignment.Create(Role.V, JT,new DateTime(2015,3,12)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,12)),
                 //March 13th
                Assignment.Create(Role.Num03, SA,new DateTime(2015,3,13)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,3,13)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,13)),
                Assignment.Create(Role.Num06, RoB,new DateTime(2015,3,13)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,3,13)),
                Assignment.Create(Role.Num08, CC,new DateTime(2015,3,13)),
                Assignment.Create(Role.V, CE,new DateTime(2015,3,13)),
                Assignment.Create(Role.Num07, RE,new DateTime(2015,3,13)),
                Assignment.Create(Role.Num04, NF,new DateTime(2015,3,13)),
                Assignment.Create(Role.PD, CG,new DateTime(2015,3,13)),
                Assignment.Create(Role.PC, JH,new DateTime(2015,3,13)),
                Assignment.Create(Role.Num05, CH,new DateTime(2015,3,13)),
                Assignment.Create(Role.PM, SM,new DateTime(2015,3,13)),
                Assignment.Create(Role.C2, EM,new DateTime(2015,3,13)),
                Assignment.Create(Role.V, LS,new DateTime(2015,3,13)),
                Assignment.Create(Role.Num01, YS,new DateTime(2015,3,13)),
                Assignment.Create(Role.V, JT,new DateTime(2015,3,13)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,13)),
                 //March 14th
                Assignment.Create(Role.Off, SA,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,3,14)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,3,14)),
                Assignment.Create(Role.SevenToThree, CC,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,3,14)),
                Assignment.Create(Role.PM, SM,new DateTime(2015,3,14)),
                Assignment.Create(Role.AM, EM,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,3,14)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,14)),
                 //March 15th
                Assignment.Create(Role.Off, SA,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,3,15)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,3,15)),
                Assignment.Create(Role.PM, SM,new DateTime(2015,3,15)),
                Assignment.Create(Role.AM, EM,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,3,15)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,15)),
                 //March 16th
                Assignment.Create(Role.V, SA,new DateTime(2015,3,16)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,16)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num04, RoB,new DateTime(2015,3,16)),
                Assignment.Create(Role.PC, RB,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num01, CC,new DateTime(2015,3,16)),
                Assignment.Create(Role.C2, CE,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num01, RE,new DateTime(2015,3,16)),
                Assignment.Create(Role.V, NF,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num05, CG,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num07, JH,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num09, CH,new DateTime(2015,3,16)),
                Assignment.Create(Role.PM, SM,new DateTime(2015,3,16)),
                Assignment.Create(Role.PC, EM,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num06, LS,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num03, YS,new DateTime(2015,3,16)),
                Assignment.Create(Role.Num08, JT,new DateTime(2015,3,16)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,16)),
                 //March 17th
                Assignment.Create(Role.V, SA,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,3,17)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,17)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num08, RB,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num010, CC,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num01, CE,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num07, RE,new DateTime(2015,3,17)),
                Assignment.Create(Role.V, NF,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num06, CG,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num03, JH,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num04, CH,new DateTime(2015,3,17)),
                Assignment.Create(Role.PM, SM,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num011, EM,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num09, LS,new DateTime(2015,3,17)),
                Assignment.Create(Role.C2, YS,new DateTime(2015,3,17)),
                Assignment.Create(Role.Num05, JT,new DateTime(2015,3,17)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,17)),
                 //March 18th
                Assignment.Create(Role.V, SA,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num04, StB,new DateTime(2015,3,18)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,3,18)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num010, CC,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num011, CE,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num05, RE,new DateTime(2015,3,18)),
                Assignment.Create(Role.V, NF,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num07, CG,new DateTime(2015,3,18)),
                Assignment.Create(Role.C2, JH,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num09, CH,new DateTime(2015,3,18)),
                Assignment.Create(Role.PM, SM,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num03, EM,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num06, LS,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num01, YS,new DateTime(2015,3,18)),
                Assignment.Create(Role.Num08, JT,new DateTime(2015,3,18)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,18)),
                 //March 19th
                Assignment.Create(Role.V, SA,new DateTime(2015,3,19)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,19)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num06, RoB,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num02, RB,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num08, CC,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num09, CE,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num04, RE,new DateTime(2015,3,19)),
                Assignment.Create(Role.V, NF,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num07, CG,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num01, JH,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num05, CH,new DateTime(2015,3,19)),
                Assignment.Create(Role.PM, SM,new DateTime(2015,3,19)),
                Assignment.Create(Role.C2, EM,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num03, LS,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num011, YS,new DateTime(2015,3,19)),
                Assignment.Create(Role.Num010, JT,new DateTime(2015,3,19)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,3,19)),
                 //March 20th
                Assignment.Create(Role.V, SA,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,3,20)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num04, RoB,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num05, RB,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num06, CC,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num03, CE,new DateTime(2015,3,20)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,3,20)),
                Assignment.Create(Role.V, NF,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num08, CG,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num010, JH,new DateTime(2015,3,20)),
                Assignment.Create(Role.PM, CH,new DateTime(2015,3,20)),
                Assignment.Create(Role.PC, SM,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num01, EM,new DateTime(2015,3,20)),
                Assignment.Create(Role.C2, LS,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num09, YS,new DateTime(2015,3,20)),
                Assignment.Create(Role.Num07, JT,new DateTime(2015,3,20)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,3,20)),
                 //March 21st
                Assignment.Create(Role.Off, SA,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,3,21)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,3,21)),
                Assignment.Create(Role.PM, CH,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,3,21)),
                Assignment.Create(Role.AM, LS,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,3,21)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,3,21)),
                Assignment.Create(Role.SevenToThree, CW,new DateTime(2015,3,21)),
                 //March 22nd
                Assignment.Create(Role.Off, SA,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,3,22)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,3,22)),
                Assignment.Create(Role.PM, CH,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,3,22)),
                Assignment.Create(Role.AM, LS,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,3,22)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,22)),
                 //March 23rd
                Assignment.Create(Role.Num03, SA,new DateTime(2015,3,23)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,23)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num01, RoB,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num08, CC,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num05, CE,new DateTime(2015,3,23)),
                Assignment.Create(Role.PC, RE,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num010, NF,new DateTime(2015,3,23)),
                Assignment.Create(Role.C2, CG,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num09, JH,new DateTime(2015,3,23)),
                Assignment.Create(Role.PM, CH,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num06, SM,new DateTime(2015,3,23)),
                Assignment.Create(Role.V, EM,new DateTime(2015,3,23)),
                Assignment.Create(Role.PC, LS,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num02, YS,new DateTime(2015,3,23)),
                Assignment.Create(Role.Num07, JT,new DateTime(2015,3,23)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,3,23)),
                 //March 24th
                Assignment.Create(Role.Num010, SA,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,3,24)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num08, RoB,new DateTime(2015,3,24)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num03, CC,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num06, CE,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num09, RE,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num05, NF,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num01, CG,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num04, JH,new DateTime(2015,3,24)),
                Assignment.Create(Role.PM, CH,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num07, SM,new DateTime(2015,3,24)),
                Assignment.Create(Role.V, EM,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num013, LS,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num012, YS,new DateTime(2015,3,24)),
                Assignment.Create(Role.Num011, JT,new DateTime(2015,3,24)),
                Assignment.Create(Role.C2, CW,new DateTime(2015,3,24)),
                 //March 25th
                Assignment.Create(Role.Num01, SA,new DateTime(2015,3,25)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,25)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num05, RoB,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num02, RB,new DateTime(2015,3,25)),
                Assignment.Create(Role.C2, CC,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num07, CE,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num04, RE,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num08, NF,new DateTime(2015,3,25)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num011, JH,new DateTime(2015,3,25)),
                Assignment.Create(Role.PM, CH,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num06, SM,new DateTime(2015,3,25)),
                Assignment.Create(Role.V, EM,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num03, LS,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num010, YS,new DateTime(2015,3,25)),
                Assignment.Create(Role.Num09, JT,new DateTime(2015,3,25)),
                Assignment.Create(Role.SevenToOne, CW,new DateTime(2015,3,25)),
                //March 26th
                Assignment.Create(Role.Num012, SA,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,3,26)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num04, RoB,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num06, RB,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num01, CC,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num05, CE,new DateTime(2015,3,26)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num010, NF,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num011, CG,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num09, JH,new DateTime(2015,3,26)),
                Assignment.Create(Role.PM, CH,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num07, SM,new DateTime(2015,3,26)),
                Assignment.Create(Role.V, EM,new DateTime(2015,3,26)),
                Assignment.Create(Role.C2, LS,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num08, YS,new DateTime(2015,3,26)),
                Assignment.Create(Role.Num03, JT,new DateTime(2015,3,26)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,26)),
                 //March 27th
                Assignment.Create(Role.Num09, SA,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num06, StB,new DateTime(2015,3,27)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,27)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num011, CC,new DateTime(2015,3,27)),
                Assignment.Create(Role.PM, CE,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num03, NF,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num08, CG,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num07, JH,new DateTime(2015,3,27)),
                Assignment.Create(Role.PC, CH,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num05, SM,new DateTime(2015,3,27)),
                Assignment.Create(Role.V, EM,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num01, LS,new DateTime(2015,3,27)),
                Assignment.Create(Role.Num010, YS,new DateTime(2015,3,27)),
                Assignment.Create(Role.C2, JT,new DateTime(2015,3,27)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,27)),
                 //March 28th
                Assignment.Create(Role.Off, SA,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,28)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,28)),
                Assignment.Create(Role.PM, CE,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,28)),
                Assignment.Create(Role.SevenToThree, CG,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,3,28)),
                Assignment.Create(Role.AM, JT,new DateTime(2015,3,28)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,28)),
                 //March 29th
                Assignment.Create(Role.Off, SA,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,29)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,3,29)),
                Assignment.Create(Role.PM, CE,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,3,29)),
                Assignment.Create(Role.AM, JT,new DateTime(2015,3,29)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,3,29)),
                 //March 30th
                Assignment.Create(Role.C2, SA,new DateTime(2015,3,30)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,3,30)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,3,30)),
                Assignment.Create(Role.PC, RoB,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num05, CC,new DateTime(2015,3,30)),
                Assignment.Create(Role.PM, CE,new DateTime(2015,3,30)),
                Assignment.Create(Role.V, RE,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num07, NF,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num01, CG,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num06, JH,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num08, CH,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num02, SM,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num010, EM,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num09, LS,new DateTime(2015,3,30)),
                Assignment.Create(Role.Num03, YS,new DateTime(2015,3,30)),
                Assignment.Create(Role.PC, JT,new DateTime(2015,3,30)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,3,30)),
                 //March 31st
                Assignment.Create(Role.Num01, SA,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,3,31)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num07, RoB,new DateTime(2015,3,31)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num06, CC,new DateTime(2015,3,31)),
                Assignment.Create(Role.PM, CE,new DateTime(2015,3,31)),
                Assignment.Create(Role.V, RE,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num05, NF,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num012, CG,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num09, JH,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num08, CH,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num011, SM,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num03, EM,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num04, LS,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num010, YS,new DateTime(2015,3,31)),
                Assignment.Create(Role.Num013, JT,new DateTime(2015,3,31)),
                Assignment.Create(Role.C2, CW,new DateTime(2015,3,31)),
            };

        }

    }
}
