using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using xNet;

namespace viotp
{
    public class viotp
    {
        // get and set
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public int balance { get; set; }
        public string token { get; set; }
        public string json { get; set; }
        // set
        public const string urlVio = "https://api.viotp.com";
        public const string urlVio_User = urlVio+"/users/";
        public const string urlVio_Networks = urlVio+"/networks/";
        public const string urlVio_Service = urlVio + "/service/";
        public const string urlVio_Resquest = urlVio + "/request/";
        public const string urlVio_Session = urlVio + "/session/";
        //client
        private HttpRequest rq = new HttpRequest();

        //get
        public int status_code;
        public string status_message;
        public string status_success;
        public JObject status_data;

        // class

        public class Service
        {
            public int id { get; set; }
            public string name { get; set; }
            public int price { get; set; }
        }


        public class Number
        {
            public int phone_number { get; set; }
            public int balance { get; set;  }
            public int request_id { get; set; }
        }


        public class OldNumberService
        {
           
            public int ID { get; set; }
            public int ServiceID { get; set; }
            public string ServiceName { get; set; }
            public int Status { get; set; }
            public int Price { get; set; }
            public string Phone { get; set; }
            public string SmsContent { get; set; }
            public string IsSound { get; set; }
            public DateTime CreatedTime { get; set; }
            public string Code { get; set; }
        }

        /// <summary>
        /// Get history
        /// </summary>
        /// <param name="service"></param>
        /// <param name="status"></param>
        /// <param name="limit"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns>List<OldNumberService></returns>
        public List<OldNumberService> getHistory(int service = 1, int status = 1, int limit = 100 , string StartDate = "", string EndDate = "")
        {
            if (checkToken())
            {
                string url = getSerivceUrl(5) + "history?token=" + token + "&service=" + service + "&status=" + status + "&limit=" + limit;
                if (StartDate != "" || StartDate != "")
                    url += "&fromDate=" + StartDate + "&toDate=" + EndDate;
                else 
                    url += "&fromDate=" + DateTime.Now.ToString("yyyy-MM-dd") + "&toDate=" + DateTime.Now.ToString("yyyy-MM-dd");
                Console.WriteLine(url);
                string content = (string)rq.Get(url).ToString();
                json = content;
                JObject jContent = JObject.Parse(content);
                status_code = (int)jContent["status_code"];
                status_message = (string)jContent["message"];
                status_success = (string)jContent["success"];
                List<OldNumberService> lhis = new List<OldNumberService>();
                if (status_code == 200)
                {
                    foreach (JObject Hex in jContent["data"])
                    {
                        OldNumberService num = new OldNumberService();
                        num.ID = (int)Hex["ID"];
                        num.SmsContent = (string)Hex["SmsContent"];
                        num.Phone = (string)Hex["Phone"];
                        num.CreatedTime = (DateTime)Hex["CreatedTime"];
                        num.ServiceID = (int)Hex["ServiceID"];
                        num.Status = (int)Hex["Status"];
                        num.IsSound = (string)Hex["IsSound"];
                        num.Code = (string)Hex["Code"];
                        num.Price = (int)Hex["Price"];
                        lhis.Add(num);
                    }
                    return lhis;
                }

                return null;
            }
            return null;
        }
        /// <summary>
        /// Get old number
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>OldNumberService</returns>
        public OldNumberService getOldNumber (int id)
        {
            if (checkToken())
            {
                string url = getSerivceUrl(5) + "get?token=" + token + "&requestId=" + id;
                string content = (string)rq.Get(url).ToString();
                json = content;
                JObject jContent = JObject.Parse(content);
                status_code = (int)jContent["status_code"];
                status_message = (string)jContent["message"];
                status_success = (string)jContent["success"];
                if(status_code == 200){
                    OldNumberService num = new OldNumberService();
                    num.ID = (int)jContent["data"]["ID"];
                    num.SmsContent = (string)jContent["data"]["SmsContent"];
                    num.Phone = (string)jContent["data"]["Phone"];
                    num.CreatedTime = (DateTime)jContent["data"]["CreatedTime"];
                    num.ServiceID = (int)jContent["data"]["ServiceID"];
                    num.Status = (int)jContent["data"]["Status"];
                    num.IsSound = (string)jContent["data"]["IsSound"];
                    num.Code = (string)jContent["data"]["Code"];
                    num.Price = (int)jContent["data"]["Price"];
                    return num;
                }
                return null;
            }
            return null;
        }
        /// <summary>
        /// Get phone number
        /// Lấy số sđt
        /// </summary>
        /// <param name="serviceId">Id của dịch vụ</param>
        /// <param name="networks">Nhà mạng muốn lấy số (ex: MOBIFONE, VINAPHONE, VIETTEL ,VIETNAMOBILE, ITELECOM)</param>
        /// <param name="prefix"> Đầu số muốn lấy số (ex: 94|96|97|98)</param>
        /// <param name="exceptPrefix">Đầu số không muốn lấy số (ex: 94|96|97|98)</param>
        /// <param name="number">number</param>
        /// <returns></returns>
        public Number getNumber (int serviceId, List<Networks> networks = null, string prefix="", string exceptPrefix="", string number = "")
        {
            
            if (checkToken())
            {

                string url = getSerivceUrl(4) + "get?token=" + token + "&" + "serviceId=" + serviceId;
                // get service
                string[] NUM = new string[getNetwork().Count];
                if (networks == null)
                {    // MOBIFONE|VINAPHONE|VIETTEL|VIETNAMOBILE|ITELECOM
                    int STT = 0;
                    foreach (KeyValuePair<int, string> items in getNetwork())
                    {
                        NUM[STT] = items.Value;
                        STT++;
                    }

                }
                else
                {
                    NUM = new string[networks.Count];
                    for (int i = 0; i < networks.Count; i++)
                    {
                        Networks net = networks[i];
                        NUM[i] = Convert.ToString(net);
                    }
                }

                url += "&network=" + string.Join("|", NUM);

                if (prefix != "")
                    url += "&prefix=" + prefix;

                if (exceptPrefix != "")
                    url += "&exceptPrefix=" + exceptPrefix;

                if (number != "")
                    url += "&number=" + number;

                string content = (string)rq.Get(url).ToString();
                json = content;
                JObject jContent = JObject.Parse(content);
                status_code = (int)jContent["status_code"];
                status_message = (string)jContent["message"];
                status_success = (string)jContent["success"];
                Console.WriteLine(content);
                Number num = new Number();

                if(status_code == 200)
                {
                    try
                    {
                        num.phone_number = (int)jContent["data"]["phone_number"];
                        num.balance = (int)jContent["data"]["balance"];
                        num.request_id = (int)jContent["data"]["request_id"];

                        return num;
                    } catch
                    {
                        return null;
                    }
                    
                }
                return null;
            }
            else
            {
                return null;
            }

            return null;
        }


        public void Update_Network(string path)
        {
            if (checkToken())
            {
                string code = "using System;\n";
                code += "using System.Collections.Generic;\n";
                code += "using System.Linq;\n";
                code += "using System.Text;\n";
                code += "using System.Threading.Tasks;\n";
                code += "namespace viotp\n";
                code += "{\n";
                code += "   public enum Networks\n";
                code += "       {\n";
                foreach (KeyValuePair<int, string> items in getNetwork())
                {
                    code += "           " + items.Value + ",\n";
                }
                code += "       }\n";
                code += "}";

                File.WriteAllText(path, code);

                Console.WriteLine(
                    "Cập nhật dữ liệu nhà mạng thành công!!\n",
                    "Được viết bởi Hex Trần, chúc bạn sử dụng vui vẻ!"
                );
            } else
            {
                Console.WriteLine("Cập nhật thất bại, có lẻ thiếu Token!");
            }

            
        }
        /// <summary>
        /// get service URL
        /// </summary>
        /// <param name="serviceId">1. user || 2. network || 3. service || 4.request</param>
        /// <returns>url</returns>
        public string getSerivceUrl(int serviceId)
        {
            switch (serviceId)
            {
                case 1:
                    return urlVio_User;
                case 2:
                    return urlVio_Networks;
                case 3:
                    return urlVio_Service;
                case 4:
                    return urlVio_Resquest;
                case 5:
                    return urlVio_Session;
                default:
                    return "";
                
            }
        }

        /// <summary>
        /// get list services
        /// </summary>
        /// <returns>List</returns>
        public List<Service> getService()
        {

            if (checkToken())
            {
                string content = (string)rq.Get(getSerivceUrl(3) + "get?token=" + token).ToString();
                json = content;
                JObject jContent = JObject.Parse(content);
                status_code = (int)jContent["status_code"];
                status_message = (string)jContent["message"];
                status_success = (string)jContent["success"];
                List<Service> lsv = new List<Service>();
                if(status_code == 200)
                {
                    JArray jData = (JArray)jContent["data"];
                    foreach (JObject j in jData)
                    {
                        Service sv = new Service();
                        sv.id = (int)j["id"];
                        sv.name = (string)j["name"];
                        sv.price = (int)j["price"];
                        lsv.Add(sv);
                    }
                    return lsv;
                }
                return null;
            } else
            {
                return null;
            }
            return null;
            
        }

        /// <summary>
        /// get list networks
        /// </summary>
        /// <returns>Dictionary</returns>
        public Dictionary<int,string> getNetwork()
        {
            if (checkToken())
            {
                string content = (string)rq.Get(getSerivceUrl(2) + "get?token=" + token).ToString();
                json = content;
                JObject jContent = JObject.Parse(content);
                status_code = (int)jContent["status_code"];
                status_message = (string)jContent["message"];
                status_success = (string)jContent["success"];
                //status_data = (JObject)jContent["data"];
                Dictionary<int, string> svrall = new Dictionary<int, string>();
                if (status_code == 200)
                {
                    JArray jData = (JArray)jContent["data"];
                    foreach (JObject j in jData)
                    {
                        svrall.Add((int)j["id"], (string)j["name"]);
                    }
                    return svrall;
                }
               
                return null;
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// get balance
        /// </summary>
        /// <returns>string</returns>
        public string getBalance()
        {
            if (checkToken())
            {
                try
                {
                    
                    string content = (string)rq.Get(getSerivceUrl(1) + "balance?token=" + token).ToString();
                    json = content;
                    JObject jContent = JObject.Parse(content);
                    status_code = (int)jContent["status_code"];
                    status_message = (string)jContent["message"];
                    status_success = (string)jContent["success"];
                    status_data = (JObject)jContent["data"];
                    balance = (int)jContent["data"]["balance"];
                    return balance + "";
                }
                catch 
                {
                    return "Token is not vaild";
                }
                
            }
            else
            {
                return "Missing tokens";
            }
        }

        /// <summary>
        /// check vaild token
        /// </summary>
        /// <returns>bool</returns>
        private bool checkToken()
        {
            if (token.Length > 0)
                return true;
            else
                return false;
        }
    }
}
