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
    [Table("ContactAdvice")]
    public class ContactAdvice : ObservableObject
    {
        #region Fields
        private int _dossierId;
        private string _advice = string.Empty;
        private string? _contactAdviceText = string.Empty;
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
        public string Advice
        {
            get { return _advice; }
            set { _advice = value; OnPropertyChanged(); }
        }

        [StringLength(320)]
        public string? ContactAdviceText
        {
            get { return _contactAdviceText; }
            set { _contactAdviceText = value; OnPropertyChanged(); }
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