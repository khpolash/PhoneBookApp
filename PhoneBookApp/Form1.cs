using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneBookApp
{
    public partial class Form1 : Form
    {
        String imgLoc = "";
        public Form1()
        {
            InitializeComponent();
        }

        string connectionString = ConfigurationManager.ConnectionStrings["phonebookdbconnection"].ConnectionString;

      

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string queryStringView1 = "SELECT * FROM phone_book_table where name='" + searchTextBox.Text + "'";
                SqlCommand commandview1 = new SqlCommand(queryStringView1, connection);
                SqlDataReader tableReader1 = commandview1.ExecuteReader();

                if (tableReader1.Read())
                {
                    showNameTextBox.Text = tableReader1["name"].ToString();
                    showMobileTextBox.Text = tableReader1["mobile"].ToString();
                    showPhoneTextBox.Text = tableReader1["phone"].ToString();
                    showFaxTextBox.Text = tableReader1["fax"].ToString();
                    showEmailTextBox.Text = tableReader1["email"].ToString();
                    showWebsiteTextBox.Text = tableReader1["website"].ToString();
                    showHomeDistrictTextBox.Text = tableReader1["home_district"].ToString();
                    showDateOfBirthTextBox.Text = tableReader1["date_of_barth"].ToString();
                    showReligionTextBox.Text = tableReader1["religion"].ToString();
                    showSexTextBox.Text = tableReader1["sex"].ToString();
                    showBolldGroupTextBox.Text = tableReader1["blood_group"].ToString();
                    byte[] img = (byte[])tableReader1["image"];
                    if (img == null)
                    {
                        showPictureBox.Image = null;
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(img);
                        showPictureBox.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    messageLabel.Text = @"Sorry no record found!!";
                }

                tableReader1.Close();
                connection.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            tLabel.Text = DateTime.Now.ToLongTimeString();
            dLabel.Text = DateTime.Now.ToLongDateString();
        }

        private void loadImageButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = @"JPG File(*.jpg)|*.jpg|GIF File(*.gif)|*.gif|All Files(*.*)|*.* ";
                dlg.Title = @"Select Picture";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    imgLoc = dlg.FileName.ToString();
                    savePictureBox.ImageLocation = imgLoc;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (IsParsonNameAlreadyExists())
            {
                MessageBox.Show(@"Name already Exists. Please add somthing or change");
                return;
            }
            if (IsMobileNoExists())
            {
                MessageBox.Show(@"Mobile number already save .");
                return;
            }
           
                try
                {
                    byte[] img = null;
                    var fs = new FileStream(imgLoc, FileMode.Open, FileAccess.Read);
                    var br = new BinaryReader(fs);
                    img = br.ReadBytes((int)fs.Length);


                    String name = saveNameTextBox.Text;
                    String mobile = saveMobileTextBox.Text;
                    String phone = savePhoneTextBox.Text;
                    String fax = saveFaxTextBox.Text;
                    String email = saveEmailTextBox.Text;
                    String website = saveWebsiteTextBox.Text;
                    String homeDistrict = saveDistrictComboBox.Text;
                    String dateOfBirth = saveDataofBirthMaskedTextBox.Text;
                    String religon = saveReligionComboBox.Text;
                    String sex = saveSexComboBox.Text;
                    String bloodGroup = saveBloodGroupComboBox.Text;


                    var connection = new SqlConnection(connectionString);
                    connection.Open();

                    string insertResult = "INSERT INTO phone_book_table(name, mobile, phone, fax, email, website, home_district, date_of_barth, religion, sex, blood_group, image)VALUES('" + name + "','" + mobile + "','" + phone + "','" + fax + "','" + email + "','" + website + "','" + homeDistrict + "','" + dateOfBirth + "','" + religon + "','" + sex + "','" + bloodGroup + "',@img)";

                    var command = new SqlCommand(insertResult, connection);
                    command.Parameters.Add(new SqlParameter("@img", img));
                    int numofrows = command.ExecuteNonQuery();
                    MessageBox.Show(@"Information successfully Stored.\n Thank You!");
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            
        }

        private bool IsMobileNoExists()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string selectQuery = "SELECT * FROM phone_book_table WHERE mobile='" + saveMobileTextBox.Text + "'";

            SqlCommand command = new SqlCommand(selectQuery, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            bool isMobileNoExists = false;
            while (reader.Read())
            {
                isMobileNoExists = true;
            }
            reader.Close();
            connection.Close();
            return isMobileNoExists;
        }

        private bool IsParsonNameAlreadyExists()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string selectQuery = "SELECT * FROM phone_book_table WHERE name='" + saveNameTextBox.Text + "'";

            SqlCommand command = new SqlCommand(selectQuery, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            bool isParsonNameExists = false;
            while (reader.Read())
            {
                isParsonNameExists = true;
            }
            reader.Close();
            connection.Close();
            return isParsonNameExists;
        }

        private void datePicPictureBox_Click(object sender, EventArgs e)
        {
           picDategroupBox.Show();
        }

        private void datePicMonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            saveDataofBirthMaskedTextBox.Text = datePicMonthCalendar.SelectionStart.ToShortDateString();
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            picDategroupBox.Visible = false;
        }
        private void searchUpdateButton_Click(object sender, EventArgs e)
        {
            try
            {

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string queryStringView1 = "SELECT * FROM phone_book_table where name='" + searchUpdateTextBox.Text + "'";
                SqlCommand commandview1 = new SqlCommand(queryStringView1, connection);
                SqlDataReader tableReader1 = commandview1.ExecuteReader();

                if (tableReader1.Read())
                {
                    updatetNameTextBox.Text = tableReader1["name"].ToString();
                    updateMobileTextBox.Text = tableReader1["mobile"].ToString();
                    updatePhoneTextBox.Text = tableReader1["phone"].ToString();
                    updateFaxTextBox.Text = tableReader1["fax"].ToString();
                    updateEmailTextBox.Text = tableReader1["email"].ToString();
                    updateWebsiteTextBox.Text = tableReader1["website"].ToString();
                    updateHomeDistrictTextBox.Text = tableReader1["home_district"].ToString();
                    updateBirthMaskedTextBox.Text = tableReader1["date_of_barth"].ToString();
                    updateReligionTextBox.Text = tableReader1["religion"].ToString();
                    updateSexTextBox.Text = tableReader1["sex"].ToString();
                    updateBloodGroupTextBox.Text = tableReader1["blood_group"].ToString();
                }
                else
                {
                    updateMessageLabel.Text = @"Sorry no record found!!";
                }

                tableReader1.Close();
                connection.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteSearchButton_Click(object sender, EventArgs e)
        {
            try
            {

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string queryStringView1 = "DELETE FROM phone_book_table WHERE name='" + deleteSearchTextBox.Text + "'";
                SqlCommand command = new SqlCommand(queryStringView1, connection);
                int numofrows = command.ExecuteNonQuery();
                deleteMessageLabel.Text = @"Information successfully DELETE.\n Thank You!";
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void viewAllContactButton_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string queryStringView1 = "SELECT * FROM phone_book_table";
                SqlCommand commandview1 = new SqlCommand(queryStringView1, connection);
                SqlDataReader tableReader1 = commandview1.ExecuteReader();

                contactDataGridView.Rows.Clear();
                while(tableReader1.Read())
                {
                    contactDataGridView.Rows.Add(tableReader1[1].ToString(), tableReader1[2].ToString(), tableReader1[3].ToString(), tableReader1[4].ToString(), tableReader1[11].ToString());
                }


                tableReader1.Close();
                connection.Close();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string queryStringView1 = "UPDATE phone_book_table SET name='" + updatetNameTextBox.Text + "', mobile='" + updateMobileTextBox.Text + "', phone='" + updatePhoneTextBox.Text + "', fax='" + updateFaxTextBox.Text + "', email='" + updateEmailTextBox.Text + "', website='" + updateWebsiteTextBox.Text + "', home_district='" + updateHomeDistrictTextBox.Text + "', religion='" + updateReligionTextBox.Text + "', sex='" + updateSexTextBox.Text + "', blood_group='" + updateBloodGroupTextBox.Text + "' where name='" + searchUpdateTextBox.Text + "'";
                SqlCommand command = new SqlCommand(queryStringView1, connection);
                int numofrows = command.ExecuteNonQuery();
                MessageBox.Show(@"Information successfully Update.\n Thank You!");
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
