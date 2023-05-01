using System.Collections.Generic;
using System;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Data;

namespace KopkariFX
{

    public class BOProcessResult
    {
        /// <summary>
        /// kosongan. harus setting di dalemnya
        /// </summary>
        /// <param name="xProcessName"></param>
        public BOProcessResult()
        {

        }

        /// <summary>
        /// construct awal. selalu return isSuccess = false = slalu GAGAL !!!
        /// </summary>
        public BOProcessResult(string xProcessName)
        {
            this.isSuccess = false;
            this.xStatusCode = "500";
            this.xProcessName = xProcessName;
            //this.xMessage = "Internal Server Error";
            this.xMessageList = new List<string>();
        }


        //construct minimal
        public BOProcessResult(bool isSuccess, string xStatusCode, string xProcessName, string xMessage)
        {
            this.isSuccess = isSuccess;
            this.xStatusCode = xStatusCode;
            this.xProcessName = xProcessName;
            this.xMessage = xMessage;
            this.xMessageList = new List<string>();
        }

        /// <summary>
        /// ini construct yg paling lengkap
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="xStatusCode"></param>
        /// <param name="xProcessName"></param>
        /// <param name="xMessage"></param>
        /// <param name="xDataObject"></param>
        public BOProcessResult(bool isSuccess, string xStatusCode, string xProcessName, string xMessage, object xDataObject)
        {
            this.isSuccess = isSuccess;
            this.xStatusCode = xStatusCode;
            this.xProcessName = xProcessName;
            this.xMessage = xMessage;
            this.xDataObject = xDataObject;
            this.xMessageList = new List<string>();
        }


        /// <summary>
        /// klo construct error. pake yang ini. 
        /// </summary>
        /// <param name="xStatusCode"></param>
        /// <param name="xProcessName"></param>
        /// <param name="xMessageList"></param>
        public BOProcessResult(string xStatusCode, string xProcessName, List<string> xMessageList)
        {
            this.isSuccess = false;
            this.xStatusCode = xStatusCode;
            this.xProcessName = xProcessName;
            this.xMessageList = xMessageList;
            this.xDataObject = null;
        }


        /// <summary>
        /// construct sukses & ada hasil yg di return
        /// </summary>
        /// <param name="xProcessName"></param>
        /// <param name="xDataObject"></param>
        public BOProcessResult(string xProcessName, object xDataObject)
        {
            this.isSuccess = true;
            this.xStatusCode = "200";
            this.xProcessName = xProcessName;
            this.xDataObject = xDataObject;
            this.xMessageList = new List<string>();
        }




        /// <summary>
        /// hasil prosesnya sukses ato kagak
        /// </summary>
        public bool isSuccess { get; set; }


        /// <summary>
        /// kode status hasil ngejalanin proses. biasa ikutin kode http aja (500 = error)
        /// </summary>
        public string xStatusCode { get; set; }


        /// <summary>
        /// nama fungsi / proses yang dipanggil
        /// </summary>
        public string xProcessName { get; set; }



        /// <summary>
        /// message single line
        /// </summary>
        public string xMessage { get; set; }


        /// <summary>
        /// message multiple line. supaya gampang return ke UI nya jadi rapih berupa list
        /// </summary>
        public List<string> xMessageList { get; set; }


        /// <summary>
        /// message lengkap. union dari xMessage & xMessageList. untuk peralihan dari xmessagelist ke xmessage. 
        /// </summary>
        public string xMessageComplete
        {
            get
            {
                string xRetVal = "";

                if (xMessageList != null)
                {
                    xRetVal = string.Join("\n", xMessageCompleteList);
                }

                return xRetVal;
            }
        }



        /// <summary>
        /// message lengkap berupa list string. union dari xMessage & xMessageList. untuk peralihan dari xmessagelist ke xmessage. 
        /// </summary>
        public List<string> xMessageCompleteList
        {
            get
            {
                List<string> xRetVal = new List<string>();

                if (xMessageList != null)
                {
                    xRetVal = xMessageList;
                }

                //tambahin yang single line terakhir
                if (!string.IsNullOrWhiteSpace(xMessage))
                {
                    xRetVal.Add(xMessage);
                }

                //tambahin dalemnya lagi. 
                if (xInnerProcessResult != null)
                {
                    xRetVal.AddRange(xInnerProcessResult.xMessageCompleteList);
                }

                return xRetVal;
            }
        }



        /// <summary>
        /// hasil proses yang mau di return
        /// </summary>
        public object xDataObject { get; set; }


        /// <summary>
        /// buat tampung hasil proses dalemnya. jadi bisa bedain kayak exception.innerexception
        /// </summary>
        public BOProcessResult xInnerProcessResult { get; set; }

    }
}
