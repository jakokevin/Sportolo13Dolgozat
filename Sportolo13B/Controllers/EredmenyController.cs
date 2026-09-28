using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolo13B.Models;
using Sportolo13B.Models.DTOs;
using Sportolo13B.Models;
using Sportolo13B.Models.DTOs;

namespace Sportolo13B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=sportolo13b;";

        [HttpGet]
        public List<Eredmeny> GetEredmenyek()
        {
            List<Eredmeny> eredmenyek = new List<Eredmeny>();
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT * FROM eredmeny;";
            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var eredmeny = new Eredmeny
                {
                    Id = data.GetInt32("id"),
                    Competition = data.GetString("competition"),
                    Description = data.GetString("description"),
                    ResultTime = data.GetDateTime("resultTime"),
                    UpdateTime = data.GetDateTime("updateTime"),
                    SportoloId = data.GetInt32("sportoloId")
                };

                eredmenyek.Add(eredmeny);
            }
            connection.Close();
            return eredmenyek;
        }


        //id alapjan
        [HttpGet("byId")]
        public object GetEredmenyById(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            string sql = @"SELECT * FROM `eredmeny` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            object? data = null;
            if (datareader.Read() == true)
            {
                var eredmeny = new Eredmeny{
                    Id = datareader.GetInt32(0),
                    Competition = datareader.GetString(1),
                    Description = datareader.GetString(2),
                    ResultTime = datareader.GetDateTime(3),
                    UpdateTime = datareader.GetDateTime(4),
                    SportoloId = datareader.GetInt32(5)
                };

                data = new
                {
                    message = "sikeres lekerdezes",
                    result = eredmeny
                };
            }
            else
            {
                data = new
                {
                    message = "nincs ilyen eredmeny",
                    result = ""
                };
            }
            connection.Close();
            return data;
        }


        //post uj eredmeny

        [HttpPost]
        public object AddNewEredmeny(
            [FromBody] AddNewEredmenyDto addNewEredmenyDto){
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            string sql = @"INSERT INTO `eredmeny`
                           (`competition`,`description`,`resultTime`,`updateTime`,`sportoloId`)
                           VALUES (@competition,@description,@resultTime,@updateTime,
                            @sportoloId)";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@competition", addNewEredmenyDto.Competition);
            cmd.Parameters.AddWithValue("@description",addNewEredmenyDto.Description);
            cmd.Parameters.AddWithValue("@resultTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime",DateTime.Now);
            cmd.Parameters.AddWithValue("@sportoloId",addNewEredmenyDto.SportoloId);
            cmd.ExecuteNonQuery();
            connection.Close();
            return new
            {
                message = "sikeres felvétel",
                result = addNewEredmenyDto
            };
        }




    }
}