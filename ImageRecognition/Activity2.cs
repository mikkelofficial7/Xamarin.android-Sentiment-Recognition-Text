using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ImageRecognition
{
    [Activity(Label = "Beri Kami Saran")]
    public class Activity2 : Activity
    {
        private EditText name;
        private EditText age;
        private EditText suggestion;
        private Button btnSend;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Activity2);

            //definisikan edit textnya
            name = FindViewById<EditText>(Resource.Id.editText1);
            age = FindViewById<EditText>(Resource.Id.editText2);
            suggestion = FindViewById<EditText>(Resource.Id.editText3);
            btnSend = (Button)FindViewById(Resource.Id.button1);

            Button btnBack = (Button)FindViewById(Resource.Id.button2);
            btnBack.Click += delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
            btnSend.Click += delegate
            {
                Toast.MakeText(this, "Sedang Memproses...", ToastLength.Short).Show();
                try
                {
                    string connsqlstring = string.Format("Server=tcp:xamarinuser.database.windows.net,1433;Data Source=tcp:xamarinuser.database.windows.net,1433;Initial Catalog=user;User ID=YOUR_ID@xamarinuser;Password=YOUR_PASSWORD");
                    //koneksi ke database
                    using (SqlConnection con = new SqlConnection(connsqlstring))
                    {
                        string Name = name.Text;
                        string Age = age.Text;
                        string Suggestion = suggestion.Text;

                        Guid g = Guid.NewGuid();
                        string GuidString = Convert.ToBase64String(g.ToByteArray());
                        GuidString = GuidString.Replace("=", "");
                        GuidString = GuidString.Replace("+", "");

                        if (Name != "" && Age != "" && Suggestion != "")
                        {
                            string query = "INSERT INTO [user] (id,name,age,suggestion) Values ('" + GuidString + "','" + Name + "','" + Age + "','" + Suggestion + "')";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            Toast.MakeText(this, "Data berhasil disimpan, terima kasih", ToastLength.Short).Show();
                            cmd.Connection.Close();
                            name.Text = "";
                            age.Text = "";
                            suggestion.Text = "";
                        }
                        else
                        {
                            Toast.MakeText(this, "Oops..tidak boleh ada yg kosong", ToastLength.Short).Show();
                        }
                    }
                }
                catch (Exception exception)
                {
                    AlertDialog.Builder connectionException = new AlertDialog.Builder(this);
                    connectionException.SetTitle("Connection Error");
                    connectionException.SetMessage(exception.ToString());
                    connectionException.SetNegativeButton("Return", delegate { });
                    connectionException.Create();
                    connectionException.Show();
                }
            };
        }
    }
}