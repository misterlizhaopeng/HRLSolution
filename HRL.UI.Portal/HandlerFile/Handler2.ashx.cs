﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRL.UI.Portal.HandlerFile
{
    /// <summary>
    /// Handler2 的摘要说明
    /// </summary>
    public class Handler2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string jsonVal1 = "{\"tooltip\":{\"trigger\": \"item\",\"formatter\": \"{a} <br />{b} : {c} ({d}%)\"},\"legend\": {\"orient\": \"vertical\",\"x\": \"left\",\"data\": [\"优秀党员\",\"困难党员\",\"一般党员\",\"劳模党员\",\"党员干部\"]},\"toolbox\": {\"show\": true,\"feature\": {\"mark\": {\"show\": true},\"dataView\": {\"show\": true,\"readOnly\": false},\"magicType\": {\"show\": true,\"type\": [\"pie\",\"funnel\"],\"option\": {\"funnel\": {\"x\": \"25%\",\"width\": \"50%\",\"funnelAlign\": \"left\",\"max\": 1548}}},\"restore\": {\"show\": true},\"saveAsImage\": {\"show\": true}}},\"calculable\": true,\"series\": [{\"name\": \"访问来源\",\"type\": \"pie\",\"radius\": \"55%\",\"center\": [\"50%\",\"60%\"],\"data\": [{\"value\": 35,\"name\": \"优秀党员\"},{\"value\": 69,\"name\": \"困难党员\"},{\"value\": 375,\"name\": \"一般党员\"},{\"value\": 30,\"name\": \"劳模党员\"},{\"value\": 4,\"name\": \"党员干部\"}]}]}";
            


            context.Response.Write(jsonVal1);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}