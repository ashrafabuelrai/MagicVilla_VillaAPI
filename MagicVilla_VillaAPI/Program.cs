


using MagicVilla_VillaAPI.Logger;

namespace MagicVilla_VillaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region LoggerInFile
            /*Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .WriteTo.File("log/villaLogs.txt",rollingInterval:RollingInterval.Day).CreateLogger();
            builder.Host.UseSerilog();*/
            #endregion
            builder.Services.AddControllers(options => {
                //options.ReturnHttpNotAcceptable = true;
                }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<ILogging, Logging>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
