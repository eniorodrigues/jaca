﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Common;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.Sql;
using System.Runtime.InteropServices;
using System.Configuration;
using Microsoft.Office.Interop.Excel;
using ExcelIt = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Drawing;
using testeCampos;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using OfficeOpenXml;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Collections;

namespace testeExcel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string path;
        public static string excelConnectionString;
        public string[] files;
        public string conexao;
        public string baseDeDados;
        public string tabela;
        public string caminho;
        public string directoryPath;
        private static Excel.Application MyApp = null;
        public List<string> filesAdionado = new List<string>();
        public List<string> colunas = new List<string>();
        public List<string> colunasCreate = new List<string>();
        public string tipoArquivo;
        Stream myStream = null;
        string nomeSheet;
        StringBuilder camposDataGrid = new StringBuilder();
        public List<String> itemsDataGrid = new List<String>();
        public SqlConnection conn = null;
        public bool checado;
        String databaseName;

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void buttonAbrir_Click(object sender, EventArgs e)
        {
           // SqlConnection conn = new SqlConnection(@"Data Source=BRCAENRODRIGUES\SQLEXPRESS; Initial Catalog=EKA_2018_JU; Integrated Security=True");
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "Csv files (*.csv*)|*.csv*|Excel files (*.xls*)|*.xls*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            caminho = openFileDialog1.FileName;
                            directoryPath = Path.GetDirectoryName(openFileDialog1.FileName);
                            files = (openFileDialog1.SafeFileNames);
                            carregaLinhas();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void carregaLinhas()
        {
            lblEndereço.Text = caminho;
            MyApp = new Excel.Application();
            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + caminho + ";Extended Properties=Excel 12.0;";
            MyApp.Workbooks.Add("");
            MyApp.Workbooks.Add(caminho);
            SqlTransaction trAx = null;

            for (int i = 1; i <= MyApp.Workbooks[2].Worksheets.Count; i++)
            {
                cmbPlanilha.Items.Add(MyApp.Workbooks[2].Worksheets[i].Name);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             // conn = new SqlConnection(@"Data Source=BRCAENRODRIGUES\SQLEXPRESS; Initial Catalog=EKA_2018_JU; Integrated Security=True");
        }

 
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {   
          
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
         
        private void button1_Click_1(object sender, EventArgs e)
        {

        }
        
        public void Compras()
        {
 
            //  string filePath = caminho;
            string filePath = caminho; 
            // Abrindo, modificando meu arquivo e salvando
            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DateTime hoje = DateTime.Now;
            int linha =1;
            int produtozerado = 0;
            bool produtoVazio=false;
            string tabela = "D_Compras";

            package.Save();
            package.Dispose();

            // Abrindo meu arquivo para varrer o conteudo da primeira planilha e exibir na minha página
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            StringBuilder conteudo = new StringBuilder();
            SqlCommand cmd = conn.CreateCommand();

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
            {
                produtoVazio = false;

                for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    // ultima coluna
                    if (j == workSheet.Dimension.End.Column)
                    {
                         conteudo.Append(workSheet.Cells[i, j].Value == null ? "''," : " '" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "' , '" + linha + "', ");
                        conteudo.Append(" " + pegarID("D_Compras") + " ");
                    }
                    else
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
                        {
                            DateTimeFormat = { YearMonthPattern = "yyyy-mm-dd" }
                        };
                        if (j == 1)
                        {
                            conteudo.Append(" INSERT INTO[dbo].[D_Compras] " +
                                               "([Cmp_Pro_ID] " +
                                               ",[Cmp_Cod_Divisao]" +
                                               ",[Cmp_For_ID]" +
                                               ",[Cmp_Lanc_Cont]" +
                                               ",[Cmp_Fat_Coml]" +
                                               ",[Cmp_BL_DT]" +
                                               ",[Cmp_DI_ID]" +
                                               ",[Cmp_DI_DT_Emissao]" +
                                               ",[Cmp_NF_Entrada]" +
                                               ",[Cmp_NF_Serie]" +
                                               ",[Cmp_NF_DT]" +
                                               ",[Cmp_CFOP]" +
                                               ",[Cmp_DI_DT_Vencimento]" +
                                               ",[Cmp_DI_Dias]" +
                                               ",[Cmp_Qtde]" +
                                               ",[Cmp_Valor_Fob]" +
                                               ",[Cmp_Cod_Moeda]" +
                                               ",[Cmp_Vl_Frete_Moeda]" +
                                               ",[Cmp_VL_Seguro_Moeda]" +
                                               ",[Cmp_Cod_Moeda_Frete]" +
                                               ",[Cmp_Cod_Moeda_Seguro]" +
                                               ",[Cmp_Imposto_Import]" +
                                               ",[Cmp_ICMS]" +
                                               ",[Cmp_PIS]" +
                                               ",[Cmp_COFINS]" +
                                               ",[Cmp_For_id_Frete]" +
                                               ",[Cmp_For_id_Seguro]" +
                                               ",[Cmp_Incoterm] " +
                                               ",[Lin_Origem_ID] " +
                                               ",[Arq_Origem_ID]) " +
                                               " VALUES ( ");
                            if (workSheet.Cells[i, j].Value == null)
                            {
                                conteudo.Replace("[D_Compras]", "[D_Compras_Inconsistencias]");
                                conteudo.Append(" '', ");
                            }
                            else
                            {
                                conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                        }
                        else if (( j == 22 || j == 23 || j == 24 || j == 25)
                            && (workSheet.Cells[i, j].Value == null))
                        {
                            if(workSheet.Cells[i, j].Value == null)
                            {
                           //     conteudo.Replace("[D_Compras]", "[D_Compras_Inconsistencias]");
                                conteudo.Append(" " + 0 + ", ");
                            }
                            else if(workSheet.Cells[i, j].Value == "")
                            {
                            //    conteudo.Replace("[D_Compras]", "[D_Compras_Inconsistencias]");
                                conteudo.Append(" " + 0 + ", ");
                            }
                        }
                        else if (( j == 15 || j == 16)
                           && (workSheet.Cells[i, j].Value == null))
                        {
                            if (workSheet.Cells[i, j].Value == null)
                            {
                                conteudo.Replace("[D_Compras]", "[D_Compras_Inconsistencias]");
                                conteudo.Append(" '', ");
                                MessageBox.Show("5");
                            }
                            else if (workSheet.Cells[i, j].Value == "")
                            {
                                conteudo.Replace("[D_Compras]", "[D_Compras_Inconsistencias]");
                                conteudo.Append(" '', ");
                                MessageBox.Show("7");
                            }
                        }
                        else if ((j == 6 || j == 8 || j == 11 || j == 13) && workSheet.Cells[i, j].Value != null && workSheet.Cells[i, j].Value != "" && workSheet.Cells[i, j].Value.GetType().Name.ToString() == "DateTime")
                        {
                            conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString() + "', ");
                        }
                        else if ((j == 6 || j == 8 || j == 11 || j == 13) && workSheet.Cells[i, j].Value != null && workSheet.Cells[i, j].Value != "" && workSheet.Cells[i, j].Value.GetType().Name.ToString() == "Double")
                        {
                            DateTime dt = DateTime.FromOADate(Convert.ToInt64(workSheet.Cells[i, j].Value));
                            conteudo.Append("'" + dt + "', ");
                        }
                        else if ((j == 6 || j == 8 || j == 11 || j == 13) && workSheet.Cells[i, j].Value != null)
                        {
                            DateTime dt = DateTime.ParseExact(workSheet.Cells[i, j].Value.ToString(), "dd/MM/yyyy", null);
                            conteudo.Append("'" + dt + "', ");
                        }
 
                        //caso o que não é numero esteja em branco colocar texto branco
                        else if ((j == 23 || j == 24 || j == 25 || j == 19 || j == 18 || j == 15 || j == 16 || j == 22) && (workSheet.Cells[i, j].Value == ""))
                        {
                            conteudo.Append(" " + 0 + ", ");
                        }
                        //caso número seja nula colocar zero
                        else if ((j == 23 || j == 24 || j == 25 || j == 19 || j == 18 || j == 15 || j == 16 || j == 22) && (workSheet.Cells[i, j].Value == ""))
                        {
                            conteudo.Append(" " + 0 + ", ");
                        }
                        //caso número tirar  aspas simples
                        else if ((j == 23 || j == 24 || j == 25 || j == 19 || j == 18 || j == 15 || j == 16 || j == 22) && (workSheet.Cells[i, j].Value != null))
                        {
                            conteudo.Append("" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + ", ");
                        }

                        else if (workSheet.Cells[i, j].Value != null || workSheet.Cells[i, j].Value != "")
                        {
                            if ((workSheet.Cells[i, j].Value == null ? " NULL " : workSheet.Cells[i, j].Value.GetType().Name.ToString()) == "DateTime")
                            {
                                workSheet.Cells[i, j].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern;
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                            else
                            {
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                        }
                    }
                }

                if (i == workSheet.Dimension.End.Row)
                {
                    conteudo.Append(" ) ");
                }
                else
                {
                    conteudo.Append(")");
                    conteudo.Append(Environment.NewLine);
                }
                // Clipboard.SetText(conteudo.ToString());
                if (conn.State.ToString() == "Closed")
                {
                    conn.Open();
                }
                linha = linha + 1;
                cmd.CommandText = conteudo.ToString();
                SqlTransaction trE = null;
                trE = conn.BeginTransaction();
                cmd.Transaction = trE;
                cmd.ExecuteNonQuery();
                trE.Commit();
                conteudo.Clear();
            }
  
            package.Dispose();

            SqlCommand cmdArquivoCarregado = conn.CreateCommand();
            cmdArquivoCarregado.CommandText =
              "declare @tabela varchar(max) = 'D_Compras';" +
                " if (select count(arq_id) from S_ArquivoCarregado where Arq_Tabela = @tabela) = 0" +
                " insert into S_ArquivoCarregado" +
                " (Arq_ID, Arq_Nome, Arq_Tabela, Arq_Mensagem, Arq_DataCarga, Arq_Quantidade, Arq_Login)" +
                " values(1, '\\Pasta\\D_Compras.txt', @tabela, 'Carga efetuada com sucesso.'," +
                " GETDATE(), '2', SUSER_NAME())" +
                " else" +
                " insert into S_ArquivoCarregado" +
                " (Arq_ID, Arq_Nome, Arq_Tabela, Arq_Mensagem, Arq_DataCarga, Arq_Quantidade, Arq_Login)" +
                " values(isnull((select max(arq_id) from S_ArquivoCarregado where Arq_Tabela = @tabela),0)+1, '\\Pasta\\Vendas.txt', @tabela, 'Carga efetuada com sucesso.'," +
                " GETDATE(), " + linha.ToString() + ", SUSER_NAME())";
             
            SqlTransaction trA = null;
            trA = conn.BeginTransaction();
            cmdArquivoCarregado.Transaction = trA;
            cmdArquivoCarregado.ExecuteNonQuery();
            trA.Commit();

            conn.Close();

            MessageBox.Show("Carregamento de " + linha.ToString() + " linhas concluída");
        }

        public int pegarID(string tabela)
        {
            int arqId = 0;

            //            SqlConnection conn = new SqlConnection(@"Data Source=BRCAENRODRIGUES\SQLEXPRESS; Initial Catalog=EKA_2018_JU; Integrated Security=True");
            string oString = "select max(Arq_ID) from S_ArquivoCarregado where Arq_Tabela = @tabela";
                SqlCommand oCmd = new SqlCommand(oString, conn);
                oCmd.Parameters.AddWithValue("@tabela", tabela);

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        arqId = Int16.Parse(oReader[0].ToString());
                    }
                    conn.Close();
                    }
                    return arqId +1;
                }

        public void Vendas()
        {
            string filePath = caminho ;
            // Abrindo, modificando meu arquivo e salvando
            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DateTime hoje = DateTime.Now;
            int linha = 1;
            int produtozerado = 0;
            bool produtoVazio = false;
            string tabela = "D_Vendas_Itens";

            package.Save();
            package.Dispose();

            // Abrindo meu arquivo para varrer o conteudo da primeira planilha e exibir na minha página
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            StringBuilder conteudo = new StringBuilder();
            SqlCommand cmd = conn.CreateCommand();
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
            {
                for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    // ultima coluna
                    if (j == workSheet.Dimension.End.Column)
                    {
                        conteudo.Append(workSheet.Cells[i, j].Value == null ? " 0, '" + linha + "', " : " " + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + " , '" + linha + "', ");
                        conteudo.Append(" " + pegarID("D_Compras") + " ");
                    }
                    else
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
                        {
                            DateTimeFormat = { YearMonthPattern = "yyyy-mm-dd" }
                        };
                        if (j == 1)
                        {
                            conteudo.Append(" INSERT INTO[dbo].[" + tabela + "] " +
                                              "([Vnd_Cli_ID] " +
                                              ",[Vnd_NF_ID]" +
                                              ",[Vnd_NF_Serie]" +
                                              ",[Vnd_Cod_Divisao]" +
                                              ",[Vnd_CFOP]" +
                                              ",[Vnd_Dt_Emissao]" +
                                              ",[Vnd_DT_Vencimento]" +
                                              ",[Vnd_Dias]" +
                                              ",[Vnd_Item]" +
                                              ",[Vnd_Pro_id]" +
                                              ",[Vnd_Qtde]" +
                                              ",[Vnd_Vl_Nota]" +
                                              ",[Vnd_Desconto]" +
                                              ",[Vnd_ICMS]" +
                                              ",[Vnd_PIS]" +
                                              ",[Vnd_COFINS]" +
                                              ",[Vnd_ISS]" +
                                              ",[Vnd_Comissao]" +
                                              ",[Vnd_Frete]" +
                                              ",[Vnd_Seguro]" +
                                              ",[Vnd_Dt_Embarque]" +
                                              ",[Vnd_Cod_Moeda]" +
                                              ",[Vnd_Vl_Moeda]" +
                                              ",[Vnd_Custo] " +
                                              ",[Vnd_CNPJ] " +
                                              ",[Lin_Origem_ID] " +
                                              ",[Arq_Origem_ID]) " +
                                              " VALUES ( ");

                            if (workSheet.Cells[i, j].Value == null)
                            {
                                conteudo.Replace("D_Vendas_Itens", "A_Vendas_Inconsistencias");
                                conteudo.Append(" '', ");
                            }
                            else
                            {
                                conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                        }
                        else if ((j == 1 || j == 2 || j == 3 || j == 5 || j == 10 )
                            && (workSheet.Cells[i, j].Value == null))
                        {
                            if (workSheet.Cells[i, j].Value == null)
                            {
                                conteudo.Replace("D_Vendas_Itens", "A_Vendas_Inconsistencias");
                                conteudo.Append(" '', ");
                            }
                            else if (workSheet.Cells[i, j].Value == "")
                            {
                                conteudo.Replace("D_Vendas_Itens", "A_Vendas_Inconsistencias");
                                conteudo.Append(" '', ");
                            }
                        }
                        else if (j == 9 || j == 25 && (workSheet.Cells[i, j].Value == null))
                        {
                            conteudo.Append(" '', ");
                        }
                        else if ((j == 11 || j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 23 || j == 24)
                           && (workSheet.Cells[i, j].Value == null))
                        {
                            if (workSheet.Cells[i, j].Value == null)
                            {
                              //  conteudo.Replace("D_Vendas_Itens", "A_Vendas_Inconsistencias");
                                conteudo.Append(" " + 0 + ", ");
                            }
                            else if (workSheet.Cells[i, j].Value == "")
                            {
                             //   conteudo.Replace("D_Vendas_Itens", "A_Vendas_Inconsistencias");
                                conteudo.Append(" " + 0 + ", ");
                               // MessageBox.Show("5");
                            }
                        }
                        else if ((j == 6 || j == 7 || j == 21) && workSheet.Cells[i, j].Value != null && workSheet.Cells[i, j].Value != "" && workSheet.Cells[i, j].Value.GetType().Name.ToString() == "DateTime")
                        {
                            conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString() + "', ");
                        }
                        else if ((j == 6 || j == 7 || j == 21) && workSheet.Cells[i, j].Value != null && workSheet.Cells[i, j].Value != "" && workSheet.Cells[i, j].Value.GetType().Name.ToString() == "Double")
                        {
                            DateTime dt = DateTime.FromOADate(Convert.ToInt64(workSheet.Cells[i, j].Value));
                            conteudo.Append("'" + dt + "', ");
                        }
                        else if ((j == 6 || j == 7 || j == 21) && workSheet.Cells[i, j].Value != null)
                        {
                            DateTime dt = DateTime.ParseExact(workSheet.Cells[i, j].Value.ToString(), "dd/MM/yyyy", null);
                            conteudo.Append("'" + dt + "', ");
                        }
                        else if ((j == 1 || j == 2 || j == 3 || j == 5 || j == 6 || j == 7 || j == 10 || j == 21 || j == 25) && (workSheet.Cells[i, j].Value == ""))
                        {
                            conteudo.Replace("D_Vendas_Itens", "A_Vendas_Inconsistencias");
                            conteudo.Append(" '', ");
                        }
                        //caso o que não é numero esteja em branco colocar texto branco
                        else if ((j == 9 || j == 11 || j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 23 || j == 24) && (workSheet.Cells[i, j].Value == ""))
                        {
                            conteudo.Append(" " + 0 + ", ");
                        }
                        //caso número seja nula colocar zero
                        else if ((j == 9 || j == 11 || j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 23 || j == 24) && (workSheet.Cells[i, j].Value == ""))
                        {
                            conteudo.Append(" " + 0 + ", ");
                        }
                        //caso número tirar  aspas simples
                        else if ((j == 11 || j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17 || j == 18 || j == 19 || j == 20 || j == 23 || j == 24) && (workSheet.Cells[i, j].Value != null))
                        {
                            conteudo.Append("" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + ", ");
                        }

                        else if (workSheet.Cells[i, j].Value != null || workSheet.Cells[i, j].Value != "")
                        {
                            if ((workSheet.Cells[i, j].Value == null ? " NULL " : workSheet.Cells[i, j].Value.GetType().Name.ToString()) == "DateTime")
                            {
                                workSheet.Cells[i, j].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern;
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                            else
                            {
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                        }
                    }
                }

                if (i == workSheet.Dimension.End.Row)
                {
                    conteudo.Append(" ) ");
                }
                else
                {
                    conteudo.Append(")");
                    conteudo.Append(Environment.NewLine);
                }
                //  Clipboard.SetText(conteudo.ToString());
                if (conn.State.ToString() == "Closed")
                {
                    conn.Open();
                }
                linha = linha + 1;
                cmd.CommandText = conteudo.ToString();
                SqlTransaction trE = null;
                trE = conn.BeginTransaction();
                cmd.Transaction = trE;
                cmd.ExecuteNonQuery();
                trE.Commit();
                conteudo.Clear();
            }

            package.Dispose();

            SqlCommand cmdArquivoCarregado = conn.CreateCommand();
            cmdArquivoCarregado.CommandText =
              "declare @tabela varchar(max) = 'D_Vendas_Itens';" +
                " if (select count(arq_id) from S_ArquivoCarregado where Arq_Tabela = @tabela) = 0" +
                " insert into S_ArquivoCarregado" +
                " (Arq_ID, Arq_Nome, Arq_Tabela, Arq_Mensagem, Arq_DataCarga, Arq_Quantidade, Arq_Login)" +
                " values(1, '\\Pasta\\Vendas.txt', @tabela, 'Carga efetuada com sucesso.'," +
                " GETDATE(), '2', SUSER_NAME())" +
                " else" +
                " insert into S_ArquivoCarregado" +
                " (Arq_ID, Arq_Nome, Arq_Tabela, Arq_Mensagem, Arq_DataCarga, Arq_Quantidade, Arq_Login)" +
                " values(isnull((select max(arq_id) from S_ArquivoCarregado where Arq_Tabela = @tabela),0)+1, '\\Pasta\\Vendas.txt', @tabela, 'Carga efetuada com sucesso.'," +
                " GETDATE(), " + linha.ToString() + ", SUSER_NAME())";

            SqlTransaction trA = null;
            trA = conn.BeginTransaction();
            cmdArquivoCarregado.Transaction = trA;
            cmdArquivoCarregado.ExecuteNonQuery();
            trA.Commit();

            conn.Close();

            MessageBox.Show("Carregamento de " + linha.ToString() + " registros de vendas");
        }

        public void Clientes()
        {
            string filePath = caminho;

            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DateTime hoje = DateTime.Now;
            package.Save();
            package.Dispose();
            int linha = 1;
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            SqlCommand cmd = conn.CreateCommand();

            if(conn.State.ToString() == "Closed")
            {
             conn.Open();
            }
            StringBuilder conteudo = new StringBuilder();
 
            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
            {
                for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    // ultima coluna
                    if (j == workSheet.Dimension.End.Column)
                    {
                        conteudo.Append(workSheet.Cells[i, j].Value == null ? "0, '" + linha + "', " : " " + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + " , '" + linha + "', ");
                        conteudo.Append(" " + pegarID("D_Clientes") + " ");
                    }

                    else
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
                        {
                            DateTimeFormat = { YearMonthPattern = "yyyy-mm-dd" }
                        };

                        if (j == 1)
                        {
                            conteudo.Append(" INSERT INTO D_CLIENTES " +
                            "(CLI_ID, " +
                            "CLI_NOME, " +
                            "CLI_PSS_ID, " +
                            "CLI_VINC, " +
                            "CLI_VINC_DT_INI, " +
                            "CLI_VINC_DT_FIM, " +
                            "CLI_CNPJ " +
                            ",[Lin_Origem_ID] " +
                            ",[Arq_Origem_ID]) " +
                            " VALUES ( ");
                            conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                        }
                        else if ((j == 5 || j == 6) && workSheet.Cells[i, j].Value != null && workSheet.Cells[i, j].Value.GetType().Name.ToString() != "DateTime")
                        {
                            DateTime dt = DateTime.FromOADate(Convert.ToInt64(workSheet.Cells[i, j].Value));
                            conteudo.Append("'" + dt + "', ");
                        }
                        else if ((j == 4) && workSheet.Cells[i, j].Value == null)
                        {
                            conteudo.Append("'N', ");
                        }
                        else
                        {
                            if ((workSheet.Cells[i, j].Value == null ? " NULL " : workSheet.Cells[i, j].Value.GetType().Name.ToString()) == "DateTime")
                            {
                                workSheet.Cells[i, j].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern;
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                            else
                            {
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                        }
                    }
                }

                if (i == workSheet.Dimension.End.Row)
                {
                    conteudo.Append(" ) ");
                }
                else
                {
                    conteudo.Append(")");
                    conteudo.Append(Environment.NewLine);
                }
                // Clipboard.SetText(conteudo.ToString());

                MessageBox.Show(conn.State.ToString());
                if (conn.State.ToString() == "Closed")
                {
                    conn.Open();
                }
                MessageBox.Show(conn.State.ToString());
                linha = linha + 1;
                cmd.CommandText = conteudo.ToString();
                SqlTransaction trE = null;
                trE = conn.BeginTransaction();
                cmd.Transaction = trE;
                cmd.ExecuteNonQuery();
                trE.Commit();
                conteudo.Clear();
               
            }
            package.Dispose();
        }
         
        public void Fornecedores()
        {
            string filePath = caminho;
            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DateTime hoje = DateTime.Now;

            package.Save();
            package.Dispose();
            int linha = 1;
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            SqlCommand cmd = conn.CreateCommand();

            if(conn.State.ToString() == "Closed")
            {
             conn.Open();
            }
            StringBuilder conteudo = new StringBuilder();

            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
            {
                for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    // ultima coluna
                    if (j == workSheet.Dimension.End.Column)
                    {
                        conteudo.Append(workSheet.Cells[i, j].Value == null ? " '', '" + linha + "', " : " '" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "' , '" + linha + "', ");
                        conteudo.Append(" " + pegarID("D_Fornecedores") + " ");
                    }
                    else
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
                        {
                            DateTimeFormat = { YearMonthPattern = "yyyy-mm-dd" }
                        };
                        if (j == 1)
                        {
                            conteudo.Append(" INSERT INTO D_fornecedores " +
                            "(FOR_ID, " +
                            "FOR_NOME, " +
                            "FOR_PSS_ID, " +
                            "FOR_VINC, " +
                            "FOR_VINC_DT_INI, " +
                            "FOR_VINC_DT_FIM, " +
                            "FOR_CNPJ, " +
                            "[Lin_Origem_ID], " +
                            "[Arq_Origem_ID]) " +
                            " VALUES ( ");
                            conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString().Replace(',', 'x') + "', ");
                        }
                        else if ((j == 5 || j == 6) && workSheet.Cells[i, j].Value != null && workSheet.Cells[i, j].Value.GetType().Name.ToString() != "DateTime")
                        {
                          String str = workSheet.Cells[i, j].Value.ToString();
                          if(str.Contains("0000"))
                            {
                                conteudo.Append(" NULL, ");
                            }
                          else
                            {
                                DateTime dt = DateTime.FromOADate(Convert.ToInt64(workSheet.Cells[i, j].Value));
                                conteudo.Append("'" + dt + "', ");
                            }
                        }
                        else if ((j == 4) && workSheet.Cells[i, j].Value == null)
                        {
                            conteudo.Append(" 'N', ");
                        }
                        else if ((j == 2) && workSheet.Cells[i, j].Value != null)
                        {
                            conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString().Replace("'","") + "',");
                        }
                        else
                        {
                            if ((workSheet.Cells[i, j].Value == null ? " NULL " : workSheet.Cells[i, j].Value.GetType().Name.ToString()) == "DateTime")
                            {
                                workSheet.Cells[i, j].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern;
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                            else
                            {
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                        }
                    }
                }
                if (i == workSheet.Dimension.End.Row)
                {
                    conteudo.Append(" ) ");
                }
                else
                {
                    conteudo.Append(")");
                    conteudo.Append(Environment.NewLine);
                }
                //Clipboard.SetText(conteudo.ToString());
                if (conn.State.ToString() == "Closed")
                {
                    conn.Open();
                }

                linha = linha + 1;
                cmd.CommandText = conteudo.ToString();
                SqlTransaction trE = null;
                trE = conn.BeginTransaction();
                cmd.Transaction = trE;
                cmd.ExecuteNonQuery();
                trE.Commit();
                conteudo.Clear();
            }
            package.Dispose();
        }


        public void Produtos()
        {
            string filePath = caminho;
            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DateTime hoje = DateTime.Now;
            string result = "";
            package.Save();
            package.Dispose();
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            StringBuilder conteudo = new StringBuilder();
            var lista = new List<String>();
            var listaRepetida = new List<String>();
            int linha = 1;
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            SqlCommand cmd = conn.CreateCommand();

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
            {
                for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    if ((j == 1) && workSheet.Cells[i, j].Value != null)
                    {
                        conteudo.Append(" INSERT INTO D_PRODUTOS " +
                                            "(PRO_ID, " +
                                            "PRO_DESCRICAO, " +
                                            "PRO_UND_ID, " +
                                            "PRO_NCM ) " + 
                                            " VALUES ( ");
                            conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString() + "', ");
                    }
                    else if ((j == 3) && workSheet.Cells[i, j].Value != null)
                    {
                        Unidades unidade = (Unidades)System.Enum.Parse(typeof(Unidades), workSheet.Cells[i, j].Value.ToString());
                        conteudo.Append("'" + ((int)unidade).ToString() + "', ");
                    }
                    else if (j == workSheet.Dimension.End.Column)
                        {
                            conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "' ");
                        }
                        else
                        {
                            conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL " : "'" + workSheet.Cells[i, j].Value.ToString() + "', ");
                        }

                }

                if (i == workSheet.Dimension.End.Row)
                {
                    conteudo.Append(" ) ");
                }
                else
                {
                    conteudo.Append(")");
                    conteudo.Append(Environment.NewLine);
                }
                Clipboard.SetText(conteudo.ToString());
                linha = linha + 1;
                cmd.CommandText = conteudo.ToString();
                SqlTransaction trE = null;
                trE = conn.BeginTransaction();
                cmd.Transaction = trE;
                cmd.ExecuteNonQuery();
                trE.Commit();
                conteudo.Clear();
            }
            package.Dispose();
        }

        public void Inventario()
        {
            string filePath = caminho;
            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DateTime hoje = DateTime.Now;
            string result = "";
            package.Save();
            package.Dispose();

            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            StringBuilder conteudo = new StringBuilder();
            var lista = new List<String>();
            var listaRepetida = new List<String>();
            int linha = 1;
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            SqlCommand cmd = conn.CreateCommand();
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
            {
              
                for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    if ((j == 1) && workSheet.Cells[i, j].Value != null)
                    {
                        conteudo.Append(" INSERT INTO D_INVENTARIO_CARGA " +
                                    "(INV_PRO_ID, " +
                                    "INV_DATA, " +
                                    "INV_QTDE, " +
                                    "INV_VALOR, " +
                                    "INV_UND_ID, " +
                                    "INV_CNPJ) " +
                                    " VALUES ( ");
                        conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString() + "', ");
                    }
                    ///ultima coluna
                    else if ((j == 5) && workSheet.Cells[i, j].Value != null)
                    {
                        Unidades unidade = (Unidades)System.Enum.Parse(typeof(Unidades), workSheet.Cells[i, j].Value.ToString());
                        conteudo.Append("'" + ((int)unidade).ToString() + "', ");
                    }
                    else if (j == workSheet.Dimension.End.Column)
                    {
                        conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "' ");
                    }
                    else
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
                        {
                            DateTimeFormat = { YearMonthPattern = "yyyy-mm-dd" }
                        };
                        if ((j == 2) && workSheet.Cells[i, j].Value != null && workSheet.Cells[i, j].Value.GetType().Name.ToString() != "DateTime")
                        {
                            DateTime dt = DateTime.FromOADate(Convert.ToInt64(workSheet.Cells[i, j].Value));
                            conteudo.Append("'" + dt + "', ");
                        }
                        else
                        {
                            if ((workSheet.Cells[i, j].Value == null ? " NULL " : workSheet.Cells[i, j].Value.GetType().Name.ToString()) == "DateTime")
                            {
                                workSheet.Cells[i, j].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern;
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                            else
                            {
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                        }
                    }
                }

                if (i == workSheet.Dimension.End.Row)
                {
                    conteudo.Append(" ) ");
                }
                else
                {
                    conteudo.Append(")");
                    conteudo.Append(Environment.NewLine);
                }
                
                linha = linha + 1;
                cmd.CommandText = conteudo.ToString();
                SqlTransaction trE = null;
                trE = conn.BeginTransaction();
                cmd.Transaction = trE;
                cmd.ExecuteNonQuery();
                trE.Commit();
                conteudo.Clear();
            }
            package.Dispose();
            MessageBox.Show("Carregamento de " + linha +" registros de inventario realizados com sucesso");
        }


        public void Insumo_Produto()
        {
            string filePath = caminho;
            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DateTime hoje = DateTime.Now;
            package.Save();
            package.Dispose();
            int linha = 1;
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();

            SqlCommand cmd = conn.CreateCommand();

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            StringBuilder conteudo = new StringBuilder();

            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
            {
                for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    if (j == workSheet.Dimension.End.Column)
                    {
                        conteudo.Append(workSheet.Cells[i, j].Value == null ? "0, '" + linha + "', " : " '" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "' , '" + linha + "', ");
                        conteudo.Append(" " + pegarID("D_Insumo_Produto") + " ");
                    }
                    else
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
                        {
                            DateTimeFormat = { YearMonthPattern = "yyyy-mm-dd" }
                        };

                        if (j == 1)
                        {
                            conteudo.Append(" INSERT INTO D_INSUMO_PRODUTO " +
                                 "([Ins_PA_Pro_Id] " +
                                  ",[Ins_Qtd_Produzida] " +
                                  ",[Ins_PA_Und_Id] " +
                                  ",[Ins_MP_Pro_Id] " +
                                  ",[Ins_Qtd_Requisitada] " +
                                  ",[Ins_MP_Und_Id] " +
                                  ",[Ins_Ordem_Prod] " +
                                  ",[Ins_DT_Ini] " +
                                  ",[Ins_DT_Fim] " +
                                  ",[Ins_CNPJ] " +
                                  ",[Lin_Origem_ID] " +
                                  ",[Arq_Origem_ID]) " +
                            " VALUES ( ");
                            conteudo.Append("'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                        }
                        else if ((j == 8 || j == 9) && workSheet.Cells[i, j].Value != null && workSheet.Cells[i, j].Value.GetType().Name.ToString() != "DateTime")
                        {
                            DateTime dt = DateTime.FromOADate(Convert.ToInt64(workSheet.Cells[i, j].Value));
                            conteudo.Append("'" + dt + "', ");
                        }
                        else if ((j == 3 || j == 6) && workSheet.Cells[i, j].Value != null)
                        {
                            Unidades unidade = (Unidades)System.Enum.Parse(typeof(Unidades), workSheet.Cells[i, j].Value.ToString());
                            conteudo.Append("'" + ((int)unidade).ToString() + "', ");
                        }
                        else
                        {
                            if ((workSheet.Cells[i, j].Value == null ? " NULL " : workSheet.Cells[i, j].Value.GetType().Name.ToString()) == "DateTime")
                            {
                                workSheet.Cells[i, j].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern;
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                            else
                            {
                                conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                            }
                        }
                    }
                }

                if (i == workSheet.Dimension.End.Row)
                {
                    conteudo.Append(" ) ");
                }
                else
                {
                    conteudo.Append(")");
                    conteudo.Append(Environment.NewLine);
                }
                //Clipboard.SetText(conteudo.ToString());
                if (conn.State.ToString() == "Closed")
                {
                    conn.Open();
                }
                linha = linha + 1;
                cmd.CommandText = conteudo.ToString();
                SqlTransaction trE = null;
                trE = conn.BeginTransaction();
                cmd.Transaction = trE;
                cmd.ExecuteNonQuery();
                trE.Commit();
                conteudo.Clear();
            }
            package.Dispose();
        }

        public void Custo()
        {
            string filePath = caminho;
            if(caminho == null)
            {
                MessageBox.Show("Escolha o arquivo");
            }

            ExcelPackage package = new ExcelPackage(new FileInfo(filePath));
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DateTime hoje = DateTime.Now;
            string result = "";
            package.Save();
            package.Dispose();

            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            StringBuilder conteudo = new StringBuilder();
            var lista = new List<String>();
            var listaRepetida = new List<String>();
            int linha = 1;
            package = new ExcelPackage(new FileInfo(filePath));
            workSheet = package.Workbook.Worksheets.First();
            SqlCommand cmd = conn.CreateCommand();

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
            {
                for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                {
                    if ((j == 1) )
                    {

                        conteudo.Append(" INSERT INTO D_Custo_Medio " +
                            "(Cst_Pro_Id, " +
                            "Cst_Mes, " +
                            "Cst_Ano, " +
                            "Cst_Vl_Custo, " +
                            "Cst_CNPJ " +
                            ",[Lin_Origem_ID] " +
                            ",[Arq_Origem_ID]) " +
                            " VALUES ( ");
                    }
                    if (j == workSheet.Dimension.End.Column)
                    {
                        conteudo.Append(workSheet.Cells[i, j].Value == null ? "0, '" + linha + "', " : " '" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "' , '" + linha + "', ");
                        conteudo.Append(" " + pegarID("D_Custo_Medio") + " ");
                    }
                    else
                    {
                        if ((workSheet.Cells[i, j].Value == null ? " NULL " : workSheet.Cells[i, j].Value.GetType().Name.ToString()) == "DateTime")
                        {
                            workSheet.Cells[i, j].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern;
                            conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                        }
                        else
                        {
                            conteudo.Append(workSheet.Cells[i, j].Value == null ? " NULL, " : "'" + workSheet.Cells[i, j].Value.ToString().Replace(',', '.') + "', ");
                        }
                    }
                }
                if (i == workSheet.Dimension.End.Row)
                {
                    conteudo.Append(" ) ");
                }
                else
                {
                    conteudo.Append(")");
                    conteudo.Append(Environment.NewLine);
                }
                //Clipboard.SetText(conteudo.ToString());

                if (conn.State.ToString() == "Closed")
                {
                    conn.Open();
                }

                linha = linha + 1;
                cmd.CommandText = conteudo.ToString();
                SqlTransaction trE = null;
                trE = conn.BeginTransaction();
                cmd.Transaction = trE;
                cmd.ExecuteNonQuery();
                trE.Commit();
                conteudo.Clear();
            }
            package.Dispose();

            SqlCommand cmdArquivoCarregado = conn.CreateCommand();
            cmdArquivoCarregado.CommandText =
              "declare @tabela varchar(max) = 'D_Custo_Medio';" +
                " if (select count(arq_id) from S_ArquivoCarregado where Arq_Tabela = @tabela) = 0" +
                " insert into S_ArquivoCarregado" +
                " (Arq_ID, Arq_Nome, Arq_Tabela, Arq_Mensagem, Arq_DataCarga, Arq_Quantidade, Arq_Login)" +
                " values(1, 'Custos_Complementares', @tabela, 'Carga efetuada com sucesso.'," +
                " GETDATE(), '2', SUSER_NAME())" +
                " else" +
                " insert into S_ArquivoCarregado" +
                " (Arq_ID, Arq_Nome, Arq_Tabela, Arq_Mensagem, Arq_DataCarga, Arq_Quantidade, Arq_Login)" +
                " values(isnull((select max(arq_id) from S_ArquivoCarregado where Arq_Tabela = @tabela),0)+1, '\\Pasta\\Vendas.txt', @tabela, 'Carga efetuada com sucesso.'," +
                " GETDATE(), " + linha.ToString() + ", SUSER_NAME())";

            SqlTransaction trA = null;
            trA = conn.BeginTransaction();
            cmdArquivoCarregado.Transaction = trA;
            cmdArquivoCarregado.ExecuteNonQuery();
            trA.Commit();

            conn.Close();

            MessageBox.Show("Carregamento de " + linha.ToString() + " registros de  Custo Médio");
        }


        private void button1_Click_2(object sender, EventArgs e)
        {
            if(conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            if (cmbTabela.SelectedItem == "D_Custo_Medio")
            {
                Custo();
            }
            if (cmbTabela.SelectedItem == "D_Clientes")
            {
                Clientes();
            }
            if (cmbTabela.SelectedItem == "D_Produtos")
            {
                Produtos();
            }
            if (cmbTabela.SelectedItem == "D_Inventario_Carga")
            {
                Inventario();
            }
            if (cmbTabela.SelectedItem == "D_Compras")
            {
                Compras();
            }
            if (cmbTabela.SelectedItem == "D_Vendas_Itens")
            {
                Vendas();
            }
            if (cmbTabela.SelectedItem == "D_Fornecedores")
            {
                Fornecedores();
            }
            if (cmbTabela.SelectedItem == "D_Insumo_Produto")
            {
                Insumo_Produto();
            }
            if (cmbTabela.SelectedItem == "D_Relacao_Carga")
            {
              
            }
            if(cmbTabela.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma tabela para carregamento");
            }


        }

        enum Moedas
        {
        AFA = 5,
        ETB = 8,
        ARG = 10,
        THB = 15,
        PAB = 20,
        VEB = 25,
        BOB = 30,
        GHC = 35,
        CRC = 40,
        SVC = 45,
        NIC = 50,
        NIO = 51,
        DKK = 55,
        EEK = 57,
        SKK = 58,
        ISK = 60,
        NOK = 65,
        SEK = 70,
        CZK = 75,
        NCZ = 78,
        CZ = 79,
        CR = 80,
        RUR = 88,
        GMD = 90,
        DZD = 95,
        KWD = 100,
        BHD = 105,
        YD = 110,
        IQD = 115,
        DIN = 120,
        JOD = 125,
        LYD = 130,
        MKD = 132,
        SDD = 134,
        TND = 135,
        SDR = 138,
        MAD = 139,
        AED = 145,
        STD = 148,
        AUD = 150,
        BSD = 155,
        BMD = 160,
        CAD = 165,
        GYD = 170,
        BBD = 175,
        BZD = 180,
        BND = 185,
        KYD = 190,
        SGD = 195,
        FJD = 200,
        HKD = 205,
        TTD = 210,
        XCD = 215,
        ZWD = 217,
        USD = 220,
        JMD = 230,
        LRD = 235,
        M = 240,
        NZD = 245,
        SBD = 250,
        VND = 260,
        GRD = 270,
        CVE = 295,
        ESC = 315,
        TPE = 320,
        ANG = 325,
        AWG = 328,
        SRG = 330,
        NLG = 335,
        HUF = 345,
        BEF = 360,
        FBF = 361,
        BIF = 365,
        KMF = 368,
        XAF = 370,
        XPF = 380,
        DJF = 390,
        FRF = 395,
        GNF = 398,
        LUF = 400,
        MGF = 405,
        MF = 410,
        RWF = 420,
        CHF = 425,
        HTG = 440,
        PYG = 450,
        UAH = 460,
        JPY = 470,
        I = 480,
        GEL = 482,
        LVL = 485,
        ALL = 490,
        HNL = 495,
        SLL = 500,
        MDL = 503,
        ROL = 505,
        BGL = 510,
        CYP = 520,
        GIP = 530,
        EGP = 535,
        GBP = 540,
        FKP = 545,
        IEP = 550,
        IL = 555,
        LBP = 560,
        MTL = 565,
        SHP = 570,
        SYP = 575,
        LSD = 580,
        SZL = 585,
        ITL = 595,
        TRL = 600,
        LTL = 601,
        LSL = 603,
        AZM = 607,
        DEM = 610,
        BAM = 612,
        FMK = 615,
        MZM = 620,
        NGN = 630,
        AON = 635,
        YUM = 637,
        TWD = 640,
        MXN = 645,
        NCÇ = 651,
        PEN = 660,
        BTN = 665,
        MRO = 670,
        TOP = 680,
        MOP = 685,
        ADP = 690,
        ESP = 700,  
        ARS = 706, 
        B = 710, 
        CLP = 715, 
        COP = 720, 
        CUP = 725, 
        DOP = 730, 
        PHP = 735, 
        GWP = 738, 
        MEX = 740, 
        UYP = 745, 
        BWP = 755, 
        MWK = 760, 
        ZMK = 765, 
        GTQ = 770, 
        MMK = 775, 
        UAK = 776, 
        PGK = 778, 
        HRK = 779, 
        LAK = 780, 
        ZAR = 785, 
        BRL = 790, 
        CNY = 795, 
        QAR = 800, 
        OMR = 805, 
        YER = 810, 
        IRR = 815, 
        SAR = 820, 
        KHR = 825, 
        MYR = 828, 
        BYB = 829, 
        RUB = 830, 
        TJR = 835, 
        MUR = 840, 
        NPR = 845, 
        SCR = 850, 
        LKR = 855, 
        INR = 860, 
        IDR = 865, 
        MVR = 870, 
        PKR = 875, 
        ILS = 880, 
        S = 890, 
        UZS = 893, 
        ECS = 895, 
        BDT = 905, 
        WS = 910, 
        WST = 911, 
        KZT = 913, 
        SIT = 914, 
        MNT = 915, 
        XEU = 918, 
        VUV = 920, 
        KPW = 925, 
        KRW = 930, 
        ATS = 940, 
        TSH = 945,
        TZS = 946,
        KES = 950,
        UGX = 955,
        SOS = 960,
        ZRN = 970,
        PZN = 975,
        EUR = 978,
        CLRDA = 980, 
        CLBULG = 982, 
        CLGREC = 983, 
        CLHUNG = 984, 
        CLISR = 986, 
        CLIUG = 988, 
        CLPOL = 990, 
        CLROM = 992, 
        BUA = 995, 
        FUA = 996, 
        XAU = 998
        }



        enum Unidades
        {
            NULL = 11,
            X8 = 11,
            BD = 11,
            BL = 11,
            BR = 11,
            CAIXA = 11,
            CENTO = 11,
            CJ = 11,
            CM = 14,
            CT = 11,
            CX = 11,
            FD = 11,
            G = 22,
            GALAO = 11,
            H = 11,
            HORA = 11,
            HORAS = 11,
            JG = 10,
            KAR = 23,
            KG = 10,
            KI = 12,
            KILOMETROS = 14,
            L = 17,
            LATAO = 11,
            LB = 14,
            LITRO = 17,
            LITROS = 17,
            M = 14,
            M2 = 15,
            M3 = 16,
            METR2 = 15,
            METRO = 14,
            METROS3 = 16,
            ML = 12,
            MT = 14,
            ND = 11,
            OUTROS = 11,
            PA = 13,
            PAK = 23,
            PAL = 23,
            PARCELAS = 11,
            PC = 11,
            PECA = 11,
            PR = 13,
            PT = 11,
            RL = 11,
            SRV = 11,
            ST = 23,
            SV = 11,
            TO = 21,
            TON = 23,
            TR = 11,
            UN = 11,
            UNI = 23,
            UNIDA = 11,
            VOLUM = 11,
            x8 = 11,
            bd = 11,
            bl = 11,
            br = 11,
            caixa = 11,
            cento = 11,
            cj = 11,
            cm = 14,
            ct = 11,
            cx = 11,
            fd = 11,
            g = 22,
            galao = 11,
            h = 11,
            hora = 11,
            horas = 11,
            jg = 10,
            kar = 23,
            kg = 10,
            ki = 12,
            kilometros = 14,
            l = 17,
            latao = 11,
            lb = 14,
            litro = 17,
            litros = 17,
            m = 14,
            m2 = 15,
            m3 = 16,
            metr2 = 15,
            metro = 14,
            metros3 = 16,
            ml = 12,
            mt = 14,
            nd = 11,
            outros = 11,
            pa = 13,
            pak = 23,
            pal = 23,
            parcelas = 11,
            pc = 11,
            peca = 11,
            pr = 13,
            pt = 11,
            rl = 11,
            srv = 11,
            st = 23,
            sv = 11,
            to = 21,
            ton = 23,
            tr = 11,
            un = 11,
            uni = 23,
            unida = 11,
            volum = 11,
            Null = 11,
            Bd = 11,
            Bl = 11,
            Br = 11,
            Caixa = 11,
            Cento = 11,
            Cj = 11,
            Cm = 14,
            Ct = 11,
            Cx = 11,
            Fd = 11,
            Galao = 11,
            Hora = 11,
            Horas = 11,
            Jg = 10,
            Kar = 23,
            Kg = 10,
            Ki = 12,
            Kilometros = 14,
            Latao = 11,
            Lb = 14,
            Litro = 17,
            Litros = 17,
            Metr2 = 15,
            Metro = 14,
            Metros3 = 16,
            Ml = 12,
            Mt = 14,
            Nd = 11,
            Outros = 11,
            Pa = 13,
            Pak = 23,
            Pal = 23,
            Parcelas = 11,
            Pc = 11,
            Peca = 11,
            Pr = 13,
            Pt = 11,
            Rl = 11,
            Srv = 11,
            St = 23,
            Sv = 11,
            To = 21,
            Ton = 23,
            Tr = 11,
            Un = 11,
            Uni = 23,
            Unida = 11,
            Volum = 11
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
 
        }

        private void cmbTabela_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxServidor_SelectedIndexChanged(object sender, EventArgs e)
        {
            conexao = comboBoxServidor.Text;
              conn = new SqlConnection ("Data Source=" + conexao + "; Integrated Security=True;");
            // conn = new SqlConnection(@"Data Source=BRCAENRODRIGUES\SQLSERVER; Initial Catalog=TEST_TEMP_2017; Integrated Security=True");

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            System.Data.DataTable databases = conn.GetSchema("Databases");

            comboBoxBase.Items.Clear();

            foreach (DataRow database in databases.Rows)
            {
                databaseName = database.Field<String>("database_name");
                comboBoxBase.Items.Add(databaseName);
               
            }
            conn.Close();
        }


        private void comboBoxServidor_Enter(object sender, EventArgs e)
        {
            comboBoxServidor.Items.Clear();
            string myServer = Environment.MachineName;

            System.Data.DataTable servers = SqlDataSourceEnumerator.Instance.GetDataSources();
            for (int i = 0; i < servers.Rows.Count; i++)
            {
                if (myServer == servers.Rows[i]["ServerName"].ToString())
                {
                    if ((servers.Rows[i]["InstanceName"] as string) != null)
                        comboBoxServidor.Items.Add(servers.Rows[i]["ServerName"] + "\\" + servers.Rows[i]["InstanceName"]);
                    else
                        comboBoxServidor.Items.Add(servers.Rows[i]["ServerName"]);
                }
            }
        }

        private void comboBoxBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            conn = new SqlConnection("Data Source=" + conexao + "; Integrated Security=True; Initial Catalog=" + databaseName);

        }
    }
}
