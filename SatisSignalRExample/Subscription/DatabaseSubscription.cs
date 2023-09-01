using Microsoft.AspNetCore.SignalR;
using SatisSignalRExample.Hubs;
using SatisSignalRExample.Models;
using TableDependency.SqlClient;

namespace SatisSignalRExample.Subscription
{
    public class DatabaseSubscription<T> : IDataBaseSubscription where T : class,new()
    {
        IConfiguration _configuration;

        IHubContext<SatisHub> _hubContext;

        public DatabaseSubscription(IConfiguration configuration, IHubContext<SatisHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext; 
        }

        SqlTableDependency<T> _sqlTableDependency; 

        public void Configure(string tableName)
        {
           _sqlTableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("SqlConnection"),tableName);

            _sqlTableDependency.OnChanged += async (o, e) =>
            {
               await _hubContext.Clients.All.SendAsync("receiveMessage", "Merhaba");

                SatisSignalRdbContext satisSignalRdbContext = new SatisSignalRdbContext();

                var data = (from personel in satisSignalRdbContext.Personellers
                            join satis in satisSignalRdbContext.Satislars
                            on personel.Id equals satis.PersonelId
                            select new { personel, satis }).ToList();

                List<object> dataList = new List<object>();

                var personelNames = data.Select(d => d.personel.Name).Distinct();

                foreach (var personel in personelNames)
                {

                }


            };

            _sqlTableDependency.OnError += (o, e) =>
            {

            };


            _sqlTableDependency.Start();
        }

        ~DatabaseSubscription()   // nesne remove edilmeden önce hadi selametle demeden once çalıştırılır sonra nesne remove edilir.Buna deconstructure nedir(Constructure-- )
        {
            _sqlTableDependency.Stop();
        }
    }
}
