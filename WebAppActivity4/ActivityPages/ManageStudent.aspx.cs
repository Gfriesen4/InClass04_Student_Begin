//InClass4 Start Kit
//You have been provided all the necessary Entities, Contexts, 
//and Controllers, do not change any of that code.
//You have been provided the Presentation Layer (ManageStudent.aspx file), do not change any of that code.
//You must complete this code behind file where necessary, 
//to complete the proper functionality of the Fetch and Update buttons only,
//(no Clear, Add, or Delete buttons needed).
//All method skeletons needed have been provided, 
//you just have to add code to them, do not create any other methods.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Activity4.BLL;
using Activity4.Entities;
using System.Text.RegularExpressions;

namespace WebAppActivity4.ActivityPages
{
    public partial class ManageStudent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //This method is complete, do not change.
            ShowMessage("", "");
            if (!Page.IsPostBack)
            {
                BindStudentList();
            }
        }
        protected Exception GetInnerException(Exception ex)
        {
            //This method is complete, do not change
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
        protected void ShowMessage(string message, string cssclass)
        {
            //This method is complete, do not change
            MessageLabel.Attributes.Add("class", cssclass);
            MessageLabel.InnerHtml = message;
        }
        protected void BindStudentList()
        {
            try
            {
                //Add code here to get all the Student records and bind to the "StudentList" dropdown

                StudentController sysmgr = new StudentController();
                List<Student> info = null;
                info = sysmgr.List();
                info.Sort((x, y) => x.StudentName.CompareTo(y.StudentName));
                StudentList.DataSource = info;
                StudentList.DataTextField = nameof(Student.StudentName);
                StudentList.DataValueField = nameof(Student.StudentID);
                StudentList.DataBind();
                ListItem myitem = new ListItem();
                myitem.Value = "0";
                myitem.Text = "select...";
                StudentList.Items.Insert(0, myitem);

            }
            catch (Exception ex)
            {
                ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
            }
        }
        protected void BindProgramList()
        {
            try
            {
                //Add code here to get all the Program records and bind to the "ProgramList" dropdown
                ProgramController sysmgr = new ProgramController();
                List<Program> info = null;
                info = sysmgr.List();
                info.Sort((x, y) => x.ProgramName.CompareTo(y.ProgramName));
                ProgramList.DataSource = info;
                ProgramList.DataTextField = nameof(Program.ProgramName);
                ProgramList.DataValueField = nameof(Program.ProgramID);
                ProgramList.DataBind();
                ListItem myitem = new ListItem();
                myitem.Value = "0";
                myitem.Text = "select...";
                ProgramList.Items.Insert(0, myitem);

            }
            catch (Exception ex)
            {
                ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
            }
        }
        protected void Fetch_Click(object sender, EventArgs e)
        {
            if (StudentList.SelectedIndex == 0)
            {
                ShowMessage("Please select a student first.", "alert alert-info");
            }
            else
            {
                try
                {
                    //Add code here to FindByPKID the ONE Student record and then populate all the textboxes,
                    //do not change the code above, only add code in this "try" structure
                    BindProgramList();
                    string studentID = StudentList.SelectedValue;
                    StudentController sysmgr = new StudentController();
                    Student info = null;
                    info = sysmgr.FindByPKID(int.Parse(studentID));
                    if (info == null)
                    {
                        ShowMessage("Record is not in Database.", "alert alert-info");
                        
                    }
                    else
                    {
                        ID.Text = info.StudentID.ToString();
                        StudentName.Text = info.StudentName;
                        ProgramList.SelectedValue = info.ProgramID.ToString(); 
                        Credits.Text = info.Credits.ToString();
                        EmergencyPhoneNumber.Text = info.EmergencyPhoneNumber.ToString();

                    }

                }
                catch (Exception ex)
                {
                    ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                }
            }
        }
        protected bool Validation(object sender, EventArgs e)
        {
            string input = EmergencyPhoneNumber.Text;
            Match match = Regex.Match(input, @"^[(][1-9][0-9][0-9][)][ ][1-9][0-9][0-9][-][0-9][0-9][0-9][0-9]$");
            if (!match.Success)
            {
                ShowMessage("Emergency Phone Number must be like (123) 123-1234", "alert alert-info");
                return false;
            }
            //Complete the code in this method to validate the following with codebehind
            //Student Name is required
            //Program is required
            //Credits is required and has to be a double between 0.00 and 100.00
            //Validation for the EmergencyPhoneNumber has been provided above, do not change
            else if (string.IsNullOrEmpty(StudentName.Text))
            {
                ShowMessage("Student Name is required", "alert alert-info");
                return false;
            }
            else if (ProgramList.SelectedValue == "0")
            {
                ShowMessage("Please Select a Program", "alert alert-info");
                return false;
            }            
            else if (string.IsNullOrEmpty(Credits.Text))
            {
                ShowMessage("Credits are required", "alert alert-info");
                return false;
            }
            else if (double.Parse(Credits.Text) < 0.00 || double.Parse(Credits.Text) > 100.00)
            {
                ShowMessage("Credits are required to be between 0.00 and 100.00", "alert alert-info");
                return false;
            }
            else
            {
                return true;
            }
        }
        protected void UpdateStudent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ID.Text))
            {
                ShowMessage("Please select a student and Fetch first.", "alert alert-info");
            }
            else
            {
                var isValid = Validation(sender, e);
                if (isValid)
                    try
                    {
                        //Add code here to Update the ONE Student record, 
                        //do not change the code above, only add code in this "try" structure
                        StudentController sysmgr = new StudentController();
                        Student item = new Student();
                        item.StudentID = int.Parse(ID.Text);
                        item.StudentName = StudentName.Text.Trim();                        
                        item.ProgramID = int.Parse(ProgramList.SelectedValue);
                        item.Credits = double.Parse(Credits.Text);
                        item.EmergencyPhoneNumber = EmergencyPhoneNumber.Text;
                        
                        int rowsaffected = sysmgr.Update(item);
                        if (rowsaffected > 0)
                        {
                            ShowMessage("Record has been UPDATED", "alert alert-success");
                        }
                        else
                        {
                            ShowMessage("Record was not found", "alert alert-warning");
                        }
                        
                    
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(GetInnerException(ex).ToString(), "alert alert-danger");
                    }
            }
        }
    }
}