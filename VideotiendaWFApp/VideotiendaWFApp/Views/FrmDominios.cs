﻿using System;
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
    public partial class FrmDominios : Form
    {
        public FrmDominios()
        {
            InitializeComponent();
        }

        private void FrmDominios_Load(object sender, EventArgs e)
        {
            refrescarTabla();
            this.txtTipo.Select();
        }
#region Helper
        private void refrescarTabla()
        {
            using (videotiendaEntities db = new videotiendaEntities())
            {
                var lstDominios = from d in db.dominios select d;
                grDatos.DataSource = lstDominios.ToList();
            }
        }

        private dominios getSelectedItem()
        {
            //Inicializar objeto para almacenar dominio seleccionado en la tabla
            dominios d = new dominios();

            try
            {
                //Obtener los valores de la PK del dominio seleccionado en la tabla
                d.TIPO_DOMINIO = 
                    grDatos.Rows[grDatos.CurrentRow.Index].Cells[0].Value.ToString();
                d.ID_DOMINIO = 
                    grDatos.Rows[grDatos.CurrentRow.Index].Cells[1].Value.ToString();
                //Retornar objeto con datos del dominio seleccionado en la tabla
                return d;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtTipo.Text = "";
            this.txtId.Text = "";
            this.txtValor.Text = "";
            refrescarTabla();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (videotiendaEntities db = new videotiendaEntities())
            {
                //Consultar todos los dominios
                var lstDominios = from d in db.dominios select d;
                //Aplicar filtros ingresados por el usuario
                if (!string.IsNullOrEmpty(this.txtTipo.Text)) 
                {
                    lstDominios = lstDominios.Where(d => d.TIPO_DOMINIO.Contains(this.txtTipo.Text));
                }
                if (!string.IsNullOrEmpty(this.txtId.Text))
                {
                    lstDominios = lstDominios.Where(d => d.ID_DOMINIO.Contains(this.txtId.Text));
                }
                if (!string.IsNullOrEmpty(this.txtValor.Text))
                {
                    lstDominios = lstDominios.Where(d => d.VLR_DOMINIO.Contains(this.txtValor.Text));
                }
                grDatos.DataSource = lstDominios.ToList();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Views.FrmGestionarDominios frmGestionarDominios = new Views.FrmGestionarDominios(null, null);
            frmGestionarDominios.ShowDialog();

            refrescarTabla();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            //btener dominio que se seleccionó en la tabla para editar
            dominios d = getSelectedItem();
            //hubo selección?
            if(d != null)
            {
                //inicializar formulario de edición de dominios
                Views.FrmGestionarDominios frmGestionarDominios = new Views.FrmGestionarDominios(d.TIPO_DOMINIO, d.ID_DOMINIO);
                //Abrir formulario de edición de dominios
                frmGestionarDominios.ShowDialog();
                //Refrescar tabla cuando regrese del formulario de edición
                refrescarTabla();
            }
        }

        private void grDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //Obtener el dominio que se va a eliminar 
            dominios d = getSelectedItem();
            //Hubo selección?
            if (d != null)
            {
                if (MessageBox.Show("Esta seguro que desea elminar este registro?",
                    "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1)==
                    System.Windows.Forms.DialogResult.Yes)
                    {
                    //EStablecer conexión con la base de datos a través de EntityFramework
                    using (videotiendaEntities db = new videotiendaEntities())
                    {
                        //Buscar el dominio en la BD
                        dominios dEliminar = db.dominios.Find(d.TIPO_DOMINIO, d.ID_DOMINIO);
                        //Eliminar el dominio de la tabla 
                        db.dominios.Remove(dEliminar);
                        //Confirmar cambios en la base de datos
                        db.SaveChanges();
                    }
                    //Actualizar la tabla de datos
                    this.refrescarTabla();
                }
            }
        }
    }
}
