using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Launch_Connection.Services;

public class HttpRequest
{
    public static async Task<JObject> Post_Json(string url, object requestObject, object? headers = null) //Post请求返回Json
    {
        HttpClient httpClient = new HttpClient(); //初始化httpclient
        var stopwatch = new Stopwatch(); //初始化计时器
        stopwatch.Start(); //计时开始
        var requestContent = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json"); //初始化请求内容
        if (headers != null) //添加请求头
        {
            var headersDictionary = ConvertToDictionary(headers);
            foreach (var header in headersDictionary)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
        try
        {
            var response = await httpClient.PostAsync(url, requestContent); //发送请求

            stopwatch.Stop(); //计时结束
            TimeSpan elapsedTime = stopwatch.Elapsed; //获取执行时间
            double elapsedTimeInSeconds = elapsedTime.TotalSeconds; //转换为秒
            string time = elapsedTimeInSeconds.ToString("0.00"); // 格式化执行时间为两位小数

            var headersDict = response.Headers.ToDictionary(h => h.Key, h => h.Value);
            string headersJson = JsonConvert.SerializeObject(headersDict, Formatting.Indented); //获取响应头
            var data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()); //获取响应内容
            if (response.IsSuccessStatusCode) return JObject.Parse(JsonConvert.SerializeObject(new { status = true, online = true, time = time, data = data, headers = headersJson })); //构造响应成功内容
            else return JObject.Parse(JsonConvert.SerializeObject(new { status = false, online = true, time = time, data = data, headers = headersJson })); //构造响应失败内容
        }
        catch (Exception error)
        {
            stopwatch.Stop(); //计时结束
            return JObject.Parse(JsonConvert.SerializeObject(new { status = false, online = false, data = error, headers = "null" }));
        }
    }
    public static async Task<JObject> Get_Json(string url) //Get请求返回Json
    {
        HttpClient httpClient = new HttpClient(); //初始化httpclient
        var stopwatch = new Stopwatch(); //初始化计时器
        stopwatch.Start(); //计时开始

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);

            stopwatch.Stop(); //计时结束
            TimeSpan elapsedTime = stopwatch.Elapsed; //获取执行时间并转换为秒
            double elapsedTimeInSeconds = elapsedTime.TotalSeconds;
            string time = elapsedTimeInSeconds.ToString("0.00");

            var headersDict = response.Headers.ToDictionary(h => h.Key, h => h.Value);
            string headersJson = JsonConvert.SerializeObject(headersDict, Formatting.Indented); //获取响应头
            var data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()); //获取响应内容
            if (response.IsSuccessStatusCode) return JObject.Parse(JsonConvert.SerializeObject(new { status = true, online = true, time = time, data = data, headers = headersJson })); //构造响应成功内容
            else return JObject.Parse(JsonConvert.SerializeObject(new { status = false, online = true, time = time, data = data, headers = headersJson })); //构造响应失败内容
        }
        catch (Exception error)
        {
            stopwatch.Stop(); //计时结束
            return JObject.Parse(JsonConvert.SerializeObject(new { status = false, online = false, data = error, headers = "null" }));
        }
    }
    public static async Task<string> Get(string url) //Get请求
    {
        HttpClient httpClient = new HttpClient(); //初始化httpclient

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url); //发送请求

            var data = await response.Content.ReadAsStringAsync(); //获取响应内容
            if (response.IsSuccessStatusCode) return data; //构造响应成功内容
            else return data; //构造响应失败内容
        }
        catch (Exception error)
        {
            return error.ToString();
        }
    }
    public static Dictionary<string, string> ConvertToDictionary(object obj) //将对象转换为字典
    {
        var dictionary = new Dictionary<string, string>(); //初始化字典
        var properties = obj.GetType().GetProperties(); //获取对象的属性
        foreach (var property in properties) //遍历属性
        {
            var value = property.GetValue(obj)?.ToString(); //获取属性的值
            if (!string.IsNullOrEmpty(value)) //如果属性的值不为空
            {
                dictionary.Add(property.Name, value); //添加到字典
            }
        }
        return dictionary; //返回字典
    }
}