using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace ubiDotsMockup
{
    class NetatmoAPI
    {

     


        public NetatmoAccount login()
        {
            RestClient client = new RestClient("https://api.netatmo.com/");

            var request = new RestRequest("oauth2/token", Method.POST);
            request.AddParameter("client_id", "5998a7162d3e0461318b8f4a"); // adds to POST or URL querystring based on Method
            request.AddParameter("client_secret", "JFYwD3Z9NFJ1Nk0f2YtYZnpIezfmJXxEnml7F7geVdni"); // adds to POST or URL querystring based on Method
            request.AddParameter("grant_type", "password"); // adds to POST or URL querystring based on Method
            request.AddParameter("username", "riccardopiazzi1@gmail.com"); // adds to POST or URL querystring based on Method
            request.AddParameter("password", "Abacus17"); // adds to POST or URL querystring based on Method
            request.AddParameter("scope", "read_thermostat"); // adds to POST or URL querystring based on Method

   

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            return JsonConvert.DeserializeObject<NetatmoAccount>(content);
           
        }


        public NetatmoDevices getDevices(string at)
        {
            RestClient client = new RestClient("https://api.netatmo.com/");

            var request = new RestRequest("api/getthermostatsdata", Method.GET);
            request.AddParameter("access_token", at); // adds to POST or URL querystring based on Method
       


            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            return JsonConvert.DeserializeObject<NetatmoDevices>(content);
            
        }


        public NetatmoDevices getDeviceInfo(string at, string deviceId)
        {
            RestClient client = new RestClient("https://api.netatmo.com/");

            var request = new RestRequest("api/getthermostatsdata", Method.GET);
            request.AddParameter("access_token", at); // adds to POST or URL querystring based on Method
            request.AddParameter("device_id", deviceId); // adds to POST or URL querystring based on Method

            
            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            return JsonConvert.DeserializeObject<NetatmoDevices>(content);

        }

        public NetatmoMeasure getMeasure(string ac, string deviceid, string moduleid)
        {
            RestClient client = new RestClient("https://api.netatmo.com/");

            Int32 uniBeg = (Int32)((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Subtract(new TimeSpan(0,10,0)))).TotalSeconds;

            Int32 unixNow = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var request = new RestRequest("api//getmeasure", Method.GET);
            request.AddParameter("device_id", deviceid); // adds to POST or URL querystring based on Method
            request.AddParameter("access_token", ac); // adds to POST or URL querystring based on Method
            request.AddParameter("scale", "max"); // adds to POST or URL querystring based on Method
            request.AddParameter("type", "temperature"); // adds to POST or URL querystring based on Method
            request.AddParameter("module_id", moduleid); // adds to POST or URL querystring based on Method
            request.AddParameter("date_begin", uniBeg); // adds to POST or URL querystring based on Method
            request.AddParameter("date_end", unixNow); // adds to POST or URL querystring based on Method



            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            return JsonConvert.DeserializeObject<NetatmoMeasure>(content);

        }
    }


    public class NetatmoAccount
    {
       
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public List<string> scope { get; set; }
            public int expires_in { get; set; }
            public int expire_in { get; set; }
       


    }



    // Netatmo devices
    public class LastBilan
    {
        public int y { get; set; }
        public int m { get; set; }
    }

    public class Place
    {
        public int altitude { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public bool improveLocProposed { get; set; }
        public List<double> location { get; set; }
        public string timezone { get; set; }
        public bool trust_location { get; set; }
    }

    public class Setpoint
    {
        public string setpoint_mode { get; set; }
    }

    public class Zone
    {
        public int type { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public int temp { get; set; }
    }

    public class Timetable
    {
        public int m_offset { get; set; }
        public int id { get; set; }
    }

    public class ThermProgramList
    {
        public List<Zone> zones { get; set; }
        public List<Timetable> timetable { get; set; }
        public string name { get; set; }
        public string program_id { get; set; }
        public bool selected { get; set; }
    }

    public class Measured
    {
        public int time { get; set; }
        public double temperature { get; set; }
        public int setpoint_temp { get; set; }
    }

    public class Module
    {
        public string _id { get; set; }
        public string module_name { get; set; }
        public string type { get; set; }
        public int firmware { get; set; }
        public int last_message { get; set; }
        public int rf_status { get; set; }
        public int battery_vp { get; set; }
        public int therm_orientation { get; set; }
        public int therm_relay_cmd { get; set; }
        public bool anticipating { get; set; }
        public int battery_percent { get; set; }
        public float last_therm_seen { get; set; }
        public Setpoint setpoint { get; set; }
        public List<ThermProgramList> therm_program_list { get; set; }
        public Measured measured { get; set; }
    }

    public class Device
    {
        public string _id { get; set; }
        public int firmware { get; set; }
        public LastBilan last_bilan { get; set; }
        public int last_setup { get; set; }
        public int last_status_store { get; set; }
        public Place place { get; set; }
        public int plug_connected_boiler { get; set; }
        public string type { get; set; }
        public bool udp_conn { get; set; }
        public int wifi_status { get; set; }
        public List<Module> modules { get; set; }
        public string station_name { get; set; }
        public int last_plug_seen { get; set; }
    }

    public class Administrative
    {
        public string lang { get; set; }
        public string reg_locale { get; set; }
        public int unit { get; set; }
        public int windunit { get; set; }
        public int pressureunit { get; set; }
        public int feel_like_algo { get; set; }
    }

    public class User
    {
        public string mail { get; set; }
        public Administrative administrative { get; set; }
    }

    public class Body
    {
        public List<Device> devices { get; set; }
        public User user { get; set; }
    }

    public class NetatmoDevices
    {
        public Body body { get; set; }
        public string status { get; set; }
        public double time_exec { get; set; }
        public int time_server { get; set; }
    }

    //end


    //Netatmo measuers
    public class BodyMeas
    {
        public int beg_time { get; set; }
        public int step_time { get; set; }
        public List<List<double>> value { get; set; }
    }

    public class NetatmoMeasure
    {
        public List<BodyMeas> body { get; set; }
        public string status { get; set; }
        public double time_exec { get; set; }
        public int time_server { get; set; }
    }
    //end

}
