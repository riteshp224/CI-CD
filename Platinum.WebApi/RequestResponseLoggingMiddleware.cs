using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly IHostingEnvironment _hostingEnvironment;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next"></param>
    /// <param name="memoryCache"></param>
    public RequestResponseLoggingMiddleware(RequestDelegate next, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
    {
        _next = next;
        _cache = memoryCache;
        _hostingEnvironment = hostingEnvironment;
    }

    /// <summary>
    /// Invoke on every request response
    /// </summary>
    /// <param name="context"></param>
    /// <param name="settingService"></param>
    public async Task Invoke(HttpContext context)
    {
        //var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "ReqResLog", Path.GetFileName(DateTime.Now.ToString("dd_MM_yyyy") + ".txt"));
        var exfilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "ReqResLog", "Exception_" + Path.GetFileName(DateTime.Now.ToString("dd_MM_yyyy") + ".txt"));
        var requestStartTime = DateTime.Now;
        using MemoryStream requestBodyStream = new MemoryStream();
        using MemoryStream responseBodyStream = new MemoryStream();
        Stream originalRequestBody = context.Request.Body;
        Stream originalResponseBody = context.Response.Body;
        long LoggedInUserId = 0;
        //long CompanyId = 0;
        //long RoleId = 0;
        try
        {
            var directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ReqResLog");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-7))
                {
                    fi.Delete();
                }
            }

            //if (!File.Exists(filePath))
            //{
            //    var myFile = File.Create(filePath);
            //    myFile.Close();
            //}
            string headervalue = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(headervalue))
            {
                headervalue = Regex.Replace(headervalue, "Bearer ", "", RegexOptions.IgnoreCase);

                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadToken(headervalue) as JwtSecurityToken;
                if (token != null)
                {
                    if (token.ValidTo < DateTime.UtcNow.AddMinutes(1))
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    var claims = token.Claims.ToList();
                    LoggedInUserId = Convert.ToInt64(claims.First(claim => claim.Type == "LoggedInUserId").Value);
                    //CompanyId = Convert.ToInt64(claims.First(claim => claim.Type == "CompanyId").Value);
                    //RoleId = Convert.ToInt64(claims.First(claim => claim.Type == "RoleId").Value);
                    if (!string.IsNullOrEmpty(context.Request.ContentType))
                    {
                        if (context.Request.ContentType.Contains("multipart/form-data"))
                        {
                            var formDictionary = new Dictionary<string, StringValues>();
                            var form = context.Request.Form;
                            bool my_user_id_added = false;
                            bool my_company_id_added = false;
                            bool my_role_id_added = false;
                            foreach (var key in form.Keys)
                            {
                                form.TryGetValue(key, out StringValues formValues);

                                // Change my_user_id value
                                if (key == "LoggedInUserId")
                                {
                                    formValues = new string[] { LoggedInUserId.ToString() };
                                    my_user_id_added = true;
                                }

                                // Change my_user_id value
                                //if (key == "my_company_id")
                                //{
                                //    formValues = new string[] { CompanyId.ToString() };
                                //    my_company_id_added = true;
                                //}

                                //// Change my_user_id value
                                //if (key == "my_role_id")
                                //{
                                //    formValues = new string[] { RoleId.ToString() };
                                //    my_role_id_added = true;
                                //}

                                formDictionary.Add(key, formValues);
                            }

                            if (!my_user_id_added)
                            {
                                formDictionary.Add("LoggedInUserId", LoggedInUserId.ToString());
                            }

                            //if (!my_company_id_added)
                            //{
                            //    formDictionary.Add("my_company_id", CompanyId.ToString());
                            //}

                            //if (!my_role_id_added)
                            //{
                            //    formDictionary.Add("my_role_id", RoleId.ToString());
                            //}

                            FormCollection formCollection = new FormCollection(formDictionary, form.Files);
                            context.Request.Form = formCollection;
                        }
                        else
                        {
                            string originalContent;
                            originalContent = await new StreamReader(originalRequestBody).ReadToEndAsync();

                            var dataSource = JsonConvert.DeserializeObject<dynamic>(originalContent);
                            if (dataSource != null)
                            {
                                dataSource.LoggedInUserId = LoggedInUserId;
                                //dataSource.my_company_id = CompanyId;
                                //dataSource.my_role_id = RoleId;
                                dataSource.created_by = LoggedInUserId;
                                dataSource.modified_by = LoggedInUserId;
                                var json = JsonConvert.SerializeObject(dataSource);
                                //replace request stream to downstream handlers
                                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                                originalRequestBody = await requestContent.ReadAsStreamAsync();//modified stream
                                context.Request.Body = originalRequestBody;
                            }
                        }
                    }
                    else
                    {
                        string originalContent;
                        originalContent = await new StreamReader(originalRequestBody).ReadToEndAsync();

                        var dataSource = JsonConvert.DeserializeObject<dynamic>(originalContent);
                        if (dataSource != null)
                        {
                            dataSource.my_user_id = LoggedInUserId;
                            //dataSource.my_company_id = CompanyId;
                            //dataSource.my_role_id = RoleId;
                            dataSource.created_by = LoggedInUserId;
                            dataSource.modified_by = LoggedInUserId;
                            var json = JsonConvert.SerializeObject(dataSource);
                            //replace request stream to downstream handlers
                            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                            originalRequestBody = await requestContent.ReadAsStreamAsync();//modified stream
                            context.Request.Body = originalRequestBody;
                        }
                    }
                }
            }

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            string requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            string responseBody = "";


            context.Response.Body = responseBodyStream;

            Stopwatch watch = Stopwatch.StartNew();
            await _next(context);
            watch.Stop();

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            responseBody = new StreamReader(responseBodyStream).ReadToEnd();
            //AuditLogger.LogToAudit(context.Request.Host.Host,
            //    context.Request.Path, context.Request.QueryString.ToString(), context.Connection.RemoteIpAddress.MapToIPv4().ToString(),
            //    string.Join(",", context.Request.Headers.Select(he => he.Key + ":[" + he.Value + "]").ToList()),
            //    requestBodyText, responseBody, DateTime.Now, watch.ElapsedMilliseconds);

            //if (!context.Request.Path.Value.ToString().ToLower().Contains("swagger") && !context.Request.Path.Value.ToString().ToLower().Contains("hangfire"))
            //{
            //    using StreamWriter sw = File.AppendText(filePath);
            //    sw.WriteLine("");
            //    sw.WriteLine("--------------------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ----------------------------------");
            //    sw.WriteLine("Requested URL: " + context.Request.Path.Value);
            //    sw.WriteLine("Total Duration: " + ((DateTime.Now - requestStartTime).TotalSeconds));
            //    sw.WriteLine("Request Body: " + requestBodyText);
            //    sw.WriteLine("Status Code: " + context.Response.StatusCode);
            //    sw.WriteLine("Response: " + responseBody);
            //}
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            await responseBodyStream.CopyToAsync(originalResponseBody);
        }
        catch (Exception ex)
        {
            if (!File.Exists(exfilePath))
            {
                var myFile = File.Create(exfilePath);
                myFile.Close();
            }
            //ExceptionLogger.LogToDatabse(ex);
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Unhandled Error occured, the error has been logged and the persons concerned are notified!! Please, try again in a while.");
            originalResponseBody.Write(data, 0, data.Length);

            using StreamWriter sw = File.AppendText(exfilePath);
            sw.WriteLine("");
            sw.WriteLine("--------------------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ----------------------------------");
            sw.WriteLine("Requested URL: " + context.Request.Path.Value);
            sw.WriteLine("Exception: " + ex.Message);
            sw.WriteLine("Exception: " + ex.InnerException);
            if (ex.InnerException != null)
            {
                sw.WriteLine("Exception: " + ex.InnerException.InnerException);
            }
        }
        finally
        {
            context.Request.Body = originalRequestBody;
            context.Response.Body = originalResponseBody;
        }
    }
}