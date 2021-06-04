using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Employee_Payslip_project
{
    class Staff
    {
        private float hourlyRate;
        private int hWorked;

        public float TotalPay
        {
            get; protected set;
        }
        public float BasicPay
        {
            get; protected set;
        }
        public string NameOfStaff
        {
            get; protected set;
        }

        public int HoursWorked
        {
            get
            {
                return hWorked;
            }
            set
            {
                if (HoursWorked > 0)
                    hWorked = value;
                else hWorked = 0;
            }
        }

        public Staff(string name, float rate)
        {
            name = NameOfStaff;
            rate = hourlyRate;
        }
        public virtual void CalculatePay()
        {
            Console.WriteLine("Calculating Pay........");
            BasicPay = hWorked * hourlyRate;
            BasicPay = TotalPay;
        }
        public override string ToString()
        {
            return "\nName Of Staff:" + NameOfStaff + "\nHours Worked = " + hWorked + "\nHourly Rate = " + hourlyRate + "\nBasic Pay = " + BasicPay +
                "\nTotal Pay = " + TotalPay;
        }
    }

    class Manager : Staff
    {
        private const float managerHourlyRate = 50;
        public int Allowance
        {
            get; private set;

        }
        public Manager(string name) : base(name, managerHourlyRate)
        {

        }
        public override void CalculatePay()
        {
            base.CalculatePay();
            Allowance = 1000;
            if (HoursWorked > 160)
                TotalPay = BasicPay + Allowance;
            else TotalPay = BasicPay;
        }

        public override string ToString()
        {
            return "\nName Of Staff:" + NameOfStaff + "\nHours Worked = " + HoursWorked + "\nHourly Rate = " + managerHourlyRate + "\nBasic Pay = " +
                BasicPay + "\nTotal Pay = " + TotalPay;
        }

    }
    class Admin : Staff
    {
        private const float overtimeRate = 15.5f;
        private const float adminHourlyRate = 30;

        public float Overtime
        { get; private set;
        }

        public Admin (string name) : base (name, adminHourlyRate)
        {

        }
        public override void CalculatePay()
        {
            base.CalculatePay();
            Overtime = overtimeRate * (HoursWorked - 160);
            if (HoursWorked > 160)
                TotalPay = BasicPay + Overtime;
            else TotalPay = BasicPay;

        }

        public override string ToString()
        {
            return "\nName Of Staff:" + NameOfStaff + "\nHours Worked = " + HoursWorked + "\nHourly Rate = " + adminHourlyRate + "\nBasic Pay = " +
                BasicPay + "\nTotal Pay = " + TotalPay;
        }

        class FileReader
        {
            public List<Staff> ReadFile()
            {
                List<Staff> myStaff = new List<Staff> ();
                string[] result = new string[2];
                string path = "staff.txt";
                string[] separator = { "," };

                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        while (!sr.EndOfStream)
                        {
                            result =
                         sr.ReadLine().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (result[1] == "Manager")
                                myStaff.Add(new Manager(result[0]));
                            else if (result[1] == "Admin")
                                myStaff.Add(new Admin(result[0]));

                        }
                        sr.Close();
                    }
                }
                else { Console.WriteLine("Error: File does not exist"); }
                return myStaff;
            }
        }

        class PaySlip
        {
            private int month;
            private int year;
            enum MonthsOfYear { JAN = 1, FEB = 2, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC }
            MonthsOfYear myMonths = MonthsOfYear.JAN;

            public PaySlip (int payMonth, int payYear)
                {
                payMonth = month;
                payYear = year;
                 }

            public void GeneratePaySlip(List<Staff> myStaff)
            {
                string path;
                foreach (Staff f in myStaff)
                {
                    path = f.NameOfStaff + ".txt";
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        sw.WriteLine("PAYSLIP FOR {0}",(MonthsOfYear)month,year);
                        sw.WriteLine("=======================");
                        sw.WriteLine("Name Of Staff: {0} ",f.NameOfStaff);
                        sw.WriteLine("Hours Worked: {0}",f.HoursWorked );
                        sw.WriteLine("");
                        sw.WriteLine("Basic Pay: {0:C}",f.BasicPay);
                        if (f.GetType() == typeof(Manager))
                            sw.WriteLine("Allowance: {0:C}", ((Manager)f).Allowance);
                        else if (f.GetType() == typeof(Admin))
                            sw.WriteLine("Overtime: {0:C}", ((Admin)f).Overtime);
                        sw.WriteLine("");
                        sw.WriteLine("========================");
                        sw.WriteLine("Total Pay: {0}", f.TotalPay);
                        sw.WriteLine("========================");
                        sw.Close();


                            }
                  

                        }
                            
                    }
            public void GenerateSummary(List<Staff> myStaff)
            {
                var result

                   = from f in myStaff
                     where f.HoursWorked < 10
                     orderby f.NameOfStaff ascending
                     select new { f.NameOfStaff, f.HoursWorked };
                string path = "summary.txt";
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("Staff with less than 10 working hours");
                    sw.WriteLine("");
                    foreach (var f in result)
                        sw.WriteLine("Name Of Staff: {0}, Hours Worked: {1}", f.NameOfStaff, f.HoursWorked);
                    sw.Close();
                    
                }
            }
            public override string ToString()
            {
                return "Month = " + month + "Year = " + year;
            }

        }

    }

        
    
    

        


    class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine(" My Application Started");
                Console.ReadLine();
            }
        }
    }

