using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Zorgdossier.Helpers;

namespace Zorgdossier.Models
{
    [Table("Phone")]
    public class Phone : ObservableObject
    {
        #region Fields
        private int _dossierId;
        private string _phoneSummary = string.Empty;
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

        [StringLength(720), Required]
        public string PhoneSummary
        {
            get { return _phoneSummary; }
            set { _phoneSummary = value; OnPropertyChanged(); }
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