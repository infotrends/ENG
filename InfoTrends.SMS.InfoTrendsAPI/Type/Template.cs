using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoTrendsAPI
{
    public class Template
    {
        /// <summary>
        /// constant
        /// </summary>
        public const string PARAM_TITLE 
            = "{Title}";

        public const string PARAM_CONTENT 
            = "{Content}";


        /// <summary>
        /// email base template for ULTIMATE GUIDE
        /// </summary>
        /// <returns></returns>
        public static string GetEmailBaseTemplate()
        {
            string emailTemplate = @"
            <html>
            <head></head>
            <body>
                <table width='100%' cellpadding='0' cellspacing='0' style='background: #fff;'>
                    <tr>
                        <td align='left' style='background: #444;'>
                            <table  width='100%' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td>
                                        <a target='_blank' href='http://ultimateguide.infotrends.com'>
                                            <img src='http://ultimateguide.infotrends.com/ug2/images/logo/email_logo.png' alt='logo' border='0' />
                                        </a>
                                    </td>
                                    <td width='100%' align='right'>
                                        <table width='100%'>
                                            <tr>
                                                <td width='100%'></td>
                                                <td><a target='_blank' style='font-family: Arial; font-size: 11px; color: #fff; text-decoration: none;' href='http://ultimateguide.infotrends.com/UltimateGuides/home.html?action=signin'>Sign&nbsp;in</a>&nbsp;&nbsp;</td>
                                                <td><a target='_blank' style='font-family: Arial; font-size: 11px; color: #fff; text-decoration: none;' href='http://ultimateguide.infotrends.com/UltimateGuides/home.html?action=signup'>Sign&nbsp;up</a>&nbsp;&nbsp;</td>
                                                <td><a target='_blank' style='font-family: Arial; font-size: 11px; color: #fff; text-decoration: none;' href='http://ultimateguide.infotrends.com/UltimateGuides/contact.html'>Contact</a>&nbsp;&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td style='font-family: Arial; font-size: 12px; padding: 30px;'>
                            <div style='font-size: 18px; margin: 0; font-weight: normal;'>
                                {Title}
                            </div>
                            <br />
                            <div>
                                {Content}
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td align='left' style='font-family: Arial; font-size: 11px; padding: 5px 10px; color: #444; background: #dadada;'>
                            © {CopyrightYear} InfoTrends Inc
                        </td>
                    </tr>

                </table>
            </body>
            </html>
            ";

            //replace copyright year
            emailTemplate = emailTemplate.Replace("{CopyrightYear}", DateTime.Now.Year.ToString());

            //return
            return emailTemplate;
        }
    
    
    
    }
}
