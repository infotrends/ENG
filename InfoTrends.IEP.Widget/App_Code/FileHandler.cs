using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for FileHandler
/// </summary>

public class FileHandler : IHttpHandler
{

    public bool IsReusable
    {
        get { return true; }
    }

    public void ProcessRequest(HttpContext context)
    {
        var res = context.Response;
        var req = context.Request;
        res.Write(context.Request.Url);
        var url = context.Request.Url.PathAndQuery.Trim().ToLower().TrimStart('/');
        var file = context.Server.MapPath("/") + url.Replace(".js", "");
        
        var sp = url.Split('/');
        var jsPart = sp[0].ToLower();


        if (jsPart == "js")
        {
            using (StreamReader sr = new StreamReader(file))
            {
                var js = ToJs(new Js { Value = sr.ReadToEnd() });
                res.Clear();
                res.ContentType = "text/javascript";
                res.Write(js);
            }
        }
        else
        {
            if (file.Contains("?"))
            {
                file = file.Split('?')[0];
            }
            res.Write(file);
            using (StreamReader sr = new StreamReader(file))
            {
                var txt = sr.ReadToEnd();
                var ext = Path.GetExtension(file);
                var contentType = ext == ".css" ? "text/css" : "text/html";
                res.Clear();
                res.ContentType = contentType;
                res.Write(txt);
            }
        }
        
    }

    string ToJs(Js jsObj)
    {
        var t = @"
            define([], function(){
                return {Js}
            });
        ";

        return t.Replace("{Js}", Newtonsoft.Json.JsonConvert.SerializeObject(jsObj));
    }
}

public class Js
{
    public string Value { get; set; }
}