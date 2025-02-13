using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Zorgdossier.Helpers;
using Zorgdossier.Views.SectieViews;
using ZstdSharp.Unsafe;

namespace Zorgdossier.Models
{
    [Table("BasicInformation")]
    public class BasicInformation : ObservableObject
    {
        #region Fields
        private int _dossierId;
        private string _name = string.Empty;
        private string _complaint = string.Empty;
        private string _gender = string.Empty;
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

        [StringLength(50), Required]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        [StringLength(50), Required]
        public string Complaint
        {
            get { return _complaint; }
            set { _complaint = value; OnPropertyChanged(); }
        }

        [StringLength(20), Required]
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(); }
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