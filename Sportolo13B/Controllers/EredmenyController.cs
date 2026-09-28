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

        //put 
        [HttpPut]
        public object UpdateEredmeny([FromQuery] int id, UpdateEredmenyDto updateEredmenyDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"UPDATE `eredmeny`
                           SET `competition` = @competition,`description` = @description, `resultTime` = @resultTime, `updateTime` = @updateTime,`sportoloId` = @sportoloId
                           WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@competition",updateEredmenyDto.Competition);
            cmd.Parameters.AddWithValue("@description",updateEredmenyDto.Description);
            cmd.Parameters.AddWithValue("@resultTime",updateEredmenyDto.ResultTime);
            cmd.Parameters.AddWithValue("@updateTime",DateTime.Now);
            cmd.Parameters.AddWithValue("@sportoloId",updateEredmenyDto.SportoloId);
            cmd.Parameters.AddWithValue("@id",id);
            cmd.ExecuteNonQuery();

            connection.Close();
            return new
            {
                message = "sikeres frissites",
                result = updateEredmenyDto
            };
        }


        //torles

        [HttpDelete]
        public object DeleteEredmeny(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"DELETE FROM `eredmeny` WHERE `id` = @id";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new
            {
                message = "sikeres torles",
                result = ""
            };
        }


        //6 id alapjan egy sportolo emaile és  neve

        [HttpGet("sportolo/{id}")]
        public object GetSportoloAdatok(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            string sql = @"SELECT name, email FROM sportolo WHERE id = @id;";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            var data = cmd.ExecuteReader();

            if (data.Read())
            {
                var eredmeny = new
                {
                    Name = data.GetString("name"),
                    Email = data.GetString("email")
                };

                connection.Close();
                return eredmeny;
            }

            connection.Close();
            return NotFound("nincs ilyen");
        }


        //7
        [HttpGet("sportolo/{id}/eredmenyek")]
        public List<object> GetSportoloEredmenyek(int id)
        {
            List<object> eredmenyek = new List<object>();

            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"SELECT sportolo.name, eredmeny.competition, eredmeny.description FROM sportolo INNER JOIN eredmeny ON sportolo.id = eredmeny.sportoloId WHERE sportolo.id = @id;";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var eredmeny = new
                {
                    Name = data.GetString("name"),
                    Competition = data.GetString("competition"),
                    Description = data.GetString("description")
                };

                eredmenyek.Add(eredmeny);
            }
            connection.Close();
            return eredmenyek;
        }



        //8 mennyi eredmeny van
        [HttpGet("darab")]
        public object GetEredmenyekSzama()
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT COUNT(*) FROM eredmeny;";
            var cmd = new MySqlCommand(sql, connection);

            var darab = Convert.ToInt32(cmd.ExecuteScalar());

            connection.Close();

            return new
            {
                Darab = darab
            };
        }



        // 9 egy sportolonak hany eredmenye van

        [HttpGet("sportolo/{id}/darab")]
        public object GetSportoloEredmenyeinekSzama(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            string sql = @"SELECT COUNT(*) FROM eredmeny WHERE sportoloId = @id;";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            var darab = Convert.ToInt32(cmd.ExecuteScalar());

            connection.Close();
            return new
            {
                SportoloId = id,
                Darab = darab
            };
        }


    }
}