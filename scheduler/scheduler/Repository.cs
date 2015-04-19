using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAON.Utility.Objects.Methods;
using utilities.data;

namespace scheduler.data
{
    public class Repository : IRepository
    {
        private IDbConnection Connection { get; set; }

        private Repository() { }

        public static Repository Create(IDbConnection cn)
        {
            var repo = new Repository() { Connection = cn };
            //repo.GetEmployees();
            //repo.GetAssignments();
            repo.SeedDetailedSchedule();
            repo.SaveDetailedAssignments();
            repo.SaveCallProviders();
            return repo;
        }

        public static IRepository Create()
        {
            var connectionString =
                "Server=tcp:dzvmbj8x0g.database.windows.net,1433;Database=mercycrna;User ID=mercycrnadata@dzvmbj8x0g;Password=Coconut12;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            var cn = DataInteraction.CreateSqlConnection("tcp:dzvmbj8x0g.database.windows.net,1433", "mercycrna", "mercycrnadata", "Coconut12");
            return Create(cn);
        }

        public void Refresh()
        {
            GetEmployees();
            GetAssignments();
            GetDetailedAssignments();
            GetCallProviders();
        }

        public void Clear()
        {
            _assignments = null;
            _employees = null;
            _detailedAssignments = null;
            _callProviders = null;
        }

        private List<IAssignment> _assignments;
        public List<IAssignment> Assignments
        {
            get
            {
                if (_assignments == null) { GetAssignments(); }
                return _assignments;
            }
            set { _assignments = value; }
        }

        private List<IEmployee> _employees;
        public List<IEmployee> Employees
        {
            get
            {
                if (_employees == null) { GetEmployees(); }
                return _employees;
            }
            set { _employees = value; }
        }

        private List<IDetailedAssignment> _detailedAssignments;
        public List<IDetailedAssignment> DetailedAssignments
        {
            get
            {
                if (_detailedAssignments == null) { GetDetailedAssignments(); }
                return _detailedAssignments;
            }
            set { _detailedAssignments = value; }
        }

        private List<ICallProvider> _callProviders;
        public List<ICallProvider> CallProviders
        {
            get { if (_callProviders == null) { GetCallProviders(); } return _callProviders; }
            set { _callProviders = value; }
        }

        #region empty

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

        private void EmptyDetailedAssignmentsTable()
        {
            const string deleteCommand = "TRUNCATE TABLE DETAILEDASSIGNMENTS";
            DataInteraction.ExecuteNonQuery(deleteCommand, Connection);
        }

        private void EmptyCallProvidersTable()
        {
            const string deleteCommand = "TRUNCATE TABLE CALLPROVIDERS";
            DataInteraction.ExecuteNonQuery(deleteCommand, Connection);
        }

        #endregion

        #region insert

        private void InsertEmployee(List<SqlParameter> parameters)
        {
            const string insertEmployeeCommand = "INSERT INTO EMPLOYEES (First, Last, Initials, Start, ContactId) VALUES (@first,@last,@initials,@start,@contactId)";
            DataInteraction.ExecuteNonQuery(insertEmployeeCommand, parameters, Connection);
        }

        private void InsertContact(List<SqlParameter> parameters)
        {
            const string insertContactCommand = "INSERT INTO CONTACTS (phone, email, address) VALUES (@phone,@email,@address)";
            DataInteraction.ExecuteNonQuery(insertContactCommand, parameters, Connection);
        }

        private void InsertAssignment(List<SqlParameter> parameters)
        {
            const string insertAssignmentCommand = "INSERT INTO Assignments (Role, Date, EmployeeID) VALUES (@Role,@Date,@EmployeeID)";
            DataInteraction.ExecuteNonQuery(insertAssignmentCommand, parameters, Connection);
        }

        private void InsertDetailedAssignment(List<SqlParameter> parameters)
        {
            const string insertDetailedAssignmentCommand = "INSERT INTO DetailedAssignments (Location, Surgeon, Time, Provider, PeelOut, Hospital, Date) Values (@Location, @Surgeon, @Time, @Provider, @PeelOut, @Hospital, @Date)";
            DataInteraction.ExecuteNonQuery(insertDetailedAssignmentCommand, parameters, Connection);
        }

        private void InsertCallProvider(List<SqlParameter> parameters)
        {
            const string insertAssignmentCommand = "INSERT INTO CallProviders (Provider,Name,InTime,Notes,Hospital,Date) VALUES (@Provider,@Name,@InTime,@Notes,@Hospital,@Date)";
            DataInteraction.ExecuteNonQuery(insertAssignmentCommand, parameters, Connection);
        }

        #endregion

        #region save

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

        public void SaveAssignments()
        {
            EmptyAssignmentsTable();

            foreach (var assignment in Assignments)
            {
                var assignmentParameters = GetAssignmentParameters(assignment);
                InsertAssignment(assignmentParameters);
            }
        }

        public void SaveCallProviders()
        {
            EmptyCallProvidersTable();

            foreach (var callProvider in CallProviders)
            {
                var callProviderParameters = GetCallProviderParameters(callProvider);
                InsertCallProvider(callProviderParameters);
            }
        }

        public void SaveDetailedAssignments()
        {
            EmptyDetailedAssignmentsTable();

            foreach (var detailedAssignment in DetailedAssignments)
            {
                var detailedAssignmentParameters = GetDetailedAssignmentParameters(detailedAssignment);
                InsertDetailedAssignment(detailedAssignmentParameters);
            }
        }

        #endregion

        #region get

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

        private int GetContactIdForCurrentEmployee(IEmployee employee)
        {
            var phone = DataInteraction.CreateSqlParameter("@phone", employee.Contact.Phone);
            var contactDt = DataInteraction.CreateDataTable("SELECT * FROM CONTACTS WHERE Phone=@phone", new List<SqlParameter> { phone }, Connection);
            var contactId = (int)contactDt.Rows[0]["Id"];
            return contactId;
        }

        private List<SqlParameter> GetAssignmentParameters(IAssignment assignment)
        {
            if (assignment.Employee.Id == 0)
            {
                assignment.Employee.Id = GetEmployeeIdFromInitials(assignment.Employee.Initials);
                //throw new Exception("Assignment not correctly associated to employee.");
                if (assignment.Employee.Id == 0)
                {
                    throw new Exception("Assignment STILL not correctly associated to employee.");
                }
            }



            var parameters = new List<SqlParameter>
                {
                    DataInteraction.CreateSqlParameter("@Role", assignment.Role),
                    DataInteraction.CreateSqlParameter("@Date", assignment.Date),
                    DataInteraction.CreateSqlParameter("@EmployeeID", assignment.Employee.Id)
                };
            return parameters;
        }

        private List<SqlParameter> GetDetailedAssignmentParameters(IDetailedAssignment detailedAssignment)
        {
            if (detailedAssignment.Provider == "")
            { }

            var parameters = new List<SqlParameter>
            {
                DataInteraction.CreateSqlParameter("@Location", detailedAssignment.Location),
                DataInteraction.CreateSqlParameter("@Surgeon", detailedAssignment.Surgeon),
                DataInteraction.CreateSqlParameter("@Time", detailedAssignment.Time),
                DataInteraction.CreateSqlParameter("@Provider", detailedAssignment.Provider),
                DataInteraction.CreateSqlParameter("@PeelOut", detailedAssignment.PeelOut),
                DataInteraction.CreateSqlParameter("@Hospital", detailedAssignment.Hospital),
                DataInteraction.CreateSqlParameter("@Date", detailedAssignment.Date)
            };
            return parameters;
        }

        private List<SqlParameter> GetCallProviderParameters(ICallProvider callProvider)
        {
            if (callProvider.Provider == "")
            { }

            var parameters = new List<SqlParameter>
            {
                DataInteraction.CreateSqlParameter("@Provider", callProvider.Provider),
                DataInteraction.CreateSqlParameter("@Name", callProvider.Name),
                DataInteraction.CreateSqlParameter("@Notes", callProvider.Notes),
                DataInteraction.CreateSqlParameter("@InTime", callProvider.InTime),
                DataInteraction.CreateSqlParameter("@Hospital", callProvider.Hospital),
                DataInteraction.CreateSqlParameter("@Date", callProvider.Date)

            };

            if (true) { parameters.Add(DataInteraction.CreateSqlParameter("@ID", callProvider.Id)); }

            return parameters;
        }

        private int GetEmployeeIdFromInitials(string initials)
        {
            if (Employees == null) { return 0; }
            var id = (from employee in Employees where employee.Initials == initials select employee.Id).FirstOrDefault();
            return id;
        }

        private void GetDetailedAssignments()
        {
            var dt = DataInteraction.CreateDataTable("SELECT * FROM DetailedAssignments", Connection);
            _detailedAssignments = (from DataRow dr in dt.Rows select CreateDetailedAssignment(dr)).ToList();
        }

        private void GetCallProviders()
        {
            var dt = DataInteraction.CreateDataTable("SELECT * FROM CallProviders", Connection);
            _callProviders = (from DataRow dr in dt.Rows select CreateCallProvider(dr)).ToList();
        }

        private void GetEmployees()
        {
            var dt = DataInteraction.CreateDataTable("SELECT * FROM Employees", Connection);
            _employees = (from DataRow dr in dt.Rows select CreateEmployee(dr)).ToList();
        }

        private IContact GetContact(int id)
        {
            var parameter = DataInteraction.CreateSqlParameter("@ID", id);
            var dt = DataInteraction.CreateDataTable("SELECT * FROM Contacts WHERE ID=@ID", new List<SqlParameter> { parameter }, Connection);

            var contacts = (from DataRow dr in dt.Rows select CreateContact(dr)).ToList();

            return contacts.FirstOrDefault();
        }

        private void GetAssignments()
        {
            var dt = DataInteraction.CreateDataTable("SELECT * FROM Assignments", Connection);
            _assignments = (from DataRow dr in dt.Rows select CreateAssignment(dr)).ToList();
        }

        public IEmployee GetEmployeeById(int employeeId)
        {
            return (from emp in Employees where employeeId == emp.Id select emp).FirstOrDefault();
        }

        public IEmployee GetEmployeeByInitials(string employeeInitials)
        {
            return (from emp in Employees where employeeInitials == emp.Initials select emp).FirstOrDefault();
        }

        public IAssignment GetAssignment(int id)
        {
            return (from assignment in Assignments where assignment.Id == id select assignment).FirstOrDefault();
        }

        private IEmployee GetEmployee(int id)
        {
            var parameter = DataInteraction.CreateSqlParameter("@ID", id);
            var dt = DataInteraction.CreateDataTable("SELECT * FROM Employees WHERE ID=@ID", new List<SqlParameter> { parameter }, Connection);

            var emps = (from DataRow dr in dt.Rows select CreateEmployee(dr)).ToList();

            return emps.FirstOrDefault();
        }

        public IDetailedAssignment GetDetailedAssignments(int id)
        {
            return (from detailedAssignment in DetailedAssignments where detailedAssignment.Id == id select detailedAssignment).FirstOrDefault();
        }

        public List<IDetailedAssignment> GetDetailedAssignments(DateTime date)
        {
            return
                (from detailedAssignment in DetailedAssignments
                 where detailedAssignment.Date.Date == date.Date
                 select detailedAssignment).ToList();
        }

        public List<ICallProvider> GetCallProviders(DateTime date)
        {
            return
                (from callProvider in CallProviders
                 where callProvider.Date.Date == date.Date
                 select callProvider).ToList();
        }

        public IDetailedSchedule GetDetailedSchedule(DateTime date)
        {
            var detailedSchedule = DetailedSchedule.Create();
            detailedSchedule.Date = date;
            detailedSchedule.DetailedAssignments = GetDetailedAssignments(date);
            detailedSchedule.CallProviders = GetCallProviders(date);

            return detailedSchedule;
        }

        #endregion

        #region create

        private IDetailedAssignment CreateDetailedAssignment(DataRow dr)
        {
            var detailedAssignment = DetailedAssignment.Create();
            detailedAssignment.Id = Parsers.Parser(dr["Id"], 0);
            detailedAssignment.Location = Parsers.Parser(dr["Location"], "");
            detailedAssignment.Surgeon = Parsers.Parser(dr["Surgeon"], "");
            detailedAssignment.Time = Parsers.Parser(dr["Time"], "");
            detailedAssignment.Provider = Parsers.Parser(dr["Provider"], "");
            detailedAssignment.PeelOut = Parsers.Parser(dr["PeelOut"], "");

            return detailedAssignment;
        }

        private ICallProvider CreateCallProvider(DataRow dr)
        {
            var callProvider = CallProvider.Create();
            callProvider.Id = Parsers.Parser(dr["Id"], 0);
            callProvider.Provider = Parsers.Parser(dr["Provider"], "");
            callProvider.Name = Parsers.Parser(dr["Name"], "");
            callProvider.InTime = Parsers.Parser(dr["InTime"], "");
            callProvider.Notes = Parsers.Parser(dr["Notes"], "");
            callProvider.Hospital = (Hospital)Enum.Parse(typeof(Hospital), Parsers.Parser(dr["Hospital"], "")); //(Role)Enum.Parse(typeof(Role), Parsers.Parser(dr["Role"], ""));
            callProvider.Date = Parsers.Parser(dr["Date"], DateTime.MinValue);
            return callProvider;
        }

        private IEmployee CreateEmployee(DataRow dr)
        {
            var employee = Employee.CreateEmpty();
            employee.Id = Parsers.Parser(dr["Id"], 0);
            employee.First = Parsers.Parser(dr["First"], "");
            employee.Last = Parsers.Parser(dr["Last"], "");
            employee.Initials = Parsers.Parser(dr["Initials"], "");
            employee.Start = Parsers.Parser(dr["Start"], DateTime.MinValue);

            employee.Contact = GetContact(Parsers.Parser(dr["ContactID"], 0));

            return employee;
        }

        private IContact CreateContact(DataRow dr)
        {
            var contact = Contact.CreateEmpty();
            if (dr == null) { return null; }
            contact.Id = Parsers.Parser(dr["ID"], 0);
            contact.Phone = Parsers.Parser(dr["Phone"], "");
            contact.Email = Parsers.Parser(dr["Email"], "");
            contact.Address = Parsers.Parser(dr["Address"], "");
            return contact;
        }

        private IAssignment CreateAssignment(DataRow dr)
        {
            var assignment = Assignment.CreateEmpty();
            assignment.Id = Parsers.Parser(dr["ID"], 0);
            assignment.Role = (Role)Enum.Parse(typeof(Role), Parsers.Parser(dr["Role"], ""));
            assignment.Date = Parsers.Parser(dr["Date"], DateTime.MinValue);
            assignment.Employee = (from emp in Employees where emp.Id == Parsers.Parser(dr["EmployeeID"], 0) select emp).FirstOrDefault(); //GetEmployee(Parsers.Parser(dr["EmployeeID"], 0));

            return assignment;
        }

        #endregion

        #region seed

        /// <summary>
        /// Dangerous!  Resets database!
        /// </summary>
        private void Seed()
        {
            SeedEmployees();
            SeedAssignments();
        }

        private void SeedEmployees()
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
        }

        private void SeedAssignments()
        {
            Assignments = new List<IAssignment>();

            SeedMarch();
            SeedApril();
            SeedMay();
        }

        private void SeedMarch()
        {
            var SA = Employee.Create("Shane", "Adams", "SA", DateTime.Now, "417-499-3116", "", "");
            var StB = Employee.Create("Steve", "Boeger", "StB", DateTime.Now, "417-624-3192", "", "");
            var SB = Employee.Create("Susie", "Boeger", "SB", DateTime.Now, "417-624-3192", "", "");
            var RoB = Employee.Create("Robin", "Boyd", "RoB", DateTime.Now, "417-624-2949", "", "");
            var RB = Employee.Create("Rick", "Brown", "RB", DateTime.Now, "417-496-3793", "", "");
            var CC = Employee.Create("Chris", "Crabtree", "CC", DateTime.Now, "417-830-6644", "", "");
            var CE = Employee.Create("Cary", "Edwards", "CE", DateTime.Now, "417-619-5745", "", "");
            var RE = Employee.Create("Ray", "Eisenmann", "RE", DateTime.Now, "417-425-7644", "", "");
            var NF = Employee.Create("Nancy", "Fiscus", "NF", DateTime.Now, "417-781-4152", "", "");
            var CG = Employee.Create("Chris", "Garde", "CG", DateTime.Now, "417-782-7869", "", "");
            var JH = Employee.Create("John", "Howard", "JH", DateTime.Now, "417-317-0777", "", "");
            var CH = Employee.Create("Cole", "Hughes", "CH", DateTime.Now, "417-728-4793", "", "");
            var SM = Employee.Create("Shelly", "Mabe", "SM", DateTime.Now, "417-766-6916", "", "");
            var EM = Employee.Create("Ed", "Messer", "EM", DateTime.Now, "417-381-6317", "", "");
            var LS = Employee.Create("Leanna", "Swager", "LS", DateTime.Now, "417-358-4426", "", "");
            var YS = Employee.Create("Yvonne", "Stanke", "YS", DateTime.Now, "417-862-9887", "", "");
            var JT = Employee.Create("Jim", "Tyler", "JT", DateTime.Now, "417-206-8005", "", "");
            var CW = Employee.Create("Curt", "Williams", "CW", DateTime.Now, "417-833-9007", "", "");

            var assignments = new List<IAssignment>
            {
                //March 1st
                Assignment.Create(Role.PM, SA, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, StB, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.CV, RoB, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, RB, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.AM, CE, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, RE, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, JH, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, CH, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, SM, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, EM, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, LS, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, YS, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, JT, new DateTime(2015, 3, 1)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 1)),
                //March 2nd
                Assignment.Create(Role.PM, SA, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num04, StB, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.PC, RoB, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.V, RB, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num08, CC, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.PC, CE, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.CV, RE, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.C2, NF, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.SevenToTen, CG, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num07, JH, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num01, CH, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num09, SM, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num02, EM, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num05, LS, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num06, YS, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.Num03, JT, new DateTime(2015, 3, 2)),
                Assignment.Create(Role.SevenToFive, CW, new DateTime(2015, 3, 2)),
                //March 3rd
                Assignment.Create(Role.PM, SA, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num07, RoB, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.V, RB, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num04, CC, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num013, CE, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num02, RE, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num01, NF, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num09, CG, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num05, JH, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num012, CH, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num03, SM, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num010, EM, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num08, LS, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num06, YS, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.Num011, JT, new DateTime(2015, 3, 3)),
                Assignment.Create(Role.C2, CW, new DateTime(2015, 3, 3)),
                //March 4th
                Assignment.Create(Role.PM, SA, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num02, StB, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num04, RoB, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.V, RB, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num09, CC, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num03, CE, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.CV, RE, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num08, CG, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num01, JH, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num07, CH, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.C2, SM, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num05, EM, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num06, LS, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num010, YS, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.Num011, JT, new DateTime(2015, 3, 4)),
                Assignment.Create(Role.SevenToOne, CW, new DateTime(2015, 3, 4)),
                //March 5th
                Assignment.Create(Role.PM, SA, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num04, StB, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.CV, RoB, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.V, RB, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num07, CC, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.C2, CE, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num02, RE, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num012, NF, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num06, CG, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num011, JH, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num09, CH, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num01, SM, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num010, EM, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num08, LS, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num03, YS, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Num05, JT, new DateTime(2015, 3, 5)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 5)),
                //March 6th
                Assignment.Create(Role.PC, SA, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num02, RoB, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.V, RB, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num04, CC, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num01, CE, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num07, RE, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num08, NF, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num05, CG, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.PM, JH, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num03, CH, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num011, SM, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num09, EM, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num06, LS, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.C2, YS, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Num010, JT, new DateTime(2015, 3, 6)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 6)),
                //March 7th
                Assignment.Create(Role.Off, SA, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, RoB, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, RB, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, CE, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, RE, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.SevenToThree, NF, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.PM, JH, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, CH, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, SM, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, EM, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, LS, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.AM, YS, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, JT, new DateTime(2015, 3, 7)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 7)),
                //March 8th
                Assignment.Create(Role.Off, SA, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, RoB, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, RB, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, CE, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, RE, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.PM, JH, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, CH, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, SM, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, EM, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, LS, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.AM, YS, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, JT, new DateTime(2015, 3, 8)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 8)),
                //March 9th
                Assignment.Create(Role.Num07, SA, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.PC, StB, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.CV, RoB, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.Num01, RB, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.C2, CC, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.V, CE, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.Num04, RE, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.Num03, NF, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.Num05, CG, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.PM, JH, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.Num06, CH, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.Num02, SM, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.Num08, EM, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.V, LS, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.PC, YS, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.V, JT, new DateTime(2015, 3, 9)),
                Assignment.Create(Role.V, CW, new DateTime(2015, 3, 9)),
                //March 10th
                Assignment.Create(Role.Num05, SA, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num07, StB, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num02, RoB, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num06, RB, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num01, CC, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.V, CE, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.CV, RE, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.C2, NF, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num03, CG, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.PM, JH, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num08, CH, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num09, SM, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num04, EM, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.V, LS, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.Num010, YS, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.V, JT, new DateTime(2015, 3, 10)),
                Assignment.Create(Role.V, CW, new DateTime(2015, 3, 10)),
                //March 11th
                Assignment.Create(Role.Num05, SA, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Num04, StB, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.CV, RoB, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Num07, RB, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.V, CE, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Num02, RE, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Num01, NF, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.C2, CG, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.PM, JH, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Num06, CH, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Num08, SM, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Num09, EM, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.V, LS, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.Num03, YS, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.V, JT, new DateTime(2015, 3, 11)),
                Assignment.Create(Role.V, CW, new DateTime(2015, 3, 11)),
                //March 12th
                Assignment.Create(Role.Num07, SA, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num02, RoB, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num05, RB, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num09, CC, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.V, CE, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num06, RE, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num010, NF, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num01, CG, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.PM, JH, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num08, CH, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num04, SM, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Num03, EM, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.V, LS, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.C2, YS, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.V, JT, new DateTime(2015, 3, 12)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 12)),
                //March 13th
                Assignment.Create(Role.Num03, SA, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.Num02, StB, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.Num06, RoB, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.CV, RB, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.Num08, CC, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.V, CE, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.Num07, RE, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.Num04, NF, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.PD, CG, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.PC, JH, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.Num05, CH, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.PM, SM, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.C2, EM, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.V, LS, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.Num01, YS, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.V, JT, new DateTime(2015, 3, 13)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 13)),
                //March 14th
                Assignment.Create(Role.Off, SA, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, StB, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, RoB, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.CV, RB, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.SevenToThree, CC, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, CE, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, RE, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, JH, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, CH, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.PM, SM, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.AM, EM, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, LS, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, YS, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, JT, new DateTime(2015, 3, 14)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 14)),
                //March 15th
                Assignment.Create(Role.Off, SA, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, StB, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, RoB, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.CV, RB, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, CE, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, RE, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, JH, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, CH, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.PM, SM, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.AM, EM, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, LS, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, YS, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, JT, new DateTime(2015, 3, 15)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 15)),
                //March 16th
                Assignment.Create(Role.V, SA, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num04, RoB, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.PC, RB, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num01, CC, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.C2, CE, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num01, RE, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.V, NF, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num05, CG, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num07, JH, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num09, CH, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.PM, SM, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.PC, EM, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num06, LS, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num03, YS, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Num08, JT, new DateTime(2015, 3, 16)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 16)),
                //March 17th
                Assignment.Create(Role.V, SA, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num02, StB, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.CV, RoB, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num08, RB, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num010, CC, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num01, CE, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num07, RE, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.V, NF, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num06, CG, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num03, JH, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num04, CH, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.PM, SM, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num011, EM, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num09, LS, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.C2, YS, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Num05, JT, new DateTime(2015, 3, 17)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 17)),
                //March 18th
                Assignment.Create(Role.V, SA, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num04, StB, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num02, RoB, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.CV, RB, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num010, CC, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num011, CE, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num05, RE, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.V, NF, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num07, CG, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.C2, JH, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num09, CH, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.PM, SM, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num03, EM, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num06, LS, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num01, YS, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Num08, JT, new DateTime(2015, 3, 18)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 18)),
                //March 19th
                Assignment.Create(Role.V, SA, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num06, RoB, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num02, RB, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num08, CC, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num09, CE, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num04, RE, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.V, NF, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num07, CG, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num01, JH, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num05, CH, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.PM, SM, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.C2, EM, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num03, LS, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num011, YS, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.Num010, JT, new DateTime(2015, 3, 19)),
                Assignment.Create(Role.SevenToFive, CW, new DateTime(2015, 3, 19)),
                //March 20th
                Assignment.Create(Role.V, SA, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num02, StB, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num04, RoB, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num05, RB, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num06, CC, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num03, CE, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.CV, RE, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.V, NF, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num08, CG, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num010, JH, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.PM, CH, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.PC, SM, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num01, EM, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.C2, LS, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num09, YS, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.Num07, JT, new DateTime(2015, 3, 20)),
                Assignment.Create(Role.SevenToFive, CW, new DateTime(2015, 3, 20)),
                //March 21st
                Assignment.Create(Role.Off, SA, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, StB, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, RoB, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, RB, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, CE, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.CV, RE, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, JH, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.PM, CH, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, SM, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, EM, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.AM, LS, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, YS, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.Off, JT, new DateTime(2015, 3, 21)),
                Assignment.Create(Role.SevenToThree, CW, new DateTime(2015, 3, 21)),
                //March 22nd
                Assignment.Create(Role.Off, SA, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, StB, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, RoB, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, RB, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, CE, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.CV, RE, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, JH, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.PM, CH, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, SM, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, EM, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.AM, LS, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, YS, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, JT, new DateTime(2015, 3, 22)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 22)),
                //March 23rd
                Assignment.Create(Role.Num03, SA, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num01, RoB, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num04, RB, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num08, CC, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num05, CE, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.PC, RE, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num010, NF, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.C2, CG, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num09, JH, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.PM, CH, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num06, SM, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.V, EM, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.PC, LS, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num02, YS, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.Num07, JT, new DateTime(2015, 3, 23)),
                Assignment.Create(Role.SevenToFive, CW, new DateTime(2015, 3, 23)),
                //March 24th
                Assignment.Create(Role.Num010, SA, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num02, StB, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num08, RoB, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.CV, RB, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num03, CC, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num06, CE, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num09, RE, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num05, NF, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num01, CG, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num04, JH, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.PM, CH, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num07, SM, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.V, EM, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num013, LS, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num012, YS, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.Num011, JT, new DateTime(2015, 3, 24)),
                Assignment.Create(Role.C2, CW, new DateTime(2015, 3, 24)),
                //March 25th
                Assignment.Create(Role.Num01, SA, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num05, RoB, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num02, RB, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.C2, CC, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num07, CE, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num04, RE, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num08, NF, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num011, JH, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.PM, CH, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num06, SM, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.V, EM, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num03, LS, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num010, YS, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.Num09, JT, new DateTime(2015, 3, 25)),
                Assignment.Create(Role.SevenToOne, CW, new DateTime(2015, 3, 25)),
                //March 26th
                Assignment.Create(Role.Num012, SA, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num02, StB, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num04, RoB, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num06, RB, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num01, CC, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num05, CE, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.CV, RE, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num010, NF, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num011, CG, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num09, JH, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.PM, CH, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num07, SM, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.V, EM, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.C2, LS, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num08, YS, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Num03, JT, new DateTime(2015, 3, 26)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 26)),
                //March 27th
                Assignment.Create(Role.Num09, SA, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num06, StB, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.CV, RoB, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num04, RB, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num011, CC, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.PM, CE, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num02, RE, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num03, NF, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num08, CG, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num07, JH, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.PC, CH, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num05, SM, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.V, EM, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num01, LS, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Num010, YS, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.C2, JT, new DateTime(2015, 3, 27)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 27)),
                //March 28th
                Assignment.Create(Role.Off, SA, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, StB, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.CV, RoB, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, RB, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.PM, CE, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, RE, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.SevenToThree, CG, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, JH, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, CH, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, SM, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, EM, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, LS, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, YS, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.AM, JT, new DateTime(2015, 3, 28)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 28)),
                //March 29th
                Assignment.Create(Role.Off, SA, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, StB, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.CV, RoB, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, RB, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, CC, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.PM, CE, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, RE, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, NF, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, CG, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, JH, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, CH, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, SM, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, EM, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, LS, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, YS, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.AM, JT, new DateTime(2015, 3, 29)),
                Assignment.Create(Role.Off, CW, new DateTime(2015, 3, 29)),
                //March 30th
                Assignment.Create(Role.C2, SA, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.CV, StB, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.SevenToFive, SB, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.PC, RoB, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num04, RB, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num05, CC, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.PM, CE, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.V, RE, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num07, NF, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num01, CG, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num06, JH, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num08, CH, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num02, SM, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num010, EM, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num09, LS, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.Num03, YS, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.PC, JT, new DateTime(2015, 3, 30)),
                Assignment.Create(Role.SevenToFive, CW, new DateTime(2015, 3, 30)),
                //March 31st
                Assignment.Create(Role.Num01, SA, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num02, StB, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Off, SB, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num07, RoB, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.CV, RB, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num06, CC, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.PM, CE, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.V, RE, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num05, NF, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num012, CG, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num09, JH, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num08, CH, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num011, SM, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num03, EM, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num04, LS, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num010, YS, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.Num013, JT, new DateTime(2015, 3, 31)),
                Assignment.Create(Role.C2, CW, new DateTime(2015, 3, 31)),
            };

            Assignments.AddRange(assignments);
        }

        private void SeedApril()
        {
            var SA = Employee.Create("Shane", "Adams", "SA", DateTime.Now, "417-499-3116", "", "");
            var StB = Employee.Create("Steve", "Boeger", "StB", DateTime.Now, "417-624-3192", "", "");
            var SB = Employee.Create("Susie", "Boeger", "SB", DateTime.Now, "417-624-3192", "", "");
            var RoB = Employee.Create("Robin", "Boyd", "RoB", DateTime.Now, "417-624-2949", "", "");
            var RB = Employee.Create("Rick", "Brown", "RB", DateTime.Now, "417-496-3793", "", "");
            var CC = Employee.Create("Chris", "Crabtree", "CC", DateTime.Now, "417-830-6644", "", "");
            var CE = Employee.Create("Cary", "Edwards", "CE", DateTime.Now, "417-619-5745", "", "");
            var RE = Employee.Create("Ray", "Eisenmann", "RE", DateTime.Now, "417-425-7644", "", "");
            var NF = Employee.Create("Nancy", "Fiscus", "NF", DateTime.Now, "417-781-4152", "", "");
            var CG = Employee.Create("Chris", "Garde", "CG", DateTime.Now, "417-782-7869", "", "");
            var JH = Employee.Create("John", "Howard", "JH", DateTime.Now, "417-317-0777", "", "");
            var CH = Employee.Create("Cole", "Hughes", "CH", DateTime.Now, "417-728-4793", "", "");
            var SM = Employee.Create("Shelly", "Mabe", "SM", DateTime.Now, "417-766-6916", "", "");
            var EM = Employee.Create("Ed", "Messer", "EM", DateTime.Now, "417-381-6317", "", "");
            var LS = Employee.Create("Leanna", "Swager", "LS", DateTime.Now, "417-358-4426", "", "");
            var YS = Employee.Create("Yvonne", "Stanke", "YS", DateTime.Now, "417-862-9887", "", "");
            var JT = Employee.Create("Jim", "Tyler", "JT", DateTime.Now, "417-206-8005", "", "");
            var CW = Employee.Create("Curt", "Williams", "CW", DateTime.Now, "417-833-9007", "", "");

            var assignments = new List<IAssignment>
            {
                //April 1st
                Assignment.Create(Role.Off, SA,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num06, StB,new DateTime(2015,4,1)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,4,1)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num011, CC,new DateTime(2015,4,1)),
                Assignment.Create(Role.PM, CE,new DateTime(2015,4,1)),
                Assignment.Create(Role.V, RE,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num05, NF,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num08, CG,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num04, JH,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num01, CH,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num010, SM,new DateTime(2015,4,1)),
                Assignment.Create(Role.C2, EM,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num07, LS,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num09, YS,new DateTime(2015,4,1)),
                Assignment.Create(Role.Num03, JT,new DateTime(2015,4,1)),
                Assignment.Create(Role.SevenToOne, CW,new DateTime(2015,4,1)),
                 //April 2nd
                Assignment.Create(Role.Num010, SA,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num04, StB,new DateTime(2015,4,2)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,2)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num02, RB,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num04, CC,new DateTime(2015,4,2)),
                Assignment.Create(Role.PM, CE,new DateTime(2015,4,2)),
                Assignment.Create(Role.V, RE,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num09, NF,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num07, CG,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num03, JH,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num011, CH,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num05, SM,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num01, EM,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num05, LS,new DateTime(2015,4,2)),
                Assignment.Create(Role.PM, YS,new DateTime(2015,4,2)),
                Assignment.Create(Role.Num01, JT,new DateTime(2015,4,2)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,2)),
                 //April 3rd
                Assignment.Create(Role.Num04, SA,new DateTime(2015,4,3)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,4,3)),
                Assignment.Create(Role.V, SB,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num06, RB,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num010, CC,new DateTime(2015,4,3)),
                Assignment.Create(Role.PC, CE,new DateTime(2015,4,3)),
                Assignment.Create(Role.V, RE,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num08, NF,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num07, CG,new DateTime(2015,4,3)),
                Assignment.Create(Role.C2, JH,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num03, CH,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num09, SM,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num011, EM,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num05, LS,new DateTime(2015,4,3)),
                Assignment.Create(Role.PM, YS,new DateTime(2015,4,3)),
                Assignment.Create(Role.Num01, JT,new DateTime(2015,4,3)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,3)),
                 //April 4th
                Assignment.Create(Role.SevenToThree, SA,new DateTime(2015,4,4)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,4,4)),
                Assignment.Create(Role.AM, JH,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,4,4)),
                Assignment.Create(Role.PM, YS,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,4,4)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,4)),
                 //April 5th
                Assignment.Create(Role.Off, SA,new DateTime(2015,4,5)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,4,5)),
                Assignment.Create(Role.AM, JH,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,4,5)),
                Assignment.Create(Role.PM, YS,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,4,5)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,5)),
                 //April 6th
                Assignment.Create(Role.Num03, SA,new DateTime(2015,4,6)),
                Assignment.Create(Role.PC, StB,new DateTime(2015,4,6)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num04, RoB,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num05, RB,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num07, CC,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num010, CE,new DateTime(2015,4,6)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num08, NF,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num01, CG,new DateTime(2015,4,6)),
                Assignment.Create(Role.PC, JH,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num09, CH,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num02, SM,new DateTime(2015,4,6)),
                Assignment.Create(Role.C2, EM,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num06, LS,new DateTime(2015,4,6)),
                Assignment.Create(Role.PM, YS,new DateTime(2015,4,6)),
                Assignment.Create(Role.Num011, JT,new DateTime(2015,4,6)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,4,6)),
                 //April 7th
                Assignment.Create(Role.Num09, SA,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num08, StB,new DateTime(2015,4,7)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,7)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num03, CC,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num011, CE,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num07, NF,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num013, CG,new DateTime(2015,4,7)),
                Assignment.Create(Role.PD, JH,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num05, CH,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num012, SM,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num01, EM,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num06, LS,new DateTime(2015,4,7)),
                Assignment.Create(Role.PM, YS,new DateTime(2015,4,7)),
                Assignment.Create(Role.Num010, JT,new DateTime(2015,4,7)),
                Assignment.Create(Role.C2, CW,new DateTime(2015,4,7)),
                 //April 8th
                Assignment.Create(Role.Num09, SA,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num04, StB,new DateTime(2015,4,8)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num05, RB,new DateTime(2015,4,8)),
                Assignment.Create(Role.C2, CC,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num01, CE,new DateTime(2015,4,8)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num06, NF,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num07, CG,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num03, JH,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num012, CH,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num010, SM,new DateTime(2015,4,8)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num08, LS,new DateTime(2015,4,8)),
                Assignment.Create(Role.PM, YS,new DateTime(2015,4,8)),
                Assignment.Create(Role.Num011, JT,new DateTime(2015,4,8)),
                Assignment.Create(Role.SevenToOne, CW,new DateTime(2015,4,8)),
                 //April 9th
                Assignment.Create(Role.Num09, SA,new DateTime(2015,4,9)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,4,9)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num07, RoB,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num01, CC,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num011, CE,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num03, NF,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num012, CG,new DateTime(2015,4,9)),
                Assignment.Create(Role.C2, JH,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num010, CH,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num08, SM,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num013, EM,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num06, LS,new DateTime(2015,4,9)),
                Assignment.Create(Role.PM, YS,new DateTime(2015,4,9)),
                Assignment.Create(Role.Num05, JT,new DateTime(2015,4,9)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,9)),
                 //April 10th
                Assignment.Create(Role.Num03, SA,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,4,10)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num04, RoB,new DateTime(2015,4,10)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num011, CC,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num09, CE,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num06, RE,new DateTime(2015,4,10)),
                Assignment.Create(Role.C2, NF,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num07, CG,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num01, JH,new DateTime(2015,4,10)),
                Assignment.Create(Role.PD, CH,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num05, SM,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num08, EM,new DateTime(2015,4,10)),
                Assignment.Create(Role.PM, LS,new DateTime(2015,4,10)),
                Assignment.Create(Role.PC, YS,new DateTime(2015,4,10)),
                Assignment.Create(Role.Num010, JT,new DateTime(2015,4,10)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,10)),
                 //April 11th
                Assignment.Create(Role.Off, SA,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,4,11)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,4,11)),
                Assignment.Create(Role.AM, NF,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,4,11)),
                Assignment.Create(Role.SevenToThree, EM,new DateTime(2015,4,11)),
                Assignment.Create(Role.PM, LS,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,4,11)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,11)),
                 //April 12th
                Assignment.Create(Role.Off, SA,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,4,12)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,4,12)),
                Assignment.Create(Role.AM, NF,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,4,12)),
                Assignment.Create(Role.PM, LS,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,4,12)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,12)),
                 //April 13th
                Assignment.Create(Role.V, SA,new DateTime(2015,4,13)),
                Assignment.Create(Role.Num04, StB,new DateTime(2015,4,13)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,4,13)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,13)),
                Assignment.Create(Role.PC, RB,new DateTime(2015,4,13)),
                Assignment.Create(Role.Num05, CC,new DateTime(2015,4,13)),
                Assignment.Create(Role.C2, CE,new DateTime(2015,4,13)),
                Assignment.Create(Role.Num06, RE,new DateTime(2015,4,13)),
                Assignment.Create(Role.PC, NF,new DateTime(2015,4,13)),
                Assignment.Create(Role.Num08, CG,new DateTime(2015,4,13)),
                Assignment.Create(Role.V, JH,new DateTime(2015,4,13)),
                Assignment.Create(Role.V, CH,new DateTime(2015,4,13)),
                Assignment.Create(Role.Num07, SM,new DateTime(2015,4,13)),
                Assignment.Create(Role.Num07, EM,new DateTime(2015,4,13)),
                Assignment.Create(Role.PM, LS,new DateTime(2015,4,13)),
                Assignment.Create(Role.Num01, YS,new DateTime(2015,4,13)),
                Assignment.Create(Role.Num03, JT,new DateTime(2015,4,13)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,4,13)),
                 //April 14th
                Assignment.Create(Role.V, SA,new DateTime(2015,4,14)),
                Assignment.Create(Role.CV, StB,new DateTime(2015,4,14)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num06, RB,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num07, CC,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num01    , CE,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num04, RE,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num011, NF,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num03, CG,new DateTime(2015,4,14)),
                Assignment.Create(Role.V, JH,new DateTime(2015,4,14)),
                Assignment.Create(Role.V, CH,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num05, SM,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num09, EM,new DateTime(2015,4,14)),
                Assignment.Create(Role.PM, LS,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num010, YS,new DateTime(2015,4,14)),
                Assignment.Create(Role.Num08, JT,new DateTime(2015,4,14)),
                Assignment.Create(Role.C2, CW,new DateTime(2015,4,14)),
                 //April 15th
                Assignment.Create(Role.V, SA,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num02, StB,new DateTime(2015,4,15)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,15)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num05, CC,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num010, CE,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num06, RE,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num03, NF,new DateTime(2015,4,15)),
                Assignment.Create(Role.C2, CG,new DateTime(2015,4,15)),
                Assignment.Create(Role.V, JH,new DateTime(2015,4,15)),
                Assignment.Create(Role.V, CH,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num01, SM,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num09, EM,new DateTime(2015,4,15)),
                Assignment.Create(Role.PM, LS,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num08, YS,new DateTime(2015,4,15)),
                Assignment.Create(Role.Num07, JT,new DateTime(2015,4,15)),
                Assignment.Create(Role.SevenToOne, CW,new DateTime(2015,4,15)),
                 //April 16th
                Assignment.Create(Role.V, SA,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num06, StB,new DateTime(2015,4,16)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,4,16)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num03, CC,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num09, CE,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num04, RE,new DateTime(2015,4,16)),
                Assignment.Create(Role.C2, NF,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num01, CG,new DateTime(2015,4,16)),
                Assignment.Create(Role.V, JH,new DateTime(2015,4,16)),
                Assignment.Create(Role.V, CH,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num010, SM,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num08, EM,new DateTime(2015,4,16)),
                Assignment.Create(Role.PM, LS,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num07, YS,new DateTime(2015,4,16)),
                Assignment.Create(Role.Num05, JT,new DateTime(2015,4,16)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,16)),
                 //April 17th
                Assignment.Create(Role.V, SA,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num04, StB,new DateTime(2015,4,17)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num06, RoB,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num02, RB,new DateTime(2015,4,17)),
                Assignment.Create(Role.C2, CC,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num07, CE,new DateTime(2015,4,17)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num01, NF,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num09, CG,new DateTime(2015,4,17)),
                Assignment.Create(Role.V, JH,new DateTime(2015,4,17)),
                Assignment.Create(Role.V, CH,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num08, SM,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num03, EM,new DateTime(2015,4,17)),
                Assignment.Create(Role.PC, LS,new DateTime(2015,4,17)),
                Assignment.Create(Role.Num05, YS,new DateTime(2015,4,17)),
                Assignment.Create(Role.PM, JT,new DateTime(2015,4,17)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,17)),
                 //April 18th
                Assignment.Create(Role.Off, SA,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,4,18)),
                Assignment.Create(Role.SevenToThree, SB,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,4,18)),
                Assignment.Create(Role.AM, CC,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,18)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,4,18)),
                Assignment.Create(Role.PM, JT,new DateTime(2015,4,18)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,18)),
                 //April 19th
                Assignment.Create(Role.Off, SA,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,4,19)),
                Assignment.Create(Role.AM, CC,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,19)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,4,19)),
                Assignment.Create(Role.PM, JT,new DateTime(2015,4,19)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,19)),
                 //April 20th
               Assignment.Create(Role.Num07, SA,new DateTime(2015,4,20)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,20)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,4,20)),
                Assignment.Create(Role.Num01, RoB,new DateTime(2015,4,20)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,4,20)),
                Assignment.Create(Role.PC, CC,new DateTime(2015,4,20)),
                Assignment.Create(Role.Num06, CE,new DateTime(2015,4,20)),
                Assignment.Create(Role.PC, RE,new DateTime(2015,4,20)),
                Assignment.Create(Role.Num09, NF,new DateTime(2015,4,20)),
                Assignment.Create(Role.Num08, CG,new DateTime(2015,4,20)),
                Assignment.Create(Role.Num02, JH,new DateTime(2015,4,20)),
                Assignment.Create(Role.Num05, CH,new DateTime(2015,4,20)),
                Assignment.Create(Role.C2, SM,new DateTime(2015,4,20)),
                Assignment.Create(Role.Num04, EM,new DateTime(2015,4,20)),
                Assignment.Create(Role.V, LS,new DateTime(2015,4,20)),
                Assignment.Create(Role.Num03, YS,new DateTime(2015,4,20)),
                Assignment.Create(Role.PM, JT,new DateTime(2015,4,20)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,4,20)),
                 //April 21st
                Assignment.Create(Role.Num03, SA,new DateTime(2015,4,21)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,21)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,21)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num02, RB,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num012, CC,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num04, CE,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num06, RE,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num08, NF,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num05, CG,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num011, JH,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num07, CH,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num01, SM,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num09, EM,new DateTime(2015,4,21)),
                Assignment.Create(Role.V, LS,new DateTime(2015,4,21)),
                Assignment.Create(Role.Num010, YS,new DateTime(2015,4,21)),
                Assignment.Create(Role.PM, JT,new DateTime(2015,4,21)),
                Assignment.Create(Role.C2, CW,new DateTime(2015,4,21)),
                 //April 22nd
                Assignment.Create(Role.C2, SA,new DateTime(2015,4,22)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,22)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num02, RoB,new DateTime(2015,4,22)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num03, CC,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num010, CE,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num04, RE,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num01, NF,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num08, CG,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num06, JH,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num05, CH,new DateTime(2015,4,22)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num09, EM,new DateTime(2015,4,22)),
                Assignment.Create(Role.V, LS,new DateTime(2015,4,22)),
                Assignment.Create(Role.Num07, YS,new DateTime(2015,4,22)),
                Assignment.Create(Role.PM, JT,new DateTime(2015,4,22)),
                Assignment.Create(Role.SevenToOne, CW,new DateTime(2015,4,22)),
                 //April 23rd
                Assignment.Create(Role.Num01, SA,new DateTime(2015,4,23)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,23)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num04, RoB,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num02, RB,new DateTime(2015,4,23)),
                Assignment.Create(Role.C2, CC,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num07, CE,new DateTime(2015,4,23)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num09, NF,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num06, CG,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num08, JH,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num03, CH,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num010, SM,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num05, EM,new DateTime(2015,4,23)),
                Assignment.Create(Role.V, LS,new DateTime(2015,4,23)),
                Assignment.Create(Role.Num06, YS,new DateTime(2015,4,23)),
                Assignment.Create(Role.PM, JT,new DateTime(2015,4,23)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,23)),
                //April 24th
                Assignment.Create(Role.Num010, SA,new DateTime(2015,4,24)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,24)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,24)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num01, CC,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num03, CE,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num09, NF,new DateTime(2015,4,24)),
                Assignment.Create(Role.PM, CG,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num06, JH,new DateTime(2015,4,24)),
                Assignment.Create(Role.C2, CH,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num05, SM,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num08, EM,new DateTime(2015,4,24)),
                Assignment.Create(Role.V, LS,new DateTime(2015,4,24)),
                Assignment.Create(Role.Num07, YS,new DateTime(2015,4,24)),
                Assignment.Create(Role.PC, JT,new DateTime(2015,4,24)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,24)),
                //April 25th
                Assignment.Create(Role.Off, SA,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,25)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,4,25)),
                Assignment.Create(Role.PM, CG,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,4,25)),
                Assignment.Create(Role.AM, CH,new DateTime(2015,4,25)),
                Assignment.Create(Role.SevenToThree, SM,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,4,25)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,25)),
                //April 26th
                Assignment.Create(Role.Off, SA,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,26)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,4,26)),
                Assignment.Create(Role.PM, CG,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,4,26)),
                Assignment.Create(Role.AM, CH,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,4,26)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,26)),
                 //April 27th
                Assignment.Create(Role.Num05, SA,new DateTime(2015,4,27)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,27)),
                Assignment.Create(Role.SevenToFive, SB,new DateTime(2015,4,27)),
                Assignment.Create(Role.PC, RoB,new DateTime(2015,4,27)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,4,27)),
                Assignment.Create(Role.V, CC,new DateTime(2015,4,27)),
                Assignment.Create(Role.C2, CE,new DateTime(2015,4,27)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,4,27)),
                Assignment.Create(Role.Num02, NF,new DateTime(2015,4,27)),
                Assignment.Create(Role.PM, CG,new DateTime(2015,4,27)),
                Assignment.Create(Role.Num06, JH,new DateTime(2015,4,27)),
                Assignment.Create(Role.PC, CH,new DateTime(2015,4,27)),
                Assignment.Create(Role.Num07, SM,new DateTime(2015,4,27)),
                Assignment.Create(Role.Num01, EM,new DateTime(2015,4,27)),
                Assignment.Create(Role.PD, LS,new DateTime(2015,4,27)),
                Assignment.Create(Role.Num03, YS,new DateTime(2015,4,27)),
                Assignment.Create(Role.Num08, JT,new DateTime(2015,4,27)),
                Assignment.Create(Role.SevenToFive, CW,new DateTime(2015,4,27)),
                 //April 28th
                Assignment.Create(Role.Num06, SA,new DateTime(2015,4,28)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,28)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num06, RoB,new DateTime(2015,4,28)),
                Assignment.Create(Role.CV, RB,new DateTime(2015,4,28)),
                Assignment.Create(Role.V, CC,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num01, CE,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num09, NF,new DateTime(2015,4,28)),
                Assignment.Create(Role.PM, CG,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num07, JH,new DateTime(2015,4,28)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num04, SM,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num010, EM,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num03, LS,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num08, YS,new DateTime(2015,4,28)),
                Assignment.Create(Role.Num05, JT,new DateTime(2015,4,28)),
                Assignment.Create(Role.C2, CW,new DateTime(2015,4,28)),
                 //April 29th
                Assignment.Create(Role.Num06, SA,new DateTime(2015,4,29)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,29)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num04, RoB,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num02, RB,new DateTime(2015,4,29)),
                Assignment.Create(Role.V, CC,new DateTime(2015,4,29)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,29)),
                Assignment.Create(Role.CV, RE,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num08, NF,new DateTime(2015,4,29)),
                Assignment.Create(Role.PM, CG,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num01, JH,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num03, CH,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num05, SM,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num09, EM,new DateTime(2015,4,29)),
                Assignment.Create(Role.C2, LS,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num010, YS,new DateTime(2015,4,29)),
                Assignment.Create(Role.Num07, JT,new DateTime(2015,4,29)),
                Assignment.Create(Role.SevenToOne, CW,new DateTime(2015,4,29)),
                 //April 30th
                Assignment.Create(Role.Num03, SA,new DateTime(2015,4,30)),
                Assignment.Create(Role.V, StB,new DateTime(2015,4,30)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,4,30)),
                Assignment.Create(Role.CV, RoB,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num04, RB,new DateTime(2015,4,30)),
                Assignment.Create(Role.V, CC,new DateTime(2015,4,30)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num02, RE,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num08, NF,new DateTime(2015,4,30)),
                Assignment.Create(Role.PM, CG,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num010, JH,new DateTime(2015,4,30)),
                Assignment.Create(Role.C2, CH,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num09, SM,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num07, EM,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num01, LS,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num05, YS,new DateTime(2015,4,30)),
                Assignment.Create(Role.Num06, JT,new DateTime(2015,4,30)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,4,30)),
            };

            Assignments.AddRange(assignments);
        }

        private void SeedMay()
        {
            var SA = Employee.Create("Shane", "Adams", "SA", DateTime.Now, "417-499-3116", "", "");
            var StB = Employee.Create("Steve", "Boeger", "StB", DateTime.Now, "417-624-3192", "", "");
            var SB = Employee.Create("Susie", "Boeger", "SB", DateTime.Now, "417-624-3192", "", "");
            var RoB = Employee.Create("Robin", "Boyd", "RoB", DateTime.Now, "417-624-2949", "", "");
            var RB = Employee.Create("Rick", "Brown", "RB", DateTime.Now, "417-496-3793", "", "");
            var CC = Employee.Create("Chris", "Crabtree", "CC", DateTime.Now, "417-830-6644", "", "");
            var CE = Employee.Create("Cary", "Edwards", "CE", DateTime.Now, "417-619-5745", "", "");
            var RE = Employee.Create("Ray", "Eisenmann", "RE", DateTime.Now, "417-425-7644", "", "");
            var NF = Employee.Create("Nancy", "Fiscus", "NF", DateTime.Now, "417-781-4152", "", "");
            var CG = Employee.Create("Chris", "Garde", "CG", DateTime.Now, "417-782-7869", "", "");
            var JH = Employee.Create("John", "Howard", "JH", DateTime.Now, "417-317-0777", "", "");
            var CH = Employee.Create("Cole", "Hughes", "CH", DateTime.Now, "417-728-4793", "", "");
            var SM = Employee.Create("Shelly", "Mabe", "SM", DateTime.Now, "417-766-6916", "", "");
            var EM = Employee.Create("Ed", "Messer", "EM", DateTime.Now, "417-381-6317", "", "");
            var LS = Employee.Create("Leanna", "Swager", "LS", DateTime.Now, "417-358-4426", "", "");
            var YS = Employee.Create("Yvonne", "Stanke", "YS", DateTime.Now, "417-862-9887", "", "");
            var JT = Employee.Create("Jim", "Tyler", "JT", DateTime.Now, "417-206-8005", "", "");
            var CW = Employee.Create("Curt", "Williams", "CW", DateTime.Now, "417-833-9007", "", "");

            var assignments = new List<IAssignment>
            {
                //May 1st
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,1)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,1)),
                 //May 2nd
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,2)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,2)),
                 //May 3rd
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,3)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,3)),
                 //May 4th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,4)),
                Assignment.Create(Role.AM, JH,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,4)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,4)),
                 //May 5th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,5)),
                Assignment.Create(Role.AM, JH,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,5)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,5)),
                 //May 6th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,6)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,6)),
                 //May 7th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,7)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,7)),
                 //May 8th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,8)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,8)),
                 //May 9th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,9)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,9)),
                 //May 10th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,10)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,10)),
                 //May 11th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,11)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,11)),
                 //May 12th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,12)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,12)),
                 //May 13th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,13)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,13)),
                 //May 14th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,14)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,14)),
                 //May 15th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,15)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,15)),
                 //May 16th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,16)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,16)),
                 //May 17th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,17)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,17)),
                 //May 18th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,18)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,18)),
                 //May 19th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,19)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,19)),
                 //May 20th
               Assignment.Create(Role.Off, SA,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,20)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,20)),
                 //May 21st
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,21)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,21)),
                 //May 22nd
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,22)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,22)),
                 //May 23rd
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,23)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,23)),
                 //May 24th
               Assignment.Create(Role.Off, SA,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,24)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,24)),
                 //May 25th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,25)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,25)),
                //May 26th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,26)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,26)),
                 //May 27th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,27)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,27)),
                 //May 28th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,28)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,28)),
                 //May 29th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,29)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,29)),
                 //May 30th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,30)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,30)),
                //May 31th
                Assignment.Create(Role.Off, SA,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, StB,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, SB,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, RoB,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, RB,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, CC,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, CE,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, RE,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, NF,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, CG,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, JH,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, CH,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, SM,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, EM,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, LS,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, YS,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, JT,new DateTime(2015,5,31)),
                Assignment.Create(Role.Off, CW,new DateTime(2015,5,31)),
            };

            Assignments.AddRange(assignments);
        }

        private void SeedDetailedSchedule()
        {
            var detailedSchedule = CreateEmptySchedule(DateTime.Now);
            detailedSchedule.DetailedAssignments[0].Surgeon = "Dr. XYZ";
            detailedSchedule.DetailedAssignments[1].Surgeon = "Dr. ABC";
            detailedSchedule.DetailedAssignments[2].Surgeon = "Dr. XYZ";
            detailedSchedule.DetailedAssignments[3].Surgeon = "Dr. ABC";
            detailedSchedule.DetailedAssignments[4].Surgeon = "Dr. XYZ";
            detailedSchedule.DetailedAssignments[5].Surgeon = "Dr. ABC";
            detailedSchedule.DetailedAssignments[6].Surgeon = "Dr. XYZ";
            detailedSchedule.DetailedAssignments[7].Surgeon = "Dr. Smith";
            detailedSchedule.DetailedAssignments[8].Surgeon = "Dr. Goldberg";
            detailedSchedule.DetailedAssignments[9].Surgeon = "Dr. XYZ";
            detailedSchedule.DetailedAssignments[10].Surgeon = "Dr. ABC";
            detailedSchedule.DetailedAssignments[11].Surgeon = "Dr. ABC";
            detailedSchedule.DetailedAssignments[12].Surgeon = "Dr. XYZ";
            detailedSchedule.DetailedAssignments[13].Surgeon = "Dr. Last";

            detailedSchedule.DetailedAssignments[0].Time = "0 HOURS";
            detailedSchedule.DetailedAssignments[1].Time = "1 HOUR";
            detailedSchedule.DetailedAssignments[2].Time = "2 HOURS";
            detailedSchedule.DetailedAssignments[3].Time = "3 HOUR";
            detailedSchedule.DetailedAssignments[4].Time = "4 HOURS";
            detailedSchedule.DetailedAssignments[5].Time = "5 HOUR";
            detailedSchedule.DetailedAssignments[6].Time = "6 HOURS";
            detailedSchedule.DetailedAssignments[7].Time = "7 HOUR";
            detailedSchedule.DetailedAssignments[8].Time = "8 HOURS";
            detailedSchedule.DetailedAssignments[9].Time = "9 HOUR";
            detailedSchedule.DetailedAssignments[10].Time = "10 HOURS";
            detailedSchedule.DetailedAssignments[11].Time = "11 HOUR";
            detailedSchedule.DetailedAssignments[12].Time = "12 HOURS";
            detailedSchedule.DetailedAssignments[13].Time = "13 HOUR";

            detailedSchedule.CallProviders[0].Name = "Jim Tyler";
            detailedSchedule.CallProviders[1].Name = "Jim Tyler";
            detailedSchedule.CallProviders[2].Name = "Jim Tyler";
            detailedSchedule.CallProviders[3].Name = "Jim Tyler";

            DetailedAssignments = detailedSchedule.DetailedAssignments;
            CallProviders = detailedSchedule.CallProviders;


        }

        private IDetailedSchedule CreateEmptySchedule(DateTime date)
        {
            var detailedSchedule = DetailedSchedule.Create();
            detailedSchedule.Date = date;

            var detailedAssignments = new List<IDetailedAssignment>();
            var endoscopyCarthage = CreateDefaultDetailedAssignment(date, "Endoscopy", Hospital.Carthage); detailedAssignments.Add(endoscopyCarthage);
            var or1Carthage = CreateDefaultDetailedAssignment(date, "OR #1", Hospital.Carthage); detailedAssignments.Add(or1Carthage);
            var or2Carthage = CreateDefaultDetailedAssignment(date, "OR #2", Hospital.Carthage); detailedAssignments.Add(or2Carthage);
            var or3Carthage = CreateDefaultDetailedAssignment(date, "OR #3", Hospital.Carthage); detailedAssignments.Add(or3Carthage);
            
            var endoscopy1 = CreateDefaultDetailedAssignment(date, "Endoscopy #1", Hospital.Joplin); detailedAssignments.Add(endoscopy1);
            var endoscopy2 = CreateDefaultDetailedAssignment(date, "Endoscopy #2", Hospital.Joplin); detailedAssignments.Add(endoscopy2);
            var bronchoscopy = CreateDefaultDetailedAssignment(date, "Bronchoscopy", Hospital.Joplin); detailedAssignments.Add(bronchoscopy);
            var cathLab = CreateDefaultDetailedAssignment(date, "Cath Lab", Hospital.Joplin); detailedAssignments.Add(cathLab);
            var pv = CreateDefaultDetailedAssignment(date, "PV", Hospital.Joplin); detailedAssignments.Add(pv);
            var ep = CreateDefaultDetailedAssignment(date, "EP", Hospital.Joplin); detailedAssignments.Add(ep);
            var mri = CreateDefaultDetailedAssignment(date, "MRI", Hospital.Joplin); detailedAssignments.Add(mri);
            var ob = CreateDefaultDetailedAssignment(date, "OB", Hospital.Joplin); detailedAssignments.Add(ob);
            var or1Joplin = CreateDefaultDetailedAssignment(date, "OR #1", Hospital.Joplin); detailedAssignments.Add(or1Joplin);
            var or2Joplin = CreateDefaultDetailedAssignment(date, "OR #2", Hospital.Joplin); detailedAssignments.Add(or2Joplin);
            var or3Joplin = CreateDefaultDetailedAssignment(date, "OR #3", Hospital.Joplin); detailedAssignments.Add(or3Joplin);
            var or4 = CreateDefaultDetailedAssignment(date, "OR #4", Hospital.Joplin); detailedAssignments.Add(or4);
            var or5 = CreateDefaultDetailedAssignment(date, "OR #5", Hospital.Joplin); detailedAssignments.Add(or5);
            var or6 = CreateDefaultDetailedAssignment(date, "OR #6", Hospital.Joplin); detailedAssignments.Add(or6);
            
            detailedSchedule.DetailedAssignments = detailedAssignments;

            var callProviders = new List<ICallProvider>();
            var cv = CreateDefaultCallProvider(date, "CV CRNA", "11"); callProviders.Add(cv);
            var c2 = CreateDefaultCallProvider(date, "C-2 CRNA", "11"); callProviders.Add(c2);
            var pmCrna = CreateDefaultCallProvider(date, "PM CALL CRNA", "18"); callProviders.Add(pmCrna);
            var pmDoc = CreateDefaultCallProvider(date, "PM DOC", "18"); callProviders.Add(pmDoc);
            
            detailedSchedule.CallProviders = callProviders;

            return detailedSchedule;
        }

        private ICallProvider CreateDefaultCallProvider(DateTime date,string provider, string inTime)
        {
            var callProvider = CreateEmptyCallProvider();
            callProvider.Date = date;
            callProvider.Provider = provider;
            callProvider.InTime = inTime;
            callProvider.Hospital = Hospital.Joplin;
            return callProvider;
        }

        private ICallProvider CreateEmptyCallProvider()
        {
            var callProvider = CallProvider.Create();
            callProvider.Date = DateTime.MinValue;
            callProvider.Provider = "";
            callProvider.Name = "";
            callProvider.Notes = "";
            callProvider.Hospital = Hospital.Unassigned;
            return callProvider;
        }

        private IDetailedAssignment CreateDefaultDetailedAssignment(DateTime date, string location, Hospital hospital)
        {
            var detailedAssignment = CreateEmptyDetailedAssignment();
            detailedAssignment.Date = date;
            detailedAssignment.Location = location;
            detailedAssignment.Hospital = hospital;
            return detailedAssignment;
        }

        private IDetailedAssignment CreateEmptyDetailedAssignment()
        {
            var detailedAssignment = DetailedAssignment.Create();
            detailedAssignment.Date = DateTime.MinValue;
            detailedAssignment.Location = "";
            detailedAssignment.Surgeon = "";
            detailedAssignment.Time = "";
            detailedAssignment.Provider = "";
            detailedAssignment.PeelOut = "";
            detailedAssignment.Hospital = Hospital.Unassigned;

            return detailedAssignment;
        }

        #endregion

        #region update

        public void UpdateAssignment(IAssignment assignment)
        {
            const string updateCommandString = "UPDATE ASSIGNMENTS SET Role=@Role, Date=@Date, EmployeeID=@EmployeeID WHERE ID=@ID";
            var parameters = GetAssignmentParameters(assignment);
            parameters.Add(new SqlParameter() { ParameterName = "@ID", Value = assignment.Id });
            DataInteraction.ExecuteNonQuery(updateCommandString, parameters, Connection);
        }

        public void UpdateDetailedAssignment(IDetailedAssignment detailedAssignment)
        {
            const string updateCommandString = "UPDATE DETAILEDASSIGNMENTS SET Location=@Location, Surgeon=@Surgeon, Time=@Time, Provider=@Provider, PeelOut=@PeelOut, Hospital=@Hospital, Date=@Date WHERE ID=@ID";
            var parameters = GetDetailedAssignmentParameters(detailedAssignment);
            parameters.Add(new SqlParameter() { ParameterName = "@ID", Value = detailedAssignment.Id });
            DataInteraction.ExecuteNonQuery(updateCommandString, parameters, Connection);
        }

        public void UpdateCallProvider(ICallProvider callProvider)
        {
            const string updateCommandString = "UPDATE CallProviders SET Provider=@Provider, Name=@Name, Notes=@Notes, InTime=@InTime, Hospital=@Hospital, Date=@Date WHERE ID=@ID";
            var parameters = GetCallProviderParameters(callProvider);
            parameters.Add(new SqlParameter() { ParameterName = "@ID", Value = callProvider.Id });
            DataInteraction.ExecuteNonQuery(updateCommandString, parameters, Connection);
        }

        #endregion

        #region add

        public void AddAssignment(IAssignment assignment)
        {
            var parameters = GetAssignmentParameters(assignment);
            InsertAssignment(parameters);
        }

        public void AddDetailedAssignment(IDetailedAssignment detailedAssignment)
        {
            var parameters = GetDetailedAssignmentParameters(detailedAssignment);
            InsertDetailedAssignment(parameters);
        }

        public void AddCallProvider(ICallProvider callProvider)
        {
            var parameters = GetCallProviderParameters(callProvider);
            InsertCallProvider(parameters);
        }

        #endregion

        #region delete

        public void DeleteAssignment(int id)
        {
            const string deleteCommandString = "DELETE FROM ASSIGNMENTS WHERE ID=@ID";
            var parameters = new List<SqlParameter>() { new SqlParameter() { ParameterName = "@ID", Value = id } };
            DataInteraction.ExecuteNonQuery(deleteCommandString, parameters, Connection);
        }

        public void DeleteDetailedAssignment(int id)
        {
            const string deleteCommandString = "DELETE FROM DETAILEDASSIGNMENTS WHERE ID=@ID";
            var parameters = new List<SqlParameter>() { new SqlParameter() { ParameterName = "@ID", Value = id } };
            DataInteraction.ExecuteNonQuery(deleteCommandString, parameters, Connection);
        }

        public void DeleteCallProvider(int id)
        {
            const string deleteCommandString = "DELETE FROM CALLPROVIDERS WHERE ID=@ID";
            var parameters = new List<SqlParameter>() { new SqlParameter() { ParameterName = "@ID", Value = id } };
            DataInteraction.ExecuteNonQuery(deleteCommandString, parameters, Connection);
        }

        #endregion
    }
}
