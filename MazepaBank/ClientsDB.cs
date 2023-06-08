using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MazepaBank
{
    public class ClientsDB
    {
        public List<Client> clients;
        public string ActiveNumber;
        public List<MTransaction> transactions;
        public Credit credit;
        string connectionString = @"Data Source=CIA;Initial Catalog=MazepaBank;Integrated Security=True";
        public List<Jar> jars;
        public List<MTransaction> Jtrans;

        public ClientsDB()
        {
            clients = new List<Client>();
            ActiveNumber = null;
            transactions = new List<MTransaction>();
            credit = new Credit();
            jars = new List<Jar>();
            Jtrans = new List<MTransaction>();
        }

        public void DownloadFromDb()
        {
            clients.Clear();
            string query = "SELECT * FROM Clients";
            DataSet dataSet = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    dataSet = new DataSet();
                    try
                    {
                        connection.Open();
                        adapter.Fill(dataSet,"Clients");
                        
                    }
                    catch (Exception ex)
                    {

                       MessageBox.Show("Error: " + ex.Message);
                    }
                    finally
                    {

                        connection.Close();
                    }
                }
            }

            foreach(DataRow row in dataSet.Tables["Clients"].Rows)
            {
                clients.Add(new Client(row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString(), Convert.ToDouble(row[9])));
            }

            //MessageBox.Show(clients[0].ToString());

        }

        


        public void AddNewClient(string com)
        {
            
                string sqlExpression = com;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                    //MessageBox.Show("NEw data was added");
                }          
            
        }

        public void DownLoadTransactions()
        {
            transactions = new List<MTransaction>();
            if (ActiveNumber != null)
            {
                transactions.Clear();
                string query = $"SELECT * FROM TransactionHistory WHERE Number = '{ActiveNumber}'";
                DataSet dataSet = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        dataSet = new DataSet();
                        try
                        {
                            connection.Open();
                            adapter.Fill(dataSet, "TransactionHistory");

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Error: " + ex.Message);
                        }
                        finally
                        {

                            connection.Close();
                        }
                    }
                }
                foreach (DataRow row in dataSet.Tables["TransactionHistory"].Rows)
                {
                    transactions.Add(new MTransaction(row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), Convert.ToDouble(row[5])));
                }

            }
            else
            {
                MessageBox.Show("Transactions download ERROR!!!");
            }
        }



        public void DownLoadTransactionsJar()
        {
            Jtrans = new List<MTransaction>();
            if (ActiveNumber != null)
            {
                Jtrans.Clear();
                string query = $"SELECT * FROM TransactionHistory";
                DataSet dataSet = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        dataSet = new DataSet();
                        try
                        {
                            connection.Open();
                            adapter.Fill(dataSet, "TransactionHistory");

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Error: " + ex.Message);
                        }
                        finally
                        {

                            connection.Close();
                        }
                    }
                }
                foreach (DataRow row in dataSet.Tables["TransactionHistory"].Rows)
                {
                    Jtrans.Add(new MTransaction(row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), Convert.ToDouble(row[5])));
                }

            }
            else
            {
                MessageBox.Show("Transactions download ERROR!!!");
            }
        }




        public void DownLoadCredits()
        {
            if (ActiveNumber != null)
            {
                //transactions.Clear();
                string query = $"SELECT * FROM Credits WHERE Number = '{ActiveNumber}'";
                DataSet dataSet = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        dataSet = new DataSet();
                        try
                        {
                            connection.Open();
                            adapter.Fill(dataSet, "Credits");

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Error: " + ex.Message);
                        }
                        finally
                        {

                            connection.Close();
                        }
                    }
                }

                
                try
                {
                    DataRow dataRow = dataSet.Tables["Credits"].Rows[0];
                    if (dataSet.Tables["Credits"].Rows.Count > 0) credit = new Credit(dataRow[1].ToString(), Convert.ToDouble(dataRow[2]), Convert.ToDateTime(dataRow[3]), Convert.ToDouble(dataRow[4]), Convert.ToDateTime(dataRow[5]));
                    else credit = null;
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Credits download ERROR!!!");
            }
        }



        public void DownLoadJars()
        {
            jars.Clear();
            if (ActiveNumber != null)
            {
                //transactions.Clear();
                string query = $"SELECT * FROM Deposit";
                DataSet dataSet = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        dataSet = new DataSet();
                        try
                        {
                            connection.Open();
                            adapter.Fill(dataSet, "Deposit");

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Error: " + ex.Message);
                        }
                        finally
                        {

                            connection.Close();
                        }
                    }
                }
                foreach (DataRow row in dataSet.Tables["Deposit"].Rows)
                {
                    List<MTransaction> JT = new List<MTransaction>();
                    //MessageBox.Show(transactions.Count.ToString());
                    foreach (MTransaction transaction in transactions)
                    {
                        if(transaction.TName.Trim() == row[2].ToString().Trim()) JT.Add(transaction);
                    }
                    jars.Add(new Jar(row[1].ToString(), row[2].ToString(), Convert.ToDouble(row[3]), row[4].ToString(),JT));
                }

            }
            else
            {
                MessageBox.Show("Deposits download ERROR!!!");
            }
        }




        public Client ActiveClient()
        {
           foreach(Client client in clients)
            {
                if (client.Number == ActiveNumber) return client;
                
            }
           return null;
        }

        public void ChangeBalance(string telNum, double bal)
        {
            string tempB = Math.Round(bal, 2).ToString();
           
            string updateQuery = $"UPDATE Clients SET Balance = {tempB.Replace(",",".")} WHERE Number = '{telNum}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();
                        command.ExecuteNonQuery();
                        //MessageBox.Show("NEw data was added");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }


        public void ChangeBalanceJar(string telNum, double bal)
        {
            string tempB = Math.Round(bal, 2).ToString();

            string updateQuery = $"UPDATE Deposit SET Balance = {tempB.Replace(",", ".")} WHERE CreatorNumber = '{telNum}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();
                        command.ExecuteNonQuery();
                       // MessageBox.Show("NEw data was added");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        public void DelCred(string telNum)
        {
            string sql = $"DELETE FROM Credits WHERE Number = '{telNum}'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                   // MessageBox.Show("Data was deleted");

                }
            }
        }



        public void ChangeCreditBalance(string telNum, Credit credit)
        {
            string tempB = Math.Round(credit.Balance, 2).ToString();

            string updateQuery1 = $"UPDATE Credits SET CreditBalance = {tempB.Replace(",", ".")} WHERE Number = '{telNum}'";
            string updateQuery2 = $"UPDATE Credits SET LastDate = '{credit.LastDate.ToShortDateString()}' WHERE Number = '{telNum}'";
            
            
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery1, connection))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();
                        command.ExecuteNonQuery();
                        //MessageBox.Show("NEw data was added");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery2, connection))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();
                        command.ExecuteNonQuery();
                        //MessageBox.Show("NEw data was added");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        public void UpdateTrans()
        {

        }

        public void AddTrans(MTransaction rec, MTransaction send)
        {
            string tempR = Math.Round(rec.Sum, 2).ToString();
            string tempS = Math.Round(send.Sum, 2).ToString();
            string sqlExpressionR = $"INSERT INTO TransactionHistory (Number, Type, Name, Description, Summ) VALUES ('{rec.TelNum}', '{rec.Type}', '{rec.TName}', '{rec.Description}', {tempR.Replace(",", ".")})" ;
            string sqlExpressionS = $"INSERT INTO TransactionHistory (Number, Type, Name, Description, Summ) VALUES ('{send.TelNum}', '{send.Type}', '{send.TName}', '{send.Description}', {tempS.Replace(",", ".")})" ;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpressionR, connection);
                command.ExecuteNonQuery();
                //MessageBox.Show("NEw data was added");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpressionS, connection);
                command.ExecuteNonQuery();
                //MessageBox.Show("NEw data was added");
            }
        }


        public void AddTrans(MTransaction send)
        {
            
            string tempS = Math.Round(send.Sum, 2).ToString();
            
            string sqlExpressionS = $"INSERT INTO TransactionHistory (Number, Type, Name, Description, Summ) VALUES ('{send.TelNum}', '{send.Type}', '{send.TName}', '{send.Description}', {tempS.Replace(",", ".")})";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpressionS, connection);
                command.ExecuteNonQuery();
                //MessageBox.Show("NEw data was added");
            }
        }


        public void AddCredit(Credit cred)
        {
            string tempR = Math.Round(cred.Balance, 2).ToString();
            string sqlExpressionR = $"INSERT INTO Credits (Number, CreditBalance, DateBegin, CPercent, LastDate) VALUES ('{cred.Number}', {tempR.Replace(",", ".")}, '{cred.DateBegin.ToShortDateString()}', '{Convert.ToInt32(cred.Percent)}', '{cred.LastDate.ToShortDateString()}')";
           

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpressionR, connection);
                command.ExecuteNonQuery();
                //MessageBox.Show("NEw data was added");
            }

        }


        public void AddJar(Jar jar)
        {
            
            string sqlExpressionR = $"INSERT INTO Deposit (CreatorNumber, Name, Balance,Description) VALUES ('{jar.CreatorNumber}', '{jar.IDName}', 0, '{jar.Description}')";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpressionR, connection);
                command.ExecuteNonQuery();
                //MessageBox.Show("NEw data was added");
            }

        }
    }
}
