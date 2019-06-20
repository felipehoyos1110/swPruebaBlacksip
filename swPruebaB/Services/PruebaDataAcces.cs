using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swPruebaB.Models;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;

namespace swPruebaB.Services
{
    public class PruebaDataAcces
    {
        public Prueba[] GetAllPrueba()
        {
            bool bOcurrioExcepcion = false;
            DataSet dsDatosRetorno;
            dsDatosRetorno = new DataSet("GetAllPrueba");
            Prueba[] resultadoPrueba;

            try
            {
                // Tomar info de conexion del web.config
                using (OracleConnection conDB = new OracleConnection(ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString))
                {
                    // Conectar
                    conDB.Open();

                    // Construir llamado a procedimiento
                    OracleCommand cmd = conDB.CreateCommand();
                    cmd.CommandText = "Prueba.ConsultarContactos";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Paso de parametros
                    cmd.Parameters.Add("pEstado", OracleDbType.Varchar2).Value = "true";
                    cmd.Parameters.Add("pResultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    // Ejecutar procedimiento y llenar dataset
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dsDatosRetorno);
                    conDB.Dispose();
                    conDB.Close();
                }


                DataTable dtRespuesta;

                dtRespuesta = dsDatosRetorno.Tables[0];

                resultadoPrueba = new Prueba[dtRespuesta.Rows.Count];

                for (int i = 0; i < dtRespuesta.Rows.Count; i++)
                {
                    Prueba prueba = new Prueba();
                    // RespuestaMetodo no es la primera tabla asi que esta retornando normal
                    prueba.Nombre = dtRespuesta.Rows[i]["Nombre"].ToString();
                    prueba.Apellido = dtRespuesta.Rows[i]["Apellido"].ToString();

                    resultadoPrueba[i] = prueba;
                }                

            }

            catch (Exception ex)
            {
                bOcurrioExcepcion = true;
                resultadoPrueba = new Prueba[1];
            }

            return resultadoPrueba;

        }


        public bool SavePrueba(Prueba prueba)
        {

            bool bOcurrioExcepcion = false;
            DataSet dsDatosRetorno;
            dsDatosRetorno = new DataSet("SavePrueba");

            try
            {
                // Tomar info de conexion del web.config
                using (OracleConnection conDB = new OracleConnection(ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString))
                {
                    // Conectar
                    conDB.Open();

                    // Construir llamado a procedimiento
                    OracleCommand cmd = conDB.CreateCommand();
                    cmd.CommandText = "Prueba.CrearContacto";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Paso de parametros
                    cmd.Parameters.Add("pNombre", OracleDbType.Varchar2).Value = prueba.Nombre;
                    cmd.Parameters.Add("pApellido", OracleDbType.Varchar2).Value = prueba.Apellido;
                    cmd.Parameters.Add("pResultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    // Ejecutar procedimiento y llenar dataset
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dsDatosRetorno);
                    conDB.Dispose();
                    conDB.Close();
                }


                DataTable dtRespuesta;

                dtRespuesta = dsDatosRetorno.Tables[0];

                if ((decimal)dtRespuesta.Rows[0]["IDRetorno"] == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                return false;
            }


        }




        }
}