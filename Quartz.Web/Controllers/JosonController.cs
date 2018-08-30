using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Quartz.Web.Controllers
{
    public class JosonController : Controller
    {
 
        public ActionResult Index()
        {

            string fileLogPath = AppDomain.CurrentDomain.BaseDirectory + "log\\";
            string fileLogName = "TestQuartz_" + DateTime.Now.ToLongDateString() + "_log.txt";

            String ReadData = ReadByStreamReader(fileLogPath + fileLogName);

            ViewBag.Data = ReadData;

            return View("~/Views/Joson.cshtml");
        }


        #region 读取文件

        /// <summary>
        /// 使用StreamReader读取文件，然后一行一行的输出
        /// </summary>
        /// <param name="path"></param>
        public String ReadByStreamReader(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            String line = String.Empty;
            String JosonStr = String.Empty;

            while ((line = sr.ReadLine()) != null)
            {
                JosonStr += "<BR>" + line.ToString();
            }

            return JosonStr;
        }


        /// <summary>
        /// 使用FileStream类进行文件的读取，并将它转换成char数组，然后输出
        /// </summary>
        /// <param name="path"></param>
        public char[] ReadByFileStream(string path)
        {

            #region 读取文件

            byte[] byData = new byte[100];
            char[] charData = new char[1000];

            FileStream file = new FileStream(path, FileMode.Open);

            try
            {

                file.Seek(0, SeekOrigin.Begin);
                file.Read(byData, 0, 100);

                //byData传进来的字节数组,用以接受FileStream对象中的数据
                //第2个参数是字节数组中开始写入数据的位置,它通常是0,表示从数组的开端文件中向数组写数据
                //最后一个参数规定从文件读多少字符.

                Decoder d = Encoding.Default.GetDecoder();
                d.GetChars(byData, 0, byData.Length, charData, 0);

                Console.WriteLine(charData);

            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally { file.Close(); }
            #endregion

            return charData;
        } 

        #endregion

    }
}
