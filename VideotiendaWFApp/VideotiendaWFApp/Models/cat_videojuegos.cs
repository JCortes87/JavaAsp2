//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VideotiendaWFApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class cat_videojuegos
    {
        public decimal ID_CAT_VIDEOJUEGOS { get; set; }
        public Nullable<decimal> ID_CATEGORIA { get; set; }
        public Nullable<decimal> NRO_REFERENCIA { get; set; }
    
        public virtual categorias categorias { get; set; }
        public virtual videojuegos videojuegos { get; set; }
    }
}