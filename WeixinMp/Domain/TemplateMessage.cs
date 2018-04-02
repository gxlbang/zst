﻿/*
 * 姓名:gxlbang
 * 类名:TemplateMessage
 * CLR版本：4.0.30319.42000
 * 创建时间:2018-03-24 17:13:01
 * 功能描述:
 * 
 * 修改历史：
 * 
 * ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
 * ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
 * ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weixin.Mp.Sdk.Domain
{
    public class TemplateMessage
    {
        /// <summary>
        /// 接收者openid
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 模板跳转链接
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 跳小程序所需数据，不需跳小程序可不用传该数据
        /// </summary>
        public Miniprogram miniprogram { get; set; }

        /// <summary>
        /// 模板数据
        /// </summary>
        public Data data { get; set; }

        /// <summary>
        /// 将对象转化为Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            string s = "{\"touser\":\"" + touser + "\",\"template_id\":\"" + template_id + "\",\"url\":\"" + url +
                //"\",\"miniprogram\":{\"appid\":\"" + miniprogram.appid + "\",\"pagepath\":\"" + miniprogram.pagepath + "\"}," +
                "\",\"data\":{\"first\":{\"value\":\"" + data.first.value + "\",\"color\":\"" + data.first.color + "\"}," +
                "\"keynote1\":{\"value\":\"" + data.keynote1.value + "\",\"color\":\"" + data.keynote1.color + "\"}," +
                "\"keynote2\":{\"value\":\"" + data.keynote2.value + "\",\"color\":\"" + data.keynote2.color + "\"}," +
                "\"keynote3\":{\"value\":\"" + data.keynote3.value + "\",\"color\":\"" + data.keynote3.color + "\"}," +
                "\"remark\":{\"value\":\"" + data.remark.value + "\",\"color\":\"" + data.remark.color + "\"}}}";

            return s;
        }

    }

    public class Miniprogram {
        /// <summary>
        /// 所需跳转到的小程序appid（该小程序appid必须与发模板消息的公众号是绑定关联关系）
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar）
        /// </summary>
        public string pagepath { get; set; }
    }
    public class First
    {
        /// <summary>
        /// 模版内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string color { get; set; }
    }
    public class Keynote1
    {
        /// <summary>
        /// 模版内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string color { get; set; }
    }
    public class Keynote2
    {
        /// <summary>
        /// 模版内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string color { get; set; }
    }
    public class Keynote3
    {
        /// <summary>
        /// 模版内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string color { get; set; }
    }
    public class Keynote4
    {
        /// <summary>
        /// 模版内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string color { get; set; }
    }
    public class Remark
    {
        /// <summary>
        /// 模版内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string color { get; set; }
    }
    public class Data {
        /// <summary>
        /// 描述内容
        /// </summary>
        public First first { get; set; }
        public Keynote1 keynote1 { get; set; }
        public Keynote2 keynote2 { get; set; }
        public Keynote3 keynote3 { get; set; }
        public Keynote4 keynote4 { get; set; }
        public Remark remark { get; set; }
    }
}
