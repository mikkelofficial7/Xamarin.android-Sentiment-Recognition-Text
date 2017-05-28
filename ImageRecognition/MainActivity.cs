using Android.App;
using Android.Widget;
using Newtonsoft.Json;
using Android.Content;
using Android.Support.V7.App;
using ImageRecognition.Model;
using System.Diagnostics;
using Android.OS;
using System.Net;
using System.IO;
using System;
using Android.Views;
using System.Threading.Tasks;
using System.Threading;
using Android.Graphics;

namespace ImageRecognition
{
    [Activity(Label = "Text Sentiment", MainLauncher = true, Icon = "@drawable/icons", Theme = "@style/Theme.AppCompat.Light.DarkActionBar")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string type1 = "/mood/";
            string type2 = "/Sentiment/";
            string author1 = "/prfekt/";
            string author2 = "/uClassify/";
            string APIKEY = "classify/?readKey=YOUR_API_KEY&text=";
            string baseAPI = "https://api.uclassify.com/v1";


            SetContentView(Resource.Layout.Main);

            EditText textInput = FindViewById<EditText>(Resource.Id.TextInput);
            TextView MtextResult1 = FindViewById<TextView>(Resource.Id.textResultMood1);
            TextView StextResult1 = FindViewById<TextView>(Resource.Id.textResultSentiment1);
            TextView MtextResult2 = FindViewById<TextView>(Resource.Id.textResultMood2);
            TextView StextResult2 = FindViewById<TextView>(Resource.Id.textResultSentiment2);
            TextView txtConcl = FindViewById<TextView>(Resource.Id.textConclusion);
            TextView txtview = FindViewById<TextView>(Resource.Id.textView);
            Button btnProses = FindViewById<Button>(Resource.Id.buttonProcess);
            Button btnFeed = FindViewById<Button>(Resource.Id.buttonFeedback);

            btnFeed.Click += delegate
            {
                var intent = new Intent(this, typeof(Activity2));
                StartActivity(intent);
            };
            btnProses.Click += delegate
            {
                string sentence = textInput.Text;
                if (sentence.Length > 0)
                {
                    ProgressDialog pDialog = new ProgressDialog(this);
                    pDialog.SetCancelable(false);
                    pDialog.SetMessage("Analisa...");
                    pDialog.Show();
                    new Thread(new ThreadStart(delegate
                    {
                        string urlMood = baseAPI + author1 + type1 + APIKEY + sentence;
                        string urlSentiment = baseAPI + author2 + type2 + APIKEY + sentence;

                        //MOOD VERSION
                        WebRequest request = WebRequest.Create(urlMood);
                        WebResponse response = request.GetResponse();
                        Stream data = response.GetResponseStream();
                        StreamReader reader = new StreamReader(data);

                        //SENTIMENT VERSION
                        WebRequest requests = WebRequest.Create(urlSentiment);
                        WebResponse responses = requests.GetResponse();
                        Stream datas = responses.GetResponseStream();
                        StreamReader readers = new StreamReader(datas);

                        RunOnUiThread(() => {

                            string JSON_MOOD = reader.ReadToEnd();
                            string JSON_SENTIMENT = readers.ReadToEnd();

                            Mood feel = JsonConvert.DeserializeObject<Mood>(JSON_MOOD);
                            Sentiment feels = JsonConvert.DeserializeObject<Sentiment>(JSON_SENTIMENT);

                            pDialog.Dismiss();

                            double happy = Math.Round(Convert.ToDouble(feel.happy) * 100, 1);
                            double upset = Math.Round(Convert.ToDouble(feel.upset) * 100, 1);
                            double positive = Math.Round(Convert.ToDouble(feels.positive) * 100, 1);
                            double negative = Math.Round(Convert.ToDouble(feels.negative) * 100, 1);

                            MtextResult1.Text = "Rasa Senang   :  " + happy + "%";
                            MtextResult2.Text = "Rasa Tertekan :  " + upset + "%";
                            StextResult1.Text = "Aura Positif  :  " + positive + "%";
                            StextResult2.Text = "Aura Negative :  " + negative + "%";
                            txtview.Text = "Kesimpulan : ";
                            if (happy >= upset && positive >= negative)
                            {
                                MtextResult1.SetTextColor(Color.Green);
                                StextResult1.SetTextColor(Color.Green);
                                txtConcl.Text = "\"Anda sangat menjaga ucapan dan mood anda hari ini\"";
                            }
                            else if (happy >= upset && positive < negative)
                            {
                                MtextResult1.SetTextColor(Color.Green);
                                StextResult2.SetTextColor(Color.Red);
                                txtConcl.Text = "\"Anda sangat menjaga mood anda namun anda harus berhati-hati dalam berucap\"";
                            }
                            if (happy < upset && positive >= negative)
                            {
                                MtextResult2.SetTextColor(Color.Red);
                                StextResult1.SetTextColor(Color.Green);
                                txtConcl.Text = "\"Mood anda sedang tidak baik namun ucapan anda cukup terjaga hari ini\"";
                            }
                            if (happy < upset && positive < negative)
                            {
                                MtextResult2.SetTextColor(Color.Red);
                                StextResult2.SetTextColor(Color.Red);
                                txtConcl.Text = "\"Mood dan ucapan anda buruk hari ini, mohon berhati-hati dalam melakukan aktivitas\"";
                            }
                            textInput.Text = "";
                        });

                    })).Start();
                }
                else
                {
                    Toast.MakeText(this, "Field Text tidak boleh kosong", ToastLength.Short).Show();
                }

            };
        }
    }
}  
