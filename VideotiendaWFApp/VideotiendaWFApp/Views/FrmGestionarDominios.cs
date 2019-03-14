using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideotiendaWFApp.Models;

namespace VideotiendaWFApp.Views
{
    public partial class FrmGestionarDominios : Form
    {

        dominios oDominio = null;
        private string tipoDominio;
        private string idDominio;

        public FrmGestionarDominios(string tipoDominio, string idDominio)
        {
            //Dibujar el formulario
            InitializeComponent();

            //Recibir los datos de la PK (si son nulos estamos insertando, si hay datos estamos editando)
            //Si hay datos estamos editando
            this.tipoDominio = tipoDominio;
            this.idDominio = idDominio;
            
            //Si hay datos (edición) llamamos a cargarDatos()
            if (!string.IsNullOrEmpty(this.idDominio) &&
                !string.IsNullOrEmpty(this.tipoDominio))
            {
                //Si es modo edición, bloqueamos los TextBox de la PK
                cargarDatos();
                this.txtTipo.ReadOnly = true;
                this.txtId.ReadOnly = true;
            }
            else
            {
                //Si es modo inserción habilitamos los TextBox de la PK
                this.txtTipo.ReadOnly = false;
                this.txtId.ReadOnly = false;
            }
        }

        private void FrmGestionarDominios_Load(object sender, EventArgs e)
        {
            this.txtTipo.Select();
        }

        private void cargarDatos()
        {
            using (videotiendaEntities db = new videotiendaEntities())
            {
                oDominio = db.dominios.Find(tipoDominio, idDominio);
                txtTipo.Text = oDominio.TIPO_DOMINIO;
                txtId.Text = oDominio.ID_DOMINIO;
                txtValor.Text = oDominio.VLR_DOMINIO;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validar que todos los campos obligatorios se hayan diligenciado
            if(string.IsNullOrEmpty(this.txtId.Text) || 
                string.IsNullOrEmpty(this.txtTipo.Text)  ||
                string.IsNullOrEmpty(this.txtValor.Text))
            {
                MessageBox.Show("Los Campos marcados con asterisco (*) son obligatorios");
            }
            else
            {
                //Establecer conexión con la BD a través de EF
                using (videotiendaEntities db = new videotiendaEntities())
                {
                    //Si estamos en modo inserción, inicializamos el objeto dominio
                    if(this.tipoDominio == null && this.idDominio == null)
                    {
                        oDominio = new dominios();
                    }
                    
                    oDominio.TIPO_DOMINIO = this.txtTipo.Text;
                    oDominio.ID_DOMINIO = this.txtId.Text;
                    oDominio.VLR_DOMINIO = this.txtValor.Text;
                    //En modo inserción, adicionamos un nuevo registro
                    if(this.tipoDominio == null && this.idDominio == null)
                    {
                        db.dominios.Add(oDominio);
                    }
                    else
                    {
                        //En modo edición, cambiamos el estado del objeto a modificación
                        db.Entry(oDominio).State = System.Data.Entity.EntityState.Modified;
                       
                    }
                    db.SaveChanges();
                    //Cerrar el formulario y volver al formulario de datos
                    this.Close();

                }
            }
        }
    }
}
