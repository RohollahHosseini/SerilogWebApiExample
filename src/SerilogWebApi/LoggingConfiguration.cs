using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace SerilogWebApi
{
   /// <summary>
   /// ما است Serilog این کلاس برای کانفیگ کردن 
   /// </summary>
    public class LoggingConfiguration
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigurLogger => (context, configuration) =>
        {
            #region Enriching Logger Context

            //برای ما مشخص میکنه product در چه مکانی هستیم در حالت توسعه هستیم یا 
            var env = context.HostingEnvironment;


            configuration.Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", env.ApplicationName)
            .Enrich.WithProperty("Test","Test")// my information to log
            .Enrich.WithEnvironmentName()
            .Enrich.WithSpan()// create ==>TraceId
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails();

            #endregion

            //logging to Console
            configuration.WriteTo.Console().MinimumLevel.Error();

            //Add SEQ Panel
            //get ApiKey from ==> SEQ Panel
            configuration.WriteTo.Seq(serverUrl:"http://localhost:5341",apiKey:"uDsyNV8sq2DiE8E6InC2",restrictedToMinimumLevel:
               Serilog.Events.LogEventLevel.Information);

            //logging to file => .json
            configuration.WriteTo
            .Async(sinkConfiguration=>sinkConfiguration.File(new JsonFormatter(),"logs/log.json"))
            .MinimumLevel.Error();

        };
    }
}
