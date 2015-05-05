using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.Data;
using CommonLibrary.Extensions;
using CommonLibrary.Utility;
using InfoTrendsCommon.DataContext;

namespace InfoTrendsAPI.Methods
{
    public class MyFile
    {
        public Guid? AttachmentGuid { get; set; }
        public string FileName { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { set; get; }


        /// <summary>
        /// Constructor
        /// </summary>
        public MyFile(string fileName, int contentLength, string contentType, byte[] data)
            : this(null, fileName, contentLength, contentType, data)
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public MyFile(Guid? attachmentGuid, string fileName, int contentLength, string contentType, byte[] data)
        {
            this.AttachmentGuid = attachmentGuid;
            this.FileName = fileName;
            this.ContentLength = contentLength;
            this.ContentType = contentType;
            this.Data = data;
        }


        #region STATIC


        /// <summary>
        /// From HttpPostedFile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static MyFile FromHttpPostedFile(HttpPostedFile postedFile)
        {

            string fileName = postedFile.FileName;
            int fileLength = postedFile.ContentLength;
            string fileType = postedFile.ContentType;

            //convert image to bytes
            byte[] fileBytes = new byte[fileLength];
            postedFile.InputStream.Read(fileBytes, 0, fileLength);

            //clean
            postedFile = null;

            //return
            return new MyFile(fileName, fileLength, fileType, fileBytes);
        }


        /// <summary>
        /// From SoftwareDb Attachment Guid
        /// </summary>
        /// <param name="attachmentGuid"></param>
        /// <returns></returns>
        public static MyFile FromSoftwareDbAttachmentGuid(Guid attachmentGuid)
        {
            string sqlStr = @" 
                SELECT 
                    downloadName, 
                    length, 
                    type, 
                    data 
                FROM {0} 
                WHERE 
                    attachmentGUID='{1}'; 
            ";

            // format
            sqlStr = sqlStr.Fmt(
                DbConstant.SoftwareDb.Table.Attachment,
                attachmentGuid
                );

            // execute
            using (DataTable dt = SqlServerUtil.GetData(sqlStr, DbConstant.SoftwareDb.ConnectionString))
            {
                if (dt.Rows.Count == 0)
                    return null;

                DataRow dr = dt.Rows[0];

                string fileName = (string)dr["downloadName"];
                int contentLength = (int)dr["length"];
                string contentType = (string)dr["type"];
                byte[] data = (byte[])dr["data"];

                MyFile myFile = new MyFile(attachmentGuid, fileName, contentLength, contentType, data);

                // return
                return myFile;
            }
        }



        #endregion
    }
}
