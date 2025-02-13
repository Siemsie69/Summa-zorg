using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zorgdossier.Helpers;

namespace Zorgdossier.Models
{
    [Table("Organ")]
    public class Organ : ObservableObject
    {
        #region Fields
        private int _dossierId;
        private string _organs = string.Empty;
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required]
        public int DossierId
        {
            get { return _dossierId; }
            set { _dossierId = value; OnPropertyChanged(); }
        }

        [Required]
        public string Organs
        {
            get { return _organs; }
            set { _organs = value; OnPropertyChanged(); }
        }
        #endregion

        #region Relations
        [ForeignKey(nameof(DossierId))]
        public Dossier Dossier
        {
            get; set;
        }
        #endregion
    }
}
