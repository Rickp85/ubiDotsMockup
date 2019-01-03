using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using Ubidots;
using System.Configuration;
using log4net;
using log4net.Config;
using System.Collections.Generic;

namespace ubiDotsMockup
{
    class Program
    {

        private static readonly ILog logger = LogManager.GetLogger("Pippo");
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            XmlConfigurator.Configure();

            ApiClient client = new ApiClient("81ccc02cba03d9ae5a2ee1406603f064118233a8");
            Variable myTemp = client.GetVariable("589c3d0276254208983247d2");
            IFTTTConnector conn = new IFTTTConnector();
            NetatmoAPI api = new NetatmoAPI();
            NetatmoAccount myAcc = api.login();
            NetatmoDevices myDevices = api.getDevices(myAcc.access_token);
            double tempValue = -10;
            List<double> tempValuesm = new List<double>();
            int tempTime = 0;
            int lastTime = 0;

            string configvalue1 = ConfigurationSettings.AppSettings["NetatmoKey"];
            Console.WriteLine("Config: " + configvalue1);

            for (int count = 0; true; count++)
            {
                try
                {

                    Console.WriteLine("Inizio lettura n° " + count);
                        NetatmoMeasure measures = api.getMeasure(myAcc.access_token, myDevices.body.devices[0]._id, myDevices.body.devices[0].modules[0]._id);
                 //   NetatmoDevices dev = api.getDeviceInfo(myAcc.access_token, myDevices.body.devices[0]._id);

                    //Se c'e' almeno una misurazione valida (getMeasure ottiene misurazioni negli ultimi 10 minuti)
                  //  if (dev.status.Equals("ok") && dev.body.devices.Count > 0 && dev.body.devices[0].modules.Count > 0)
                  if(measures.status.Equals("ok") && measures.body.Count>0 && measures.body[0].value.Count > 0)
                    {

                        /*   tempValue = dev.body.devices[0].modules[0].measured.temperature;
                           tempTime = dev.body.devices[0].modules[0].measured.time;*/
                        tempValuesm = measures.body[0].value[0];
                        tempTime = measures.body[0].beg_time;

                        if (lastTime != tempTime)
                        {
                            Console.WriteLine("data: " + DateTime.Now.ToShortTimeString() + " valore: " + tempValue);

                            //Effettuo richiesta a IFTTT 
                            conn.doRequest("temp_rising", "la temperatura in salotto è di: " + tempValue.ToString() + "gradi alle " + DateTime.Now.ToShortTimeString());

                            //Salvo valore in ubidots
                            myTemp.SaveValue(tempValue);
                            lastTime = tempTime;
                        }

                        else
                        {
                            Console.WriteLine("data: " + DateTime.Now.ToShortTimeString() + " Nessuna nuova misurazione("+tempValue+" - "+tempTime +")");

                        }
                    }
                    else
                    {
                        conn.doRequest("temp_rising", "Errore nella lettura della temperatura");
                        Console.WriteLine("data: " + DateTime.Now.ToShortTimeString() + "Errore nella lettura della misurazione");
                    }


                 }
                catch (Exception e)
                {
                    Console.WriteLine("data: " + DateTime.Now.ToShortTimeString() + " Eccezione nella lettura della misurazione: "+e.Data + "-- "+e.Message);
                    conn.doRequest("temp_rising", "Eccezione nella lettura della temperatura: " + e.Message);
                    logger.Error(e);
                }

                Thread.Sleep(30000); //Attendo N min

            }


        }
    }
}
