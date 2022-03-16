using Microsoft.Data.SqlClient;
using System.Data;
using CursProper.Models;
namespace CursProper
{
    public class SqlHelper
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS01;Database=newnewdb;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True";

        public async Task<DataTable> GetData(int currentPage, int pageSize, string db,string id_name)
        {
            DataTable dt = new DataTable();
            //int offset = (currentPage - 1) * pageSize;
           // string sqlExpression1 = "select * from @db order by @id_name offset @offset rows fetch next @pageSize rows only";
            string sqlExpression = "SELECT * FROM " + db + " ORDER BY " + id_name + " OFFSET " + ((currentPage - 1) * pageSize).ToString() + " ROWS FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlExpression, conn);
               /* command.Parameters.Add("@db", SqlDbType.Structured).Value = db;
                command.Parameters.Add("@id_name", SqlDbType.VarChar).Value = id_name;
                command.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
                command.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;*/
                await conn.OpenAsync();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            return dt;
        }
        public async Task<DataTable> GetData( string db)
        {
            DataTable dt = new DataTable();
            //int offset = (currentPage - 1) * pageSize;
            // string sqlExpression1 = "select * from @db order by @id_name offset @offset rows fetch next @pageSize rows only";
            string sqlExpression = "SELECT * FROM " + db;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlExpression, conn);
                /* command.Parameters.Add("@db", SqlDbType.Structured).Value = db;
                 command.Parameters.Add("@id_name", SqlDbType.VarChar).Value = id_name;
                 command.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
                 command.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;*/
                await conn.OpenAsync();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            return dt;
        }
        public async Task<int> GetCount(string db)
        {
            string sqlExpression = "SELECT COUNT(*) FROM " + db;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, conn);
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }
        public async Task<int> GetCount(string db, string idName,int id)
        {
            string sqlExpression = "SELECT COUNT(*) FROM " + db + " where " + idName + " = " + id.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, conn);
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }
        public async Task<DataTable> FindByIdDb(int? id,string dbName,string refId)
        {
            string sqlExpression = "SELECT * FROM "+dbName+" WHERE "+refId+" = " + id.ToString();
            DataTable db = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(sqlExpression, conn);
                adapter.Fill(db);
            }
            return db;
        }
        public async Task<DataTable> FindByReferences(string dbName,string refId, string refDb, string refIdName, int? idTarget)
        {
            string sqlExpression = "select * from "+ dbName + " where "+refId+" in (select "+ refId+" from "+refDb+" where "+refIdName+" = "+idTarget.ToString()+")";
            DataTable db = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(sqlExpression, conn);
                adapter.Fill(db);
            }
            return db;
        }
        public async Task DeleteFromDb(string dbName, string idName, int? idTarget)
        {
           // string sqlExpression1 = "delete from @dbName where @idName = @idTarget";
            string sqlExpression = "delete from " + dbName + " where " + idName + " = " + idTarget.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(sqlExpression, conn);
            /*    sqlCommand.Parameters.AddWithValue("@dbName",dbName);
                sqlCommand.Parameters.AddWithValue("@idName",idName);
                sqlCommand.Parameters["@idTarget"].Value = idTarget.ToString();*/
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
        public async Task DeleteFromDb(string dbName,  int idTarget1, int idTarget2)
        {
            // string sqlExpression1 = "delete from @dbName where @idName = @idTarget";
            string sqlExpression = "delete from " + dbName + " where (OrganisationID = @idTarget1 and DatabaseID = @idTarget2)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(sqlExpression, conn);
                sqlCommand.Parameters.AddWithValue("@idTarget1", idTarget1);
                sqlCommand.Parameters.AddWithValue("@idTarget2", idTarget2);
                /*    sqlCommand.Parameters.AddWithValue("@dbName",dbName);
                    sqlCommand.Parameters.AddWithValue("@idName",idName);
                    sqlCommand.Parameters["@idTarget"].Value = idTarget.ToString();*/
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task InsertIntoCluster(string dbName,int db_id,int selected_id,string db_id_name, string selected_id_name)
        {
            string sqlExpression = "insert into " + dbName + "("+db_id_name+","+selected_id_name+") values ("+db_id.ToString()+","+selected_id.ToString()+")";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(sqlExpression, conn);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
        public async Task<int> MaxId(string coulumnName,string db)
        {
            string sqlExpression = "select max(@columnName) from " + db;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(sqlExpression, conn);
                sqlCommand.Parameters.Add("@columnName",SqlDbType.VarChar).Value = coulumnName;
                return Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
            }
        }
        public async Task InsertLitReference(string article, string source, string year, string volume, string number, string pages)
        {
            string sqlExpression = "insert into LitReferences (ReferenceId,Article,Source,Year,Volume,Number,Pages) values (@rfId,@article,@source,@year,@volume,@number,@pages)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string sql = "select max(ReferenceId) from LitReferences";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                count++;
                SqlCommand sqlCommand = new SqlCommand(sqlExpression, conn);
                sqlCommand.Parameters.AddWithValue("@rfId", count);
                sqlCommand.Parameters.Add("@article", SqlDbType.VarChar).Value=article;
                sqlCommand.Parameters.Add("@source", SqlDbType.VarChar).Value= source;
                sqlCommand.Parameters.Add("@year",SqlDbType.VarChar).Value= year;
                sqlCommand.Parameters.Add("@volume", SqlDbType.VarChar).Value = volume;
                sqlCommand.Parameters.Add("@number", SqlDbType.VarChar).Value = number;
                sqlCommand.Parameters.Add("@pages",SqlDbType.VarChar).Value=pages;
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
        public async Task InsertDatabase(string name_rus, string name_eng,string phone,string fax,string email,string url,string comment_rus)
        {
            string sqlExpression = "insert into Databases(DatabaseID,Name_rus,Name_eng,Phone,Fax,Email,URL,Comments_rus) values(@id,@name_rus,@name_eng,@phone,@fax,@email,@url,@comment)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string sql = "select max(DatabaseID) from Databases";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                count++;
                SqlCommand sqlCommand = new SqlCommand(sqlExpression, conn);
                sqlCommand.Parameters.AddWithValue("@id",count);
                sqlCommand.Parameters.AddWithValue("@name_rus", name_rus);
                sqlCommand.Parameters.AddWithValue("@name_eng", name_eng);
                sqlCommand.Parameters.AddWithValue("@phone",phone);
                sqlCommand.Parameters.AddWithValue("@fax",fax);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlCommand.Parameters.AddWithValue("@url",url);
                sqlCommand.Parameters.AddWithValue("@comment", comment_rus);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
        public async Task InsertAuthor(string name)
        {
            string sqlExpression = "insert into AuthorsInfo(AuthorID,AuthorName) values(@id,@name)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string sql = "select max(AuthorID) from AuthorsInfo";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                count++;
                SqlCommand sqlCommand = new SqlCommand(sqlExpression, conn);
                sqlCommand.Parameters.AddWithValue("@id", count);
                sqlCommand.Parameters.AddWithValue("@name", name);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
        public async Task InsertOrg(string name_rus, string name_eng, string abbreviationRus, string abbreviationEng, string adressRus, string adressEng, int database_id,int country_id)
        {
            string sqlExpression = "insert into OrganisationsInfo(OrganisationID, CountryID, Name_rus, Name_eng, Abbreviation_rus, Abbreviation_eng) values(@OrganisationID,@CountryID,@Name_rus,@Name_eng,@Abbreviation_rus,@Abbreviation_eng)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string sql = "select max(OrganisationID) from OrganisationsInfo";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                count++;
                SqlCommand sqlCommand = new SqlCommand(sqlExpression, conn);
                sqlCommand.Parameters.AddWithValue("@OrganisationID", count);
                sqlCommand.Parameters.AddWithValue("@CountryID", country_id);
                sqlCommand.Parameters.AddWithValue("@Name_rus", name_rus);
                sqlCommand.Parameters.AddWithValue("@Name_eng", name_eng);
                sqlCommand.Parameters.AddWithValue("@Abbreviation_rus", abbreviationRus);
                sqlCommand.Parameters.AddWithValue("@Abbreviation_eng", abbreviationEng);
                await sqlCommand.ExecuteNonQueryAsync();
                sqlExpression = "insert into OrganisationsInfo_Databases(DatabaseID,OrganisationID,Address_eng,Address_rus) Values (@db_id,@org_id,@adr_eng,@adr_rus)";
                SqlCommand command = new SqlCommand(sqlExpression, conn);
                command.Parameters.AddWithValue("@db_id", database_id);
                command.Parameters.AddWithValue("@org_id", count);
                command.Parameters.AddWithValue("@adr_rus", adressRus);
                command.Parameters.AddWithValue("@adr_eng", adressEng);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
