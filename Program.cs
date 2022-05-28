using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viotp
{
    class Program
    {
        public static viotp vi = new viotp();
       
        static void Main (string[] args)
        {
            Console.WriteLine("Được viết bởi Hex Trần!");
          
            // balance
            vi.token = "420566cdbb9e4e60b0756305c370cd27";
            vi.Update_Network(@"D:\study_fullstack_c#\winform\viotp\viotp\Networks.cs");

            /**Console.WriteLine(vi.getBalance()); // trả về số dư tiền trong tài khoản
            #region một số cái có thể lấy
            Console.WriteLine(vi.balance);
            Console.WriteLine(vi.status_code);
            Console.WriteLine(vi.status_message);
            // Jobject tyoe is vi.status_data
            #endregion
            Console.Clear();
            // get list networks
            Dictionary<int, string> list_sv = vi.getNetwork();
            foreach(KeyValuePair<int,string> items in list_sv)
            {
                Console.WriteLine(items.Key);
                Console.WriteLine(items.Value);
            }
          
            Console.Clear();
            // get list services
            List<viotp.Service> lsv = vi.getService();
            foreach (viotp.Service sv in lsv)
            {
                Console.WriteLine(sv.id + "||" + sv.name + "||" + sv.price);
            }**/

            vi.getNumber(2); // more, tự tìm hiểu đi haha
            /***viotp.OldNumberService oldnum = vi.getOldNumber(12962246);
            Console.WriteLine(vi.json);
            Console.WriteLine(oldnum.CreatedTime);***/
            List<viotp.OldNumberService> listz = vi.getHistory(1,2);
            foreach (viotp.OldNumberService item in listz)
            {
                Console.WriteLine(item.CreatedTime + "||" + item.ServiceName + "||" + item.ServiceID + "||" + item.Status);
            }



        }
    }
}
